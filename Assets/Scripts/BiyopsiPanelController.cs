using UnityEngine;
using UnityEngine.UI;

public class BiyopsiPanelController : MonoBehaviour
{
    [Header("Panel Referansları")]
    [SerializeField] private GameObject biyopsiPanel; // Biyopsi sonucu paneli
    [SerializeField] private Button biyopsiGorButonu; // "Biyopsi Sonucunu Gör" butonu
    [SerializeField] private Button kapatButonu; // Panel içindeki "Kapat" butonu

    private void Start()
    {
        // Başlangıçta paneli deaktif et
        if (biyopsiPanel != null)
        {
            biyopsiPanel.SetActive(false);
        }

        // Butonlara listener'ları ekle
        if (biyopsiGorButonu != null)
        {
            biyopsiGorButonu.onClick.AddListener(BiyopsiPaneliniAc);
        }

        if (kapatButonu != null)
        {
            kapatButonu.onClick.AddListener(BiyopsiPaneliniKapat);
        }
    }

    private void BiyopsiPaneliniAc()
    {
        if (biyopsiPanel != null)
        {
            biyopsiPanel.SetActive(true);
        }
    }

    private void BiyopsiPaneliniKapat()
    {
        if (biyopsiPanel != null)
        {
            biyopsiPanel.SetActive(false);
        }
    }
} 