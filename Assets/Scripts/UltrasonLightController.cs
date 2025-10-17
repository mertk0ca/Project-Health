using UnityEngine;
 // 2D Işık sistemi için

public class UltrasonLightController : MonoBehaviour
{
    [Header("Işık Ayarları")]
    [SerializeField] private float isikYaricap = 2f;
    [SerializeField] private float isikYumusaklik = 0.5f;
    [SerializeField] private float isikYogunluk = 1f;
    [SerializeField] private float hareketYumusakligi = 10f;

    private Camera mainCamera;
    private UnityEngine.Rendering.Universal.Light2D spotIsik;

    private void Start()
    {
        mainCamera = Camera.main;

        // Işık bileşenini ayarla
        spotIsik = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        if (spotIsik == null)
        {
            spotIsik = gameObject.AddComponent<UnityEngine.Rendering.Universal.Light2D>();
        }

        // Işık özelliklerini ayarla
        spotIsik.pointLightOuterRadius = isikYaricap;
        spotIsik.pointLightInnerRadius = isikYaricap * isikYumusaklik;
        spotIsik.intensity = isikYogunluk;
        spotIsik.lightType = UnityEngine.Rendering.Universal.Light2D.LightType.Point;
    }

    private void Update()
    {
        // Mouse pozisyonunu al ve dünya koordinatlarına çevir
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // Yumuşak hareket ile ışığı mouse'a doğru hareket ettir
        transform.position = Vector3.Lerp(transform.position, mousePos, Time.deltaTime * hareketYumusakligi);
    }

    // Işık özelliklerini değiştirmek için public metodlar
    public void IsikYaricapiniAyarla(float yeniYaricap)
    {
        isikYaricap = yeniYaricap;
        if (spotIsik != null)
        {
            spotIsik.pointLightOuterRadius = yeniYaricap;
            spotIsik.pointLightInnerRadius = yeniYaricap * isikYumusaklik;
        }
    }

    public void IsikYogunlugunuAyarla(float yeniYogunluk)
    {
        isikYogunluk = yeniYogunluk;
        if (spotIsik != null)
        {
            spotIsik.intensity = yeniYogunluk;
        }
    }
} 