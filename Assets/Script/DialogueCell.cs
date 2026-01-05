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
        {
            questCompleteEffect.gameObject.SetActive(false);
        }
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
        if (questCondition != null && questCondition.isCompleted && !hasShownCompleteEffect)
        {
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
                {
                    questCondition.Activate();
                }
            }
        }
        else
        {
            Debug.LogWarning("Aucun dialogue assigné à cette cellule !");
        }
    }

    private DialogueData GetDialogueToPlay()
    {
        if (questCondition != null && questCondition.isCompleted && afterQuestDialogue != null)
        {
            return afterQuestDialogue;
        }

        if (hasBeenVisited && alreadyVisitedDialogue != null)
        {
            return alreadyVisitedDialogue;
        }

        return firstVisitDialogue;
    }

    private void ShowQuestCompleteEffect()
    {
        hasShownCompleteEffect = true;

        if (questMarker != null)
        {
            questMarker.SetActive(false);
        }

        if (questCompleteEffect != null)
        {
            questCompleteEffect.gameObject.SetActive(true);
            questCompleteEffect.Play();
        }

        Debug.Log("Quête complétée ! Effet visuel activé.");
    }
}
