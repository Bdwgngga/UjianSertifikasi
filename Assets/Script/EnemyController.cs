using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Effects")]
    [SerializeField] private ParticleSystem deathVFX; // Efek VFX saat mati
    [SerializeField] private AudioClip deathSFX; // Efek SFX saat mati
    private AudioSource audioSource;

    [Header("UI Health Bar")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Canvas healthCanvas;

    [Header("Score Settings")]
    [SerializeField] private int scoreValue = 10;

    private void Awake()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    private void Update()
    {
        MoveForward();
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    public void ApplyDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Enemy terkena damage: {damage}, sisa HP: {currentHealth}");

        // Update UI Slider HP
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy mati!");
        Destroy(gameObject, 0.1f);
        GameUI.Instance.AddScore(scoreValue);

        if (deathVFX != null)
        {
            ParticleSystem vfxInstance = Instantiate(deathVFX, transform.position, Quaternion.identity);
            Destroy(vfxInstance.gameObject, vfxInstance.main.duration);
        }


        if (deathSFX != null)
        {
            audioSource.PlayOneShot(deathSFX);
        }


        if (healthCanvas != null)
        {
            Destroy(healthCanvas.gameObject);
        }
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            ApplyDamage(25);
            Destroy(other.gameObject);
        }
    }

}
