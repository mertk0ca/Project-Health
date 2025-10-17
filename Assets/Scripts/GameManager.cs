using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro için gerekli
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Text scoreText; // UI'daki Text bileşeni
    public Text scoreChangeText;
    public int score = 0;
    private int finalScore;
    public Text finalScoreText;

    public TextMeshProUGUI finalMessage; // "Başardın!" veya "Başaramadın!"
    public TextMeshProUGUI diagnosisMessage; // Tanı mesajı

    [Header("Panel Referansları")]
    [SerializeField] private GameObject anaMenuPanel;
    [SerializeField] private GameObject oyunPanel;

    [Header("Hasta Referansı")]
    [SerializeField] private PatientMove hastaHareket;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Başlangıçta ana menüyü göster, oyun panelini gizle
        if (anaMenuPanel != null)
        {
            anaMenuPanel.SetActive(true);
        }
        if (oyunPanel != null)
        {
            oyunPanel.SetActive(false);
        }

        // Hasta hareketini başlangıçta durdur
        if (hastaHareket != null)
        {
            hastaHareket.enabled = false;
        }

        if (scoreChangeText != null)
        {
            scoreChangeText.gameObject.SetActive(false);
        }

        if (finalMessage != null)
        {
            finalMessage.text = ""; // Başlangıçta boş bırak
        }

        if (diagnosisMessage != null)
        {
            diagnosisMessage.text = ""; // Başlangıçta boş bırak
        }

        // Score text'i başlangıçta aktif et
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(true);
        }
    }

    // Ana menüye geçiş yapıldığında çağrılacak fonksiyon
    public void AnaMenuyeGec()
    {
        // Ana menüye geçişte score text'i deaktif etme
    }

    // Oyun başladığında çağrılacak fonksiyon
    public void OyunuBaslat()
    {
        // Oyun başladığında score text'i aktif et
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(true);
        }
    }

    public void IncreaseScore(int amount)
    {
        if(amount != 0) 
        {
            score += amount;
            UpdateScoreText();
            ShowScoreChange(amount);
        }
    }

    public void DecreaseScore(int amount)
    {
        if (amount != 0)
        {
            score -= amount;
            UpdateScoreText();
            ShowScoreChange(-amount);
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            if(score > 100)
            {
                scoreText.color = Color.yellow;
            }

            if(score < 0) 
            {
                score = 0;
            }

            finalScore = score;

            if(finalScore > 100) 
            {
                finalScore = 100;
            }
            scoreText.text = "Not: " + score.ToString();
            finalScoreText.text = "Not: " + finalScore.ToString();

            // Skor 60'a göre anlık olarak mesajları güncelle
            if (finalMessage != null && diagnosisMessage != null)
            {
                if (score >= 60)
                {
                    finalMessage.text = "Başardın";
                    finalMessage.color = Color.green;

                    diagnosisMessage.text = "Hastaya başarılı bir şekilde tanı koydun.";
                    diagnosisMessage.color = Color.green;
                }
                else
                {
                    finalMessage.text = "Başaramadın";
                    finalMessage.color = Color.red;

                    diagnosisMessage.text = "Hastaya başarılı bir şekilde tanı koyamadın.";
                    diagnosisMessage.color = Color.red;
                }
            }
        }
    }

    private void ShowScoreChange(int amount)
    {
        if (scoreChangeText != null)
        {
            scoreChangeText.color = amount > 0 ? Color.green : Color.red;
            scoreChangeText.text = (amount > 0 ? "+" : "") + amount.ToString();
            scoreChangeText.gameObject.SetActive(true);
            StartCoroutine(HideScoreChangeText());
        }
    }

    private IEnumerator HideScoreChangeText()
    {
        yield return new WaitForSeconds(1f); // 1 saniye göster
        if (scoreChangeText != null)
        {
            scoreChangeText.gameObject.SetActive(false);
        }
    }
}
