using UnityEngine;

public class Damla : MonoBehaviour
{
    [Header("Damla Ayarları")]
    [SerializeField] private float dusmeHizi = 2f;
    [SerializeField] private float yokOlmaSuresi = 5f;

    private void Start()
    {
        // Belirli süre sonra damlayı yok et
        Destroy(gameObject, yokOlmaSuresi);
    }

    private void Update()
    {
        // Damlayı aşağı doğru hareket ettir
        transform.Translate(Vector3.down * dusmeHizi * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Eğer kağıda temas ederse
        if (other.CompareTag("Kagit"))
        {
            // Kağıdın renk değiştirme scriptini bul ve tetikle
            KagitRenkDegistirme kagit = other.GetComponent<KagitRenkDegistirme>();
            if (kagit != null)
            {
                kagit.DamlaTemasEtti(transform.position);
            }

            // Damlayı yok et
            Destroy(gameObject);
        }
    }
} 