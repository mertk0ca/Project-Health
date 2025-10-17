using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button oynaButonu;
    [SerializeField] private Button cikisButonu;

    void Start()
    {
        if (oynaButonu != null)
        {
            oynaButonu.onClick.AddListener(OyunuBaslat);
        }

        if (cikisButonu != null)
        {
            cikisButonu.onClick.AddListener(OyundanCik);
        }
    }

    public void OyunuBaslat()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void OyundanCik()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
} 