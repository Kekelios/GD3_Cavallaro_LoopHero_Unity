using UnityEngine;

public class HealCell : Cell
{
    [Header("Heal Settings")]
    [SerializeField] private int healAmount = 20;
    [SerializeField] private bool canHealMultipleTimes = true;

    [Header("Visual")]
    [SerializeField] private GameObject healVisual;
    [SerializeField] private ParticleSystem healEffect;

    private bool hasBeenUsed = false;

    private void Start()
    {
        if (healVisual != null)
        {
            healVisual.SetActive(true);
        }

        if (healEffect != null)
        {
            healEffect.gameObject.SetActive(false);
        }
    }

    public override void Activate(Pawn CurrentPawn)
    {
        if (!canHealMultipleTimes && hasBeenUsed)
        {
            Debug.Log("Cette fontaine de soin a déjà été utilisée.");
            return;
        }

        HealthSystem healthSystem = CurrentPawn.GetComponent<HealthSystem>();

        if (healthSystem != null)
        {
            if (healthSystem.CurrentHealth >= healthSystem.MaxHealth)
            {
                Debug.Log("Ta vie est déjà au maximum !");
                return;
            }

            healthSystem.Heal(healAmount);
            hasBeenUsed = true;

            if (healEffect != null)
            {
                healEffect.gameObject.SetActive(true);
                healEffect.Play();
            }

            if (!canHealMultipleTimes && healVisual != null)
            {
                healVisual.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning("Le joueur n'a pas de HealthSystem !");
        }
    }
}
