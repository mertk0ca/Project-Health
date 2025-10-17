using UnityEngine;
using UnityEngine.UI;

public class KagitRenkDegistirme : MonoBehaviour
{
    [Header("Renk Değiştirme Ayarları")]
    [SerializeField] private float renkDegisimHizi = 2f;
    [SerializeField] private float yayilmaHizi = 2f;
    [SerializeField] private float maxYayilmaAlani = 2f;

    [Header("UI ve Panel Ayarları")]
    [SerializeField] private Button devamButonu;
    [SerializeField] private GameObject mevcutPanel; // 15. panel
    [SerializeField] private PanelManager panelManager;
    [SerializeField] private float butonGecikme = 3f; // Butonun görünme gecikmesi

    private SpriteRenderer spriteRenderer;
    private Color hedefRenk;
    private Color baslangicRenk;
    private float mevcutYayilma = 0f;
    private Vector3 sonTemasNoktasi;
    private bool renkDegisiyorMu = false;
    private bool renkDegisimTamamlandi = false;
    private float renkDegisimEsigi = 0.5f; // Renk değişiminin tamamlandığını kabul etmek için eşik değer

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            baslangicRenk = spriteRenderer.color;
            hedefRenk = Color.red;
            Debug.Log($"Başlangıç rengi: {baslangicRenk}, Hedef renk: {hedefRenk}");
        }

        // Başlangıçta butonu gizle
        if (devamButonu != null)
        {
            devamButonu.gameObject.SetActive(false);
            Debug.Log("Devam butonu başlangıçta gizlendi");
        }
        else
        {
            Debug.LogError("Devam butonu referansı eksik!");
        }
    }

    private void Update()
    {
        if (renkDegisiyorMu && !renkDegisimTamamlandi)
        {
            mevcutYayilma += yayilmaHizi * Time.deltaTime;
            RenkGuncelle();
        }
    }

    public void DamlaTemasEtti(Vector3 temasNoktasi)
    {
        Debug.Log("Damla temas etti: " + temasNoktasi);
        sonTemasNoktasi = temasNoktasi;
        renkDegisiyorMu = true;
    }

    private void RenkGuncelle()
    {
        if (spriteRenderer != null)
        {
            float lerpDegeri = Mathf.Lerp(spriteRenderer.color.r, 1f, renkDegisimHizi * Time.deltaTime);
            Color yeniRenk = new Color(lerpDegeri, spriteRenderer.color.g * 0.5f, spriteRenderer.color.b * 0.5f);
            spriteRenderer.color = yeniRenk;

            // Mevcut kırmızı değerini kontrol et
            float mevcutKirmiziDeger = spriteRenderer.color.r;
            Debug.Log($"Mevcut kırmızı değer: {mevcutKirmiziDeger}");

            // Renk değişimi tamamlandı mı kontrol et
            if (!renkDegisimTamamlandi && mevcutKirmiziDeger >= renkDegisimEsigi)
            {
                Debug.Log("Renk değişimi tamamlandı! Buton gösterilecek.");
                renkDegisimTamamlandi = true;
                Invoke("DevamButonunuGoster", butonGecikme);
            }
        }
    }

    private void DevamButonunuGoster()
    {
        Debug.Log("DevamButonunuGoster çağrıldı");
        if (devamButonu != null)
        {
            devamButonu.gameObject.SetActive(true);
            devamButonu.onClick.RemoveAllListeners(); // Önceki listener'ları temizle
            devamButonu.onClick.AddListener(SonrakiPaneleGec);
            Debug.Log("Devam butonu aktif edildi ve listener eklendi");
        }
        else
        {
            Debug.LogError("Devam butonu hala null!");
        }
    }

    private void SonrakiPaneleGec()
    {
        Debug.Log("SonrakiPaneleGec çağrıldı");
        if (panelManager != null)
        {
            if (mevcutPanel != null)
            {
                mevcutPanel.SetActive(false);
                Debug.Log("Mevcut panel deaktif edildi");
            }
            panelManager.MoveToNextPanel(18); // 15. panelden 16. panele geçiş
            Debug.Log("Sonraki panele geçiş yapıldı");
        }
        else
        {
            Debug.LogError("Panel Manager referansı eksik!");
        }
    }
} 