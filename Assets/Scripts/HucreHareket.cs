using UnityEngine;
using UnityEngine.Events;

public class HucreHareket : MonoBehaviour
{
    [Header("Hareket Ayarlar")]
    [SerializeField] private float hareketHizi = 2f;
    [SerializeField] private float minHiz = 1f;
    [SerializeField] private float maxHiz = 3f;

    [Header("Dnme Ayarlar")]
    [SerializeField] private float donmeHizi = 5f;
    [SerializeField] private float minDonmeHizi = 2.5f;
    [SerializeField] private float maxDonmeHizi = 7.5f;
    private float mevcutDonmeHizi;

    [Header("Patlama Efekti")]
    [SerializeField] private GameObject patlamaEfektiPrefab;

    public UnityEvent onHucreTamamlandi;

    private bool tamamlandi = false;
    private Vector3 hareketYonu;
    private Camera mainCamera;
    private float ekranSiniriX;
    private float ekranSiniriY;

    private void Start()
    {
        // Rastgele başlangı yönü belirle
        hareketYonu = Random.insideUnitCircle.normalized;
        hareketHizi = Random.Range(minHiz, maxHiz);

        // Rastgele dönme yönü ve hızı belirle
        mevcutDonmeHizi = Random.Range(minDonmeHizi, maxDonmeHizi);
        if (Random.value > 0.5f)
        {
            mevcutDonmeHizi *= -1; // %50 ihtimalle ters yne dön
        }

        // Ana kamerayı al
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            float kameraYukseklik = 2f * mainCamera.orthographicSize;
            float kameraGenislik = kameraYukseklik * mainCamera.aspect;

            ekranSiniriX = kameraGenislik / 2f - 0.5f;
            ekranSiniriY = kameraYukseklik / 2f - 0.5f;
        }
    }

    private void Update()
    {
        // Hareket
        Vector3 yeniPozisyon = transform.position;
        yeniPozisyon += hareketYonu * hareketHizi * Time.deltaTime;

        // Ekran sınırlarını kontrol et
        if (Mathf.Abs(yeniPozisyon.x) > ekranSiniriX)
        {
            hareketYonu.x = -hareketYonu.x;
            yeniPozisyon.x = Mathf.Sign(yeniPozisyon.x) * ekranSiniriX;
        }

        if (Mathf.Abs(yeniPozisyon.y) > ekranSiniriY)
        {
            hareketYonu.y = -hareketYonu.y;
            yeniPozisyon.y = Mathf.Sign(yeniPozisyon.y) * ekranSiniriY;
        }

        // Yeni pozisyonu uygula
        transform.position = yeniPozisyon;

        // Dönme hareketi
        transform.Rotate(Vector3.forward * mevcutDonmeHizi * Time.deltaTime);
    }

    private void OnMouseDown()
    {
        if (patlamaEfektiPrefab != null && !tamamlandi)
        {
            Instantiate(patlamaEfektiPrefab, transform.position, Quaternion.identity);
            tamamlandi = true;
            onHucreTamamlandi?.Invoke();
            gameObject.SetActive(false);
        }
    }
}