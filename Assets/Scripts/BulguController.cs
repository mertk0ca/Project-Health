using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BulguController : MonoBehaviour
{
    [System.Serializable]
    public class BulguAlani
    {
        public string bulguAdi;
        public Transform alan;
        public TextMeshProUGUI bulguText;
        public float aktivasyonMesafesi = 2f;
        public bool goruldu = false; // Bulgunun görülüp görülmediğini takip etmek için
    }

    [Header("Bulgu Alanları")]
    [SerializeField] private BulguAlani[] bulguAlanlari;

    [Header("Genel Ayarlar")]
    [SerializeField] private float kontrolAraligi = 0.1f; // Performans için kontrol aralığı
    [SerializeField] private Button devamButonu;
    [SerializeField] private TextMeshProUGUI ilerlemeText; // Yeni eklenen ilerleme texti

    private Camera mainCamera;
    private float sonKontrolZamani;
    private bool tumBulgularTamamlandi = false;
    private int gorulenBulguSayisi = 0; // Yeni eklenen sayaç

    private void Start()
    {
        mainCamera = Camera.main;
        
        // Başlangıçta tüm textleri gizle
        foreach (var bulgu in bulguAlanlari)
        {
            if (bulgu.bulguText != null)
            {
                bulgu.bulguText.gameObject.SetActive(false);
                bulgu.goruldu = false;
            }
        }

        // Devam butonunu başlangıçta deaktif et
        if (devamButonu != null)
        {
            devamButonu.gameObject.SetActive(false);
        }

        // İlerleme textini başlangıçta güncelle
        IlerlemeTextiniGuncelle();
    }

    private void Update()
    {
        // if (tumBulgularTamamlandi) return; // Bu satırı kaldırıyorum
        // Performans için kontrol aralığı
        if (Time.time - sonKontrolZamani < kontrolAraligi) return;
        sonKontrolZamani = Time.time;

        // Mouse pozisyonunu dünya koordinatlarına çevir
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        bool tumBulgularAktif = true;
        int yeniGorulenBulguSayisi = 0; // Yeni eklenen sayaç

        // Tüm bulgu alanlarını kontrol et
        foreach (var bulgu in bulguAlanlari)
        {
            if (bulgu.alan != null && bulgu.bulguText != null)
            {
                // Mouse ile alan arasındaki mesafeyi hesapla
                float mesafe = Vector2.Distance(mousePos, bulgu.alan.position);

                // Eğer mouse yeterince yakınsa texti göster, değilse gizle
                if (mesafe <= bulgu.aktivasyonMesafesi)
                {
                    if (!bulgu.bulguText.gameObject.activeSelf)
                    {
                        bulgu.bulguText.gameObject.SetActive(true);
                        bulgu.goruldu = true;
                        Debug.Log($"{bulgu.bulguAdi} alanına yaklaşıldı!");
                    }
                }
                else
                {
                    if (bulgu.bulguText.gameObject.activeSelf)
                    {
                        bulgu.bulguText.gameObject.SetActive(false);
                    }
                }

                // Eğer herhangi bir bulgu henüz görülmediyse
                if (!bulgu.goruldu)
                {
                    tumBulgularAktif = false;
                }
                else
                {
                    yeniGorulenBulguSayisi++; // Görülen bulgu sayısını artır
                }
            }
        }

        // Eğer görülen bulgu sayısı değiştiyse ilerleme textini güncelle
        if (yeniGorulenBulguSayisi != gorulenBulguSayisi)
        {
            gorulenBulguSayisi = yeniGorulenBulguSayisi;
            IlerlemeTextiniGuncelle();
        }

        // Tüm bulgular görüldüyse ve daha önce tamamlanmadıysa
        if (tumBulgularAktif && !tumBulgularTamamlandi)
        {
            tumBulgularTamamlandi = true;
            BulgularTamamlandi();
        }
    }

    private void IlerlemeTextiniGuncelle()
    {
        if (ilerlemeText != null)
        {
            ilerlemeText.text = $"{gorulenBulguSayisi}/{bulguAlanlari.Length}";
        }
    }

    private void BulgularTamamlandi()
    {
        Debug.Log("Tüm bulgular tamamlandı!");
        if (devamButonu != null)
        {
            devamButonu.gameObject.SetActive(true);
        }
    }

    // Gizmos ile alanları görselleştir (Unity Editor'da)
    private void OnDrawGizmos()
    {
        if (bulguAlanlari == null) return;

        foreach (var bulgu in bulguAlanlari)
        {
            if (bulgu.alan != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(bulgu.alan.position, bulgu.aktivasyonMesafesi);
            }
        }
    }
} 