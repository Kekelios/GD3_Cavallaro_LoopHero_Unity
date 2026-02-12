using UnityEngine;
using UnityEngine.UI;

public class PanelFadeIn : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.5f;

    private Image panelImage;
    private float fadeTimer = 0f;
    private Color targetColor;

    private void Awake()
    {
        panelImage = GetComponent<Image>();
        if (panelImage != null)
        {
            targetColor = panelImage.color;
        }
    }

    private void OnEnable()
    {
        fadeTimer = 0f;
        if (panelImage != null)
        {
            Color color = targetColor;
            color.a = 0f;
            panelImage.color = color;
        }
    }

    private void Update()
    {
        if (panelImage != null && fadeTimer < fadeDuration)
        {
            fadeTimer += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(fadeTimer / fadeDuration);

            Color color = targetColor;
            color.a = targetColor.a * progress;
            panelImage.color = color;
        }
    }
}
