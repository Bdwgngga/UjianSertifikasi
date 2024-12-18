using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement Setting")]
    public float moveSpeed = 5f;
    private Animator animator;
    private Vector3 movement;

    private PlayerInput playerInput;
    private InputAction moveAction;

    void Awake()
    {
        // Ambil komponen PlayerInput
        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            moveAction = playerInput.actions["Movement"];
        }
        else
        {
            //Debug jika playercomponent hilang
            Debug.LogError("PlayerInput component not found!");
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (moveAction != null)
        {
            Vector3 inputVector = moveAction.ReadValue<Vector3>();
            movement = new Vector3(inputVector.x, 0f, 0f);

            // Cek animator sebelum menggunakannya
            if (animator != null)
            {
                animator.SetBool("isMoving", inputVector.x != 0);
            }

            // Mengatur arah pemain berdasarkan input
            if (inputVector.x > 0)
            {
                transform.localScale = new Vector3(1f, 1f, 1f); // Menghadap kanan
            }
            else if (inputVector.x < 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f); // Menghadap kiri
            }
        }
    }

    void FixedUpdate()
    {
        // Gerakan pemain berdasarkan input
        transform.position += movement * moveSpeed * Time.deltaTime;
    }
}
