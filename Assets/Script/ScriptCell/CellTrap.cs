using UnityEngine;

public class TrapCell : Cell
{
    [Header("Trap Settings")]
    [SerializeField] private int damageAmount = 15;
    [SerializeField] private bool canDamageMultipleTimes = true;

    [Header("Visual")]
    [SerializeField] private GameObject trapVisual;
    [SerializeField] private ParticleSystem damageEffect;
    [SerializeField] private Color trapColor = new Color(0.8f, 0.1f, 0.1f);

    private bool hasBeenTriggered = false;
    private Renderer cellRenderer;

    private void Start()
    {
        cellRenderer = GetComponent<Renderer>();

        if (cellRenderer != null)
        {
            cellRenderer.material.color = trapColor;
        }

        if (trapVisual != null)
        {
            trapVisual.SetActive(true);
        }

        if (damageEffect != null)
        {
            damageEffect.gameObject.SetActive(false);
        }
    }

    public override void Activate(Pawn CurrentPawn)
    {
        if (!canDamageMultipleTimes && hasBeenTriggered)
        {
            Debug.Log("Ce piège a déjà été déclenché.");
            return;
        }

        HealthSystem healthSystem = CurrentPawn.GetComponent<HealthSystem>();

        if (healthSystem != null)
        {
            healthSystem.TakeDamage(damageAmount);
            hasBeenTriggered = true;

            if (damageEffect != null)
            {
                damageEffect.gameObject.SetActive(true);
                damageEffect.Play();
            }

            Debug.Log($"Piège activé ! {damageAmount} dégâts infligés.");
        }
        else
        {
            Debug.LogWarning("Le joueur n'a pas de HealthSystem !");
        }
    }
}
