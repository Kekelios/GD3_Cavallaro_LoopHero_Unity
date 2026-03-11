using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Déplace le joueur en vue Third Person dans le mini-jeu.
/// Nécessite : CharacterController, et le tag "Player" sur le GameObject.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class MiniGamePlayerController : MonoBehaviour
{
    [Header("Déplacement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Caméra")]
    [SerializeField] private Transform cameraTransform; // Glisse la Main Camera ici

    private CharacterController characterController;
    private Vector2 moveInput;
    private float verticalVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    /// <summary>
    /// Appelé automatiquement par le New Input System (action "Move").
    /// </summary>
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void Update()
    {
        Move();
        ApplyGravity();
    }

    /// <summary>Calcule et applique le déplacement relatif à la caméra.</summary>
    private void Move()
    {
        if (moveInput.sqrMagnitude < 0.01f) return;

        // Direction relative à l'orientation de la caméra (ignore l'axe Y de la caméra)
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = (camForward * moveInput.y + camRight * moveInput.x);

        // Le joueur se tourne dans la direction du mouvement
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * 10f);

        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    /// <summary>Applique la gravité pour que le joueur reste au sol.</summary>
    private void ApplyGravity()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity = -1f; // Petite valeur pour rester collé au sol
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        characterController.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }
}
