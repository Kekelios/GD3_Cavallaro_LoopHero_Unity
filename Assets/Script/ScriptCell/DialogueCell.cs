using UnityEngine;

public class DialogueCell : Cell
{
    [Header("Dialogues")]
    [SerializeField] private DialogueData firstVisitDialogue;
    [SerializeField] private DialogueData alreadyVisitedDialogue;
    [SerializeField] private DialogueData afterQuestDialogue;

    [Header("Quest System")]
    [SerializeField] private QuestCondition questCondition;
    [SerializeField] private bool activatesQuest = true;
    [SerializeField] private bool triggersVictory = false;

    [Header("Keys")]
    [SerializeField] private PlayerData playerData;

    [Header("Visual Indicators")]
    [SerializeField] private GameObject questMarker;
    [SerializeField] private ParticleSystem questCompleteEffect;
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float pulseScale = 1.3f;

    private bool hasBeenVisited = false;
    private bool hasShownCompleteEffect = false;
    private Vector3 originalScale;

    private void Start()
    {
        if (questMarker != null)
        {
            questMarker.SetActive(true);
            originalScale = questMarker.transform.localScale;
        }

        if (questCompleteEffect != null)
            questCompleteEffect.gameObject.SetActive(false);

        if (DialogueManager.Instance != null)
            DialogueManager.Instance.OnDialogueEnded += OnDialogueFinished;
    }

    private void OnDestroy()
    {
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.OnDialogueEnded -= OnDialogueFinished;
    }

    private void Update()
    {
        if (questMarker != null && questMarker.activeSelf && questCondition != null)
        {
            if (questCondition.isCompleted && !hasShownCompleteEffect)
            {
                float scale = 1f + Mathf.Sin(Time.time * pulseSpeed) * (pulseScale - 1f) * 0.5f;
                questMarker.transform.localScale = originalScale * scale;
            }
            else if (!questCondition.isCompleted)
            {
                questMarker.transform.localScale = originalScale;
            }
        }
    }

    public override void Activate(Pawn CurrentPawn)
    {
        // La victoire nécessite la quête complétée ET les 2 clés
        bool allConditionsMet = (questCondition != null && questCondition.isCompleted)
                                && (playerData != null && playerData.keyCount >= 2);

        if (allConditionsMet)
        {
            triggersVictory = true;

            if (!hasShownCompleteEffect)
                ShowQuestCompleteEffect();
        }

        DialogueData dialogueToPlay = GetDialogueToPlay();

        if (dialogueToPlay != null)
        {
            DialogueManager.Instance.StartDialogue(dialogueToPlay);

            if (!hasBeenVisited)
            {
                hasBeenVisited = true;

                if (activatesQuest && questCondition != null && !questCondition.isActive)
                    questCondition.Activate();
            }
        }
    }

    private void OnDialogueFinished()
    {
        // Déclenche la victoire uniquement si toutes les conditions sont remplies
        bool allConditionsMet = triggersVictory
                                && (questCondition != null && questCondition.isCompleted)
                                && (playerData != null && playerData.keyCount >= 2);

        if (allConditionsMet)
        {
            if (VictoryManager.Instance != null)
                VictoryManager.Instance.TriggerVictory();
        }
    }

    /// <summary>
    /// Retourne le dialogue selon l'état des clés.
    /// Le dialogue final nécessite les 2 clés (TreasureCell + MiniGameCell).
    /// </summary>
    private DialogueData GetDialogueToPlay()
    {
        if (playerData != null && playerData.keyCount >= 2)
            return afterQuestDialogue;

        if (hasBeenVisited)
            return alreadyVisitedDialogue;

        return firstVisitDialogue;
    }

    private void ShowQuestCompleteEffect()
    {
        hasShownCompleteEffect = true;

        if (questMarker != null)
            questMarker.SetActive(false);

        if (questCompleteEffect != null)
        {
            questCompleteEffect.gameObject.SetActive(true);
            questCompleteEffect.Play();
        }

        Debug.Log("Quête complétée ! Effet visuel activé.");
    }
}
