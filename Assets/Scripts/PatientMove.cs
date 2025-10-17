using UnityEngine;
using System.Collections;
using UnityEngine.Events; // Event sistemi için ekledik

public class PatientMove : MonoBehaviour
{
    [SerializeField] public Vector3 startScale = Vector3.zero;
    [SerializeField] public float animationTime = 2f;
    [SerializeField] public float swayAmount = 0.15f;
    [SerializeField] public Vector3 targetScale = Vector3.one;
    
    // Animasyon bittiğinde tetiklenecek event
    public UnityEvent onAnimationComplete;
    
    private SpriteRenderer spriteRenderer;
    private Color startColor;

    void Start()
    {
        // Sprite Renderer'ı al ve alfa değerini 0 yap
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            startColor = spriteRenderer.color;
            startColor.a = 0f;
            spriteRenderer.color = startColor;
        }

        // PanelManager'ı bul ve onAnimationComplete event'ine bağla
        PanelManager panelManager = FindObjectOfType<PanelManager>();
        if (panelManager != null)
        {
            onAnimationComplete.AddListener(panelManager.ShowFirstPanel);
        }

        transform.localScale = startScale;
        StartCoroutine(AnimatePatient());
    }

    IEnumerator AnimatePatient()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < animationTime)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / animationTime;

            // Alfa değerini yavaşça artır (0'dan 1'e)
            if (spriteRenderer != null)
            {
                Color newColor = spriteRenderer.color;
                newColor.a = Mathf.Lerp(0f, 1f, normalizedTime);
                spriteRenderer.color = newColor;
            }

            // Ölçek animasyonu
            transform.localScale = Vector3.Lerp(startScale, targetScale, normalizedTime);

            // Sallanma hareketi
            float swayOffset = Mathf.Sin(normalizedTime * Mathf.PI * 2) * swayAmount;
            Vector3 newPosition = startPosition + new Vector3(swayOffset, 0f, 0f);
            transform.position = newPosition;

            yield return null;
        }

        // Son değerleri ayarla
        transform.position = startPosition;
        transform.localScale = targetScale;
        if (spriteRenderer != null)
        {
            Color finalColor = spriteRenderer.color;
            finalColor.a = 1f;
            spriteRenderer.color = finalColor;
        }

        // Eventi tetikle
        onAnimationComplete?.Invoke();
    }
}
