using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowingSkill : MonoBehaviour
{
    [Header("Throw Settings")]
    [SerializeField] private GameObject proyektilPrefab;
    [SerializeField] private Transform spawnpoint;
    [SerializeField] private float _speed = 10f;

    [Header("SFX Settings")]
    [SerializeField] private AudioClip throwSFX; // Suara saat melempar
    [SerializeField] private AudioClip hitSFX;   // Suara saat terkena musuh
    private AudioSource audioSource;

    private PlayerInput playerInput;
    private InputAction skillAction;
    private Animator animator;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (playerInput != null)
        {
            skillAction = playerInput.actions["Throw"];
        }

        // Tambahkan AudioSource jika belum ada
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (skillAction != null && skillAction.triggered)
        {
            StartThrow();
        }
    }

    private void StartThrow()
    {
        // Aktifkan trigger animasi "Throw"
        if (animator != null)
        {
            animator.SetTrigger("Throw");
        }

        // Mainkan SFX lempar
        if (throwSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(throwSFX);
        }

        GameObject knifeInstance = Instantiate(proyektilPrefab, spawnpoint.position, spawnpoint.rotation);
        ProyektilLogic logic = knifeInstance.AddComponent<ProyektilLogic>();
        logic.Initialize(_speed, hitSFX, audioSource);
    }

    //Logika Proyektil
    private class ProyektilLogic : MonoBehaviour
    {
        private float speed;
        private Vector3 forwardDirection;
        private AudioClip hitSound;
        private AudioSource source;

        // Constructor untuk menerima SFX
        public void Initialize(float speed, AudioClip hitSFX, AudioSource audioSource)
        {
            this.speed = speed;
            this.hitSound = hitSFX;
            this.source = audioSource;

            forwardDirection = transform.forward;
            Destroy(gameObject, 3f);
        }

        private void Update()
        {
            transform.position += forwardDirection * speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                if (hitSound != null && source != null)
                {
                    source.PlayOneShot(hitSound);
                }
                Destroy(gameObject);
            }
        }
    }
}