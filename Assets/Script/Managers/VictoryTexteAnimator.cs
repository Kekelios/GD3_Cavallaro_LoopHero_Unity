using UnityEngine;
using TMPro;

public class VictoryTextAnimator : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 1f;
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float targetScale = 1.2f;
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float pulseAmount = 0.1f;

    private TextMeshProUGUI textComponent;
    private Vector3 originalScale;
    private float animationTimer = 0f;
    private bool isAnimating = true;

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();

        if (textComponent == null)
        {
            Debug.LogError("VictoryTextAnimator: Aucun TextMeshProUGUI trouvé !");
            enabled = false;
            return;
        }

        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        if (textComponent == null) return;

        animationTimer = 0f;
        isAnimating = true;
        transform.localScale = Vector3.zero;

        Color color = textComponent.color;
        color.a = 0f;
        textComponent.color = color;
    }

    private void Update()
    {
        if (textComponent == null || !textComponent.isActiveAndEnabled)
        {
            return;
        }

        if (isAnimating)
        {
            animationTimer += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(animationTimer / animationDuration);
            float curveValue = scaleCurve.Evaluate(progress);

            transform.localScale = originalScale * curveValue * targetScale;

            Color color = textComponent.color;
            color.a = progress;
            textComponent.color = color;

            if (progress >= 1f)
            {
                isAnimating = false;
            }
        }
        else
        {
            float pulse = 1f + Mathf.Sin(Time.unscaledTime * pulseSpeed) * pulseAmount;
            transform.localScale = originalScale * targetScale * pulse;
        }
    }
}
