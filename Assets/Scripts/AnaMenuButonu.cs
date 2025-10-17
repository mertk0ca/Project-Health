using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AnaMenuButonu : MonoBehaviour
{
    void Start()
    {
        Debug.Log("AnaMenuButonu başlatıldı");
        Button buton = GetComponent<Button>();
        if (buton != null)
        {
            Debug.Log("Buton bulundu, listener eklendi");
            buton.onClick.AddListener(AnaMenuyeDon);
        }
        else
        {
            Debug.LogError("Buton bulunamadı!");
        }
    }

    public void AnaMenuyeDon()
    {
        Debug.Log("AnaMenuyeDon fonksiyonu çağrıldı");
        SceneManager.LoadScene("MainMenu");
    }
}