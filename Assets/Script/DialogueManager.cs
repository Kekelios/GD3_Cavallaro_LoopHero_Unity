using System;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public event Action<DialogueLine> OnDialogueLineChanged;
    public event Action OnDialogueStarted;
    public event Action OnDialogueEnded;

    private DialogueData currentDialogue;
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;

    public bool IsDialogueActive => isDialogueActive;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartDialogue(DialogueData dialogue)
    {
        if (dialogue == null || dialogue.lines.Count == 0)
        {
            Debug.LogError("Le dialogue est vide ou null !");
            return;
        }

        currentDialogue = dialogue;
        currentLineIndex = 0;
        isDialogueActive = true;

        OnDialogueStarted?.Invoke();
        DisplayCurrentLine();
    }

    public void ShowNextLine()
    {
        if (!isDialogueActive)
            return;

        currentLineIndex++;

        if (currentLineIndex >= currentDialogue.lines.Count)
        {
            EndDialogue();
        }
        else
        {
            DisplayCurrentLine();
        }
    }

    private void DisplayCurrentLine()
    {
        if (currentDialogue != null && currentLineIndex < currentDialogue.lines.Count)
        {
            OnDialogueLineChanged?.Invoke(currentDialogue.lines[currentLineIndex]);
        }
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        currentDialogue = null;
        currentLineIndex = 0;

        OnDialogueEnded?.Invoke();
    }
}
