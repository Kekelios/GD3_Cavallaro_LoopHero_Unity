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
        if (treasureVisual != null && questToComplete != null)
        {
            bool shouldBeVisible = questToComplete.isActive && !hasBeenCollected;
            treasureVisual.SetActive(shouldBeVisible);
        }
    }

    public override void Activate(Pawn CurrentPawn)
    {
        if (questToComplete != null && !questToComplete.isActive)
        {
            Debug.Log("La quête n'est pas encore active. Parle d'abord au PNJ !");
            return;
        }

        if (hasBeenCollected)
        {
            Debug.Log("Ce trésor a déjà été collecté.");
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
                onChoiceSelected = OnChoiceA
            });

            treasureDialogue.runtimeChoices.Add(new DialogueChoice
            {
                choiceText = choiceBText,
                onChoiceSelected = OnChoiceB
            });

            DialogueManager.Instance.StartDialogue(treasureDialogue);
        }
        else
        {
            CollectTreasure(0);
        }
    }

    private void OnChoiceA()
    {
        Debug.Log("Choix A : Ouverture délicate, aucun dégât !");
        CollectTreasure(0);
    }

    private void OnChoiceB()
    {
        Debug.Log($"Choix B : Coffre forcé ! {choiceBDamage} dégâts subis !");
        CollectTreasure(choiceBDamage);
    }

    private void CollectTreasure(int damageAmount)
    {
        hasBeenCollected = true;

        if (questToComplete != null)
        {
            questToComplete.Complete();
        }

        if (treasureVisual != null)
        {
            treasureVisual.SetActive(false);
        }

        if (damageAmount > 0 && currentPawn != null)
        {
            HealthSystem healthSystem = currentPawn.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                healthSystem.TakeDamage(damageAmount);
            }
        }

        Debug.Log("Trésor collecté !");
    }
}
