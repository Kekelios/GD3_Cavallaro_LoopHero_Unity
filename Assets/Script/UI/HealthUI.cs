using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private HealthSystem healthSystem;

    private void Awake()
    {
        if (healthSystem != null)
        {
            healthSystem.OnHealthChanged += UpdateHealthUI;
        }
        else
        {
            Debug.LogError("HealthSystem non assigné sur HealthUI !");
        }
    }

    private void Start()
    {
        if (healthSystem != null)
        {
            UpdateHealthUI(healthSystem.CurrentHealth, healthSystem.MaxHealth);
        }
    }

    private void OnDestroy()
    {
        if (healthSystem != null)
        {
            healthSystem.OnHealthChanged -= UpdateHealthUI;
        }
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (healthText != null)
        {
            healthText.text = $"HP: {currentHealth} / {maxHealth}";
        }
    }
}
