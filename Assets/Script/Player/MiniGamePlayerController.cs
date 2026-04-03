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
    [SerializeField] private Transform cameraTransform;

    [Header("Visuel")]
    [SerializeField] private Transform visualRoot;

    [Header("Audio")]
    [SerializeField] private float footstepInterval = 0.4f;

    private CharacterController characterController;
    private Animator animator;
    private Vector2 moveInput;
    private float verticalVelocity;
    private float footstepTimer = 0f;

    private int _speedParam;
    private int _victoryParam;

    private void Awake()
    {
        _speedParam = Animator.StringToHash("Speed");
        _victoryParam = Animator.StringToHash("Trigger");

        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    /// <summary>Appelé automatiquement par le New Input System (action "Move").</summary>
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
        if (moveInput.sqrMagnitude >= 0.01f)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = camForward * moveInput.y + camRight * moveInput.x;

            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

            if (visualRoot != null)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                visualRoot.rotation = Quaternion.Slerp(visualRoot.rotation, targetRotation, Time.deltaTime * 10f);
            }

            // Son de pas avec cooldown
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                AudioManager.Instance?.PlayFootstepSound();
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }

        animator.SetFloat(_speedParam, characterController.velocity.magnitude);
    }

    /// <summary>Applique la gravité pour que le joueur reste au sol.</summary>
    private void ApplyGravity()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity = -1f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        characterController.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }

    /// <summary>Déclenche l'animation Victory.</summary>
    public void TriggerVictory()
    {
        animator.SetTrigger(_victoryParam);
    }
}
