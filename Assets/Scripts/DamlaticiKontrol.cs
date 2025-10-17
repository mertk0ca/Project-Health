using UnityEngine;

public class DamlaticiKontrol : MonoBehaviour
{
    [Header("Damlatıcı Ayarları")]
    [SerializeField] private GameObject damlaPrefab;
    [SerializeField] private Transform damlaNoktasi;
    [SerializeField] private float damlaAraligi = 1f;
    [SerializeField] private float hareketYumusakligi = 5f;
    [SerializeField] private float maxYukseklik = 4f;
    [SerializeField] private float minYukseklik = -4f;

    private Camera mainCamera;
    private float sonrakiDamlaSuresi;

    private void Start()
    {
        mainCamera = Camera.main;
        sonrakiDamlaSuresi = Time.time + damlaAraligi;
    }

    private void Update()
    {
        MouseTakibi();
        DamlaOlustur();
    }

    private void MouseTakibi()
    {
        // Mouse pozisyonunu dünya koordinatlarına çevir
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // Y pozisyonunu sınırla
        mousePos.y = Mathf.Clamp(mousePos.y, minYukseklik, maxYukseklik);

        // Yumuşak hareket
        transform.position = Vector3.Lerp(transform.position, mousePos, Time.deltaTime * hareketYumusakligi);
    }

    private void DamlaOlustur()
    {
        if (Time.time >= sonrakiDamlaSuresi)
        {
            // Yeni damla oluştur
            if (damlaNoktasi != null && damlaPrefab != null)
            {
                Instantiate(damlaPrefab, damlaNoktasi.position, Quaternion.identity);
            }
            
            // Sonraki damla zamanını ayarla
            sonrakiDamlaSuresi = Time.time + damlaAraligi;
        }
    }
} 