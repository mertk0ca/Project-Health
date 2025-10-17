using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Events; // Events için eklendi

public class ExaminationManager : MonoBehaviour
{
    [Header("Ses Ayarları")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip dogruSesEfekti;

    [Header("UI Elemanları")]
    [SerializeField] private GameObject tepkiYokTexti;
    [SerializeField] private Canvas canvas;

    [Header("Panel Ayarları")]
    public UnityEvent panelDegisimOlayi; // Panel değişimi için event
    
    private void Start()
    {
        // Başlangıçta tepki yazısını gizle
        if (tepkiYokTexti != null)
            tepkiYokTexti.SetActive(false);
            
        // Canvas referansını otomatik bul
        if (canvas == null)
            canvas = FindObjectOfType<Canvas>();
            
        // AudioSource komponentini otomatik bul veya ekle
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
            }
        }
    }

    public void DogruButonaTiklandi()
    {
        Debug.Log("Doğru butona tıklandı");
        
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource bulunamadı!");
            return;
        }
        
        if (dogruSesEfekti == null)
        {
            Debug.LogWarning("Ses efekti atanmamış!");
            return;
        }

        audioSource.volume = 1f; // Ses seviyesini maksimuma ayarla
        audioSource.PlayOneShot(dogruSesEfekti);
        Debug.Log("Ses çalınıyor: " + dogruSesEfekti.name);
    }

    public void YanlisButonaTiklandi()
    {
        if (canvas != null && tepkiYokTexti != null)
        {
            Vector2 mousePos = Input.mousePosition;
            
            // Canvas ölçeklendirme faktörünü hesapla
            float scaleFactor = canvas.scaleFactor;
            
            // RectTransform bileşenini al
            RectTransform tepkiRectTransform = tepkiYokTexti.GetComponent<RectTransform>();
            
            // Fare pozisyonunu canvas koordinatlarına çevir
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                tepkiRectTransform.position = mousePos;
            }
            else if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                Vector2 pos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.GetComponent<RectTransform>(),
                    mousePos,
                    canvas.worldCamera,
                    out pos);
                tepkiRectTransform.localPosition = pos;
            }
            
            StartCoroutine(TepkiYokGoster());
        }
    }

    private IEnumerator TepkiYokGoster()
    {
        if (tepkiYokTexti != null)
        {
            tepkiYokTexti.SetActive(true);
            
            // 2 saniye bekle
            yield return new WaitForSeconds(2f);
            
            // Yazıyı gizle
            tepkiYokTexti.SetActive(false);
        }
    }
} 