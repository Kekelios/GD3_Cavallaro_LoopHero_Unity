using UnityEngine;

public class DialogueCell : Cell
{
    [SerializeField] private DialogueData dialogue;

    public override void Activate(Pawn CurrentPawn)
    {
        if (dialogue != null)
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }
        else
        {
            Debug.LogWarning("Aucun dialogue assigné à cette cellule !");
        }
    }
}
