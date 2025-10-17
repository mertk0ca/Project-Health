using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class BloodCellGame : MonoBehaviour
{
    [Header("Hücre Prefableri")]
    [SerializeField] private GameObject kirmiziHucrePrefab;
    [SerializeField] private GameObject beyazHucrePrefab;

    [Header("Spawn Ayarlar")]
    [SerializeField] private Vector2 spawnAlani = new Vector2(8f, 4f);
    [SerializeField] private float minSpawnYuksekligi = 1f;

    [Header("UI Elemanlar")]
    [SerializeField] private TextMeshProUGUI kirmiziSayacText;
    [SerializeField] private TextMeshProUGUI beyazSayacText;
    [SerializeField] private TextMeshProUGUI tamamlandiText;
    [SerializeField] private Button devamButonu;
    [SerializeField] private PanelManager panelManager;
    [SerializeField] private GameObject currentPanel; // Mevcut panel referansı

    [Header("Tamamlanma Ayarlar")]
    [SerializeField] private GameObject tamamlandiObje; // Tüm hücreler tamamlandığında aktif edilecek obje

    // Sayaçlar
    private int toplamKirmiziHucre;
    private int toplamBeyazHucre;
    private int bulunanKirmiziHucre;
    private int bulunanBeyazHucre;

    private void Start()
    {
        // Devam butonuna listener ekle
        if (devamButonu != null)
        {
            devamButonu.onClick.AddListener(DevamButonunaTiklandi);
        }
        
        OyunuBaslat();
    }

    private void DevamButonunaTiklandi()
    {
        Debug.Log("Devam butonuna tıklandı!");
        if (panelManager != null)
        {
            // Mevcut paneli deaktif et
            if (currentPanel != null)
            {
                currentPanel.SetActive(false);
            }
            
            // 13. panelden 14. panele geçiş yap
            panelManager.MoveToNextPanel(16);
        }
        else
        {
            Debug.LogError("Panel Manager referansı eksik!");
        }
    }

    public void OyunuBaslat()
    {
        // Varolan hücreleri temizle
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Sayaçları sıfırla
        bulunanKirmiziHucre = 0;
        bulunanBeyazHucre = 0;

        // Devam butonunu başlangıçta devre dışı bırak
        if (devamButonu != null)
        {
            devamButonu.gameObject.SetActive(false);
            devamButonu.interactable = false;
        }

        // Rastgele sayıda hücre oluştur
        toplamKirmiziHucre = Random.Range(9, 12); // 15-25 arası
        toplamBeyazHucre = Random.Range(17, 20);    // 34-40 arası

        // Hücreleri spawn et
        for (int i = 0; i < toplamKirmiziHucre; i++)
        {
            HucreOlustur(true);
        }
        for (int i = 0; i < toplamBeyazHucre; i++)
        {
            HucreOlustur(false);
        }

        // UI'yi güncelle
        SayaclariGuncelle();
        if (tamamlandiText != null)
            tamamlandiText.gameObject.SetActive(false);
    }

    private void HucreOlustur(bool kirmiziMi)
    {
        // Rastgele pozisyon belirle
        float randomX = Random.Range(-spawnAlani.x, spawnAlani.x);
        float randomY = Random.Range(minSpawnYuksekligi, spawnAlani.y);
        Vector3 spawnPozisyon = new Vector3(randomX, randomY, 0);

        // Hücreyi oluştur
        GameObject yeniHucre = Instantiate(
            kirmiziMi ? kirmiziHucrePrefab : beyazHucrePrefab,
            spawnPozisyon,
            Quaternion.identity,
            transform
        );

        // Click eventi ekle
        HucreClick hucreClick = yeniHucre.AddComponent<HucreClick>();
        hucreClick.Initialize(kirmiziMi, HucreyeTiklandi);
    }

    private void HucreyeTiklandi(bool kirmiziMi)
    {
        if (kirmiziMi)
            bulunanKirmiziHucre++;
        else
            bulunanBeyazHucre += 2;

        Debug.Log($"Hücre tıklandı - Kırmızı: {kirmiziMi}, Yeni sayılar - Kırmızı: {bulunanKirmiziHucre}, Beyaz: {bulunanBeyazHucre}");

        SayaclariGuncelle();
        OyunDurumunuKontrolEt();
    }

    private void SayaclariGuncelle()
    {
        if (kirmiziSayacText != null)
            kirmiziSayacText.text = $"Kan Hücreleri: {bulunanKirmiziHucre} g/dL";

        if (beyazSayacText != null)
            beyazSayacText.text = $"CRP: {bulunanBeyazHucre} mg/L";
    }

    private void OyunDurumunuKontrolEt()
    {
        Debug.Log($"Kırmızı: {bulunanKirmiziHucre}/{toplamKirmiziHucre}, Beyaz: {bulunanBeyazHucre}/{toplamBeyazHucre*2}");
        
        if (bulunanKirmiziHucre >= toplamKirmiziHucre &&
            bulunanBeyazHucre >= toplamBeyazHucre*2)
        {
            Debug.Log("Tüm hücreler tamamlandı!");
            
            if (tamamlandiText != null)
            {
                tamamlandiText.gameObject.SetActive(true);
                tamamlandiText.text = "Tüm Hücreler Bulundu!";
            }

            if (devamButonu != null)
            {
                Debug.Log("Devam butonu aktif ediliyor");
                devamButonu.gameObject.SetActive(true);
                devamButonu.interactable = true;
            }
            else
            {
                Debug.LogError("Devam butonu referansı null!");
            }
        }
    }
}

// Hücrelere tıklama işlemi için yardımcı sınıf
public class HucreClick : MonoBehaviour
{
    private bool kirmiziMi;
    private System.Action<bool> tiklamaCallback;

    public void Initialize(bool kirmiziMi, System.Action<bool> callback)
    {
        this.kirmiziMi = kirmiziMi;
        this.tiklamaCallback = callback;
    }

    private void OnMouseDown()
    {
        tiklamaCallback?.Invoke(kirmiziMi);
        Destroy(gameObject);
    }
}