using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // SceneManager için ekledik
using TMPro;

public class PanelManager : MonoBehaviour
{
    [Header("Buton Stili")]
    [SerializeField] private Color buttonNormalColor = new Color(1f, 1f, 1f, 0.8f);
    [SerializeField] private Color buttonHighlightedColor = new Color(0.9f, 0.9f, 0.9f, 0.9f);
    [SerializeField] private Color buttonPressedColor = new Color(0.8f, 0.8f, 0.8f, 1f);
    [SerializeField] private Color buttonTextColor = Color.white;
    //[SerializeField] private int buttonFontSize = 24;

    [SerializeField] private GameObject[] panels; // Bütün panelleri inspector'dan ekle
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private GameObject devamButonu; // Devam butonu referansı
    [SerializeField] private HucreHareket[] hucreler; // Tüm hücrelerin referansları

    [System.Serializable]
    public class PanelConfig
    {
        public bool isQuestionPanel; // Panel soru paneli mi?
        public bool hasDelayedTransition; // Panel geçişi 1 saniye gecikmeli mi?
        public int correctButtonIndex = -1; // Doğru cevabın buton indeksi (-1 = normal geçiş butonu)
        public int correctAnswerPoints = 10; // Doğru cevap puanı
        public int wrongAnswerPoints = 5; // Yanlış cevap ceza puanı
    }

    [SerializeField] private PanelConfig[] panelConfigs; // Her panel için ayarlar

    private int currentPanelIndex = 0; // İlk panelden başla
    private int tamamlananHucreSayisi = 0;

    public void ShowFirstPanel()
    {
        if (panels.Length > 0)
        {
            panels[0].SetActive(true);
            CanvasGroup cg = EnsureCanvasGroup(panels[0]);
            cg.alpha = 0f;
            StartCoroutine(FadeInPanel(cg));
        }
    }

    void Start()
    {
        // Devam butonunu başlangıçta devre dışı bırak
        if (devamButonu != null)
        {
            devamButonu.SetActive(false);
        }

        // Tüm hücrelere event listener ekle
        foreach (var hucre in hucreler)
        {
            hucre.onHucreTamamlandi.AddListener(HucreTamamlandi);
        }

        // Tüm panellerin canvas group'larını kontrol et
        for (int i = 0; i < panels.Length; i++)
        {
            CanvasGroup cg = EnsureCanvasGroup(panels[i]);
            cg.alpha = 1f;

            // Tüm butonları bul ve stilini uygula
            Button[] buttons = panels[i].GetComponentsInChildren<Button>(true); // true parametresi devre dışı butonları da dahil eder
            int panelIndex = i;

            foreach (Button button in buttons)
            {
                ApplyButtonStyle(button);
                int buttonIndex = System.Array.IndexOf(buttons, button);
                button.onClick.AddListener(() => OnButtonClicked(panelIndex, buttonIndex));
            }
        }
    }

    private void ApplyButtonStyle(Button button)
    {
        // Buton görünümünü ayarla
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            // Eğer buton şeffaf ise (alpha = 0), stil uygulama
            if (buttonImage.color.a == 0)
            {
                return;
            }

            // Buton arka planını ayarla
            buttonImage.color = buttonNormalColor;
            
            // Buton renklerini ayarla
            ColorBlock colors = new ColorBlock();
            colors.normalColor = buttonNormalColor;
            colors.highlightedColor = buttonHighlightedColor;
            colors.pressedColor = buttonPressedColor;
            colors.selectedColor = buttonHighlightedColor;
            colors.disabledColor = new Color(0.8f, 0.8f, 0.8f, 0.5f);
            colors.colorMultiplier = 1f;
            colors.fadeDuration = 0.1f;
            button.colors = colors;
        }

        // Buton metnini ayarla (TextMeshPro için)
        TextMeshProUGUI[] texts = button.GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (TextMeshProUGUI text in texts)
        {
            text.color = buttonTextColor;
            text.fontStyle = FontStyles.Bold;
        }
    }

    private void OnButtonClicked(int panelIndex, int buttonIndex)
    {
        if (panelIndex >= panelConfigs.Length) return;

        PanelConfig config = panelConfigs[panelIndex];

        if (config.isQuestionPanel)
        {
            // Tüm butonları al
            Button[] buttons = panels[panelIndex].GetComponentsInChildren<Button>();
            
            // Tüm butonları devre dışı bırak
            foreach (Button button in buttons)
            {
                button.interactable = false;
            }
            
            // Soru paneli ise
            if (buttonIndex == config.correctButtonIndex)
            {
                // Doğru cevap
                GameManager.Instance.IncreaseScore(config.correctAnswerPoints);
                // Doğru cevabı yeşil yap
                buttons[buttonIndex].GetComponent<Image>().color = Color.green;
                StartCoroutine(DelayAndMoveToNextPanel(panelIndex));
            }
            else
            {
                // Yanlış cevap
                GameManager.Instance.DecreaseScore(config.wrongAnswerPoints);
                // Yanlış cevabı kırmızı yap
                Image buttonImage = buttons[buttonIndex].GetComponent<Image>();
                Color originalColor = buttonImage.color;
                buttonImage.color = Color.red;
                StartCoroutine(ResetButtonColor(buttonImage, originalColor, buttons));
            }
        }
        else
        {
            // Normal geçiş paneli ise
            MoveToNextPanel(panelIndex);
        }
    }

    private IEnumerator ResetButtonColor(Image buttonImage, Color originalColor, Button[] buttons)
    {
        yield return new WaitForSeconds(1f);
        buttonImage.color = originalColor;
        
        // Butonları tekrar aktif et
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }

    private IEnumerator DelayAndMoveToNextPanel(int panelIndex)
    {
        yield return new WaitForSeconds(1f); // 1 saniye bekle
        MoveToNextPanel(panelIndex);
    }

    public void MoveToNextPanel(int currentPanelIndex)
    {
        if (currentPanelIndex >= panelConfigs.Length || !panelConfigs[currentPanelIndex].hasDelayedTransition)
        {
            // Gecikmesiz geçiş
            panels[currentPanelIndex].SetActive(false);
            
            if (currentPanelIndex + 1 < panels.Length)
            {
                panels[currentPanelIndex + 1].SetActive(true);
                CanvasGroup nextCg = EnsureCanvasGroup(panels[currentPanelIndex + 1]);
                nextCg.alpha = 1f;
            }
        }
        else
        {
            // Gecikmeli geçiş
            StartCoroutine(DelayedPanelTransition(currentPanelIndex));
        }
    }

    private IEnumerator DelayedPanelTransition(int currentPanelIndex)
    {
        yield return new WaitForSeconds(2f);
        
        panels[currentPanelIndex].SetActive(false);
        
        if (currentPanelIndex + 1 < panels.Length)
        {
            panels[currentPanelIndex + 1].SetActive(true);
            CanvasGroup nextCg = EnsureCanvasGroup(panels[currentPanelIndex + 1]);
            nextCg.alpha = 1f;
        }
    }

    private IEnumerator SwitchPanel(int currentIndex)
    {
        CanvasGroup currentCanvasGroup = EnsureCanvasGroup(panels[currentIndex]);
        yield return StartCoroutine(FadeOut(currentCanvasGroup));

        panels[currentIndex].SetActive(false);

        int nextIndex = currentIndex + 1;

        if (nextIndex < panels.Length)
        {
            panels[nextIndex].SetActive(true);
            CanvasGroup nextCanvasGroup = EnsureCanvasGroup(panels[nextIndex]);
            yield return StartCoroutine(FadeIn(nextCanvasGroup));
        }
        else
        {
            Debug.Log("Son paneldeyiz, ilerleyecek panel yok.");
        }
    }

    private IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }

    private IEnumerator FadeInPanel(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        float fadeDuration = 1f; // Fade süresi (saniye olarak)

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    private CanvasGroup EnsureCanvasGroup(GameObject panel)
    {
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = panel.AddComponent<CanvasGroup>();
        }
        return cg;
    }

    private IEnumerator PanelGecisYap(GameObject aktifPanel, GameObject hedefPanel)
    {
        if (aktifPanel != null)
            aktifPanel.SetActive(false);
            
        // 1 saniye bekle
        yield return new WaitForSeconds(1f);
        
        if (hedefPanel != null)
            hedefPanel.SetActive(true);
    }

    public void PanelDegistir(GameObject hedefPanel)
    {
        if (panels[currentPanelIndex] != hedefPanel)
        {
            StartCoroutine(PanelGecisYap(panels[currentPanelIndex], hedefPanel));
            currentPanelIndex = System.Array.IndexOf(panels, hedefPanel);
        }
    }

    private void HucreTamamlandi()
    {
        tamamlananHucreSayisi++;
        if (tamamlananHucreSayisi >= hucreler.Length)
        {
            // Tüm hücreler tamamlandıysa butonu aktif et
            if (devamButonu != null)
            {
                devamButonu.SetActive(true);
            }
        }
    }
}
