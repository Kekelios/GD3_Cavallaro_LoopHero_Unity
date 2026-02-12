using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform pawnTarget;
    [SerializeField] private Vector3 targetOffset = new Vector3(0, 1f, 0);

    [Header("Follow Settings")]
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private bool smoothFollow = true;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float minVerticalAngle = 10f;
    [SerializeField] private float maxVerticalAngle = 80f;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 20f;
    [SerializeField] private float defaultZoom = 10f;

    private float currentRotationY = 0f;
    private float currentRotationX = 38.25f;
    private float currentZoom;
    private bool canControl = true;
    private Vector3 currentTargetPosition;

    private Mouse mouse;

    private void Start()
    {
        mouse = Mouse.current;

        if (pawnTarget == null)
        {
            GameObject pawn = GameObject.Find("=== Player ===");
            if (pawn != null)
            {
                pawnTarget = pawn.transform;
            }
        }

        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStarted += DisableControls;
            DialogueManager.Instance.OnDialogueEnded += EnableControls;
        }

        currentZoom = defaultZoom;

        if (pawnTarget != null)
        {
            currentTargetPosition = pawnTarget.position;
        }
    }

    private void OnDestroy()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStarted -= DisableControls;
            DialogueManager.Instance.OnDialogueEnded -= EnableControls;
        }
    }

    private void LateUpdate()
    {
        if (pawnTarget == null)
            return;

        UpdateTargetPosition();

        if (canControl && mouse != null)
        {
            HandleRotation();
            HandleZoom();
        }

        UpdateCameraPosition();
    }

    private void UpdateTargetPosition()
    {
        Vector3 desiredPosition = pawnTarget.position + targetOffset;

        if (smoothFollow)
        {
            currentTargetPosition = Vector3.Lerp(currentTargetPosition, desiredPosition, followSpeed * Time.deltaTime);
        }
        else
        {
            currentTargetPosition = desiredPosition;
        }
    }

    private void HandleRotation()
    {
        if (mouse.rightButton.isPressed)
        {
            Vector2 mouseDelta = mouse.delta.ReadValue();

            currentRotationY += mouseDelta.x * rotationSpeed * Time.deltaTime;
            currentRotationX -= mouseDelta.y * rotationSpeed * Time.deltaTime;

            currentRotationX = Mathf.Clamp(currentRotationX, minVerticalAngle, maxVerticalAngle);
        }
    }

    private void HandleZoom()
    {
        float scrollValue = mouse.scroll.ReadValue().y;

        if (scrollValue != 0)
        {
            currentZoom -= scrollValue * zoomSpeed * 0.01f;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        }
    }

    private void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0);
        Vector3 direction = rotation * Vector3.back;
        Vector3 desiredPosition = currentTargetPosition + direction * currentZoom;

        transform.position = desiredPosition;
        transform.LookAt(currentTargetPosition);
    }

    private void DisableControls()
    {
        canControl = false;
    }

    private void EnableControls()
    {
        canControl = true;
    }

    public void ResetCamera()
    {
        currentRotationY = 0f;
        currentRotationX = 38.25f;
        currentZoom = defaultZoom;
    }
}
