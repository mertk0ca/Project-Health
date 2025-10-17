using UnityEngine;
using System.Collections;

public class MakeObjectObvious : MonoBehaviour
{
    [Header("Boyut Ayarlar�")]
    [SerializeField] private float buyumeOrani = 1.2f;     // Ne kadar b�y�yece�i
    [SerializeField] private float animasyonSuresi = 0.5f; // B�y�me/k���lme s�resi
    [SerializeField] private float beklemeZamani = 1f;     // �ki animasyon aras� bekleme s�resi

    private Vector3 baslangicBoyutu;
    private bool animasyonAktif = true;

    private void Start()
    {
        baslangicBoyutu = transform.localScale;
        StartCoroutine(BoyutAnimasyonu());
    }

    private IEnumerator BoyutAnimasyonu()
    {
        while (animasyonAktif)
        {
            // B�y�me animasyonu
            float gecenZaman = 0f;
            while (gecenZaman < animasyonSuresi)
            {
                gecenZaman += Time.deltaTime;
                float oran = gecenZaman / animasyonSuresi;
                transform.localScale = Vector3.Lerp(baslangicBoyutu, baslangicBoyutu * buyumeOrani, oran);
                yield return null;
            }

            // K���lme animasyonu
            gecenZaman = 0f;
            while (gecenZaman < animasyonSuresi)
            {
                gecenZaman += Time.deltaTime;
                float oran = gecenZaman / animasyonSuresi;
                transform.localScale = Vector3.Lerp(baslangicBoyutu * buyumeOrani, baslangicBoyutu, oran);
                yield return null;
            }

            // Bekleme s�resi
            yield return new WaitForSeconds(beklemeZamani);
        }
    }

    // Animasyonu d��ar�dan durdurmak i�in
    public void AnimasyonuDurdur()
    {
        animasyonAktif = false;
    }

    // Animasyonu d��ar�dan ba�latmak i�in
    public void AnimasyonuBaslat()
    {
        if (!animasyonAktif)
        {
            animasyonAktif = true;
            StartCoroutine(BoyutAnimasyonu());
        }
    }
}