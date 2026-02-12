using UnityEngine;

public class TreasureCell : Cell
{
    [Header("Quest")]
    [SerializeField] private QuestCondition questToComplete;

    [Header("Dialogue")]
    [SerializeField] private DialogueData treasureDialogue;
    [SerializeField] private string choiceAText = "Ouvrir délicatement le coffre";
    [SerializeField] private string choiceBText = "Forcer le coffre rapidement";
    [SerializeField] private int choiceBDamage = 50;

    [Header("Visual")]
    [SerializeField] private GameObject treasureVisual;

    private bool hasBeenCollected = false;
    private Pawn currentPawn;

    private void Start()
    {
        UpdateTreasureVisibility();
    }

    private void Update()
    {
        UpdateTreasureVisibility();
    }

    private void UpdateTreasureVisibility()
    {
        if (treasureVisual == null || questToComplete == null)
            return;

        // Visible uniquement si la quête est active, pas complétée, et pas déjà ramassé
        bool shouldBeVisible = questToComplete.isActive && !questToComplete.isCompleted && !hasBeenCollected;
        treasureVisual.SetActive(shouldBeVisible);
    }

    public override void Activate(Pawn CurrentPawn)
    {
        if (questToComplete != null && !questToComplete.isActive)
        {
            Debug.Log("La quête n'est pas encore active.");
            return;
        }

        if (hasBeenCollected || (questToComplete != null && questToComplete.isCompleted))
        {
            Debug.Log("Trésor déjà collecté.");
            return;
        }

        currentPawn = CurrentPawn;

        if (treasureDialogue != null)
        {
            treasureDialogue.hasChoices = true;
            treasureDialogue.runtimeChoices.Clear();

            treasureDialogue.runtimeChoices.Add(new DialogueChoice
            {
                choiceText = choiceAText,
                outcome = new ChoiceOutcome { outcomeType = ChoiceOutcomeType.None }
            });

            treasureDialogue.runtimeChoices.Add(new DialogueChoice
            {
                choiceText = choiceBText,
                outcome = new ChoiceOutcome { outcomeType = ChoiceOutcomeType.None }
            });

            //  IMPORTANT : passer l’instigator pour que le DialogueManager puisse rappeler ResolveChoice
            DialogueManager.Instance.StartDialogue(treasureDialogue, this);
        }
        else
        {
            CollectTreasure(0);
        }
    }

    public void ResolveChoice(int choiceIndex)
    {
        if (choiceIndex == 0)
        {
            Debug.Log("Ouverture délicate.");
            CollectTreasure(0);
        }
        else if (choiceIndex == 1)
        {
            Debug.Log("Coffre forcé.");
            CollectTreasure(choiceBDamage);
        }
    }

    private void CollectTreasure(int damageAmount)
    {
        hasBeenCollected = true;

        if (questToComplete != null && !questToComplete.isCompleted)
            questToComplete.Complete();

        if (treasureVisual != null)
            treasureVisual.SetActive(false);

        if (damageAmount > 0 && currentPawn != null)
        {
            HealthSystem healthSystem = currentPawn.GetComponent<HealthSystem>();
            if (healthSystem != null)
                healthSystem.TakeDamage(damageAmount);
        }

        Debug.Log("Trésor collecté !");
    }
}
