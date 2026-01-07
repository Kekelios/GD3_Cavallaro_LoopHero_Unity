using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public event Action<DialogueLine> OnDialogueLineChanged;
    public event Action OnDialogueStarted;
    public event Action OnDialogueEnded;
    public event Action<List<DialogueChoice>> OnChoicesAvailable;

    private DialogueData currentDialogue;
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;
    private bool waitingForChoice = false;

    public bool IsDialogueActive => isDialogueActive;
    public bool IsWaitingForChoice => waitingForChoice;

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
        waitingForChoice = false;

        OnDialogueStarted?.Invoke();
        DisplayCurrentLine();
    }

    public void ShowNextLine()
    {
        if (!isDialogueActive || waitingForChoice)
            return;

        currentLineIndex++;

        if (currentLineIndex >= currentDialogue.lines.Count)
        {
            if (currentDialogue.hasChoices && currentDialogue.runtimeChoices.Count > 0)
            {
                ShowChoices();
            }
            else
            {
                EndDialogue();
            }
        }
        else
        {
            DisplayCurrentLine();
        }
    }

    private void ShowChoices()
    {
        waitingForChoice = true;
        OnChoicesAvailable?.Invoke(currentDialogue.runtimeChoices);
    }

    public void SelectChoice(int choiceIndex)
    {
        if (!waitingForChoice || choiceIndex < 0 || choiceIndex >= currentDialogue.runtimeChoices.Count)
            return;

        waitingForChoice = false;
        currentDialogue.runtimeChoices[choiceIndex].onChoiceSelected?.Invoke();
        EndDialogue();
    }

    private void DisplayCurrentLine()
    {
        if (currentDialogue != null && currentLineIndex < currentDialogue.lines.Count)
        {
            OnDialogueLineChanged?.Invoke(currentDialogue.lines[currentLineIndex]);
        }
    }

    public DialogueLine GetCurrentLine()
    {
        if (currentDialogue != null && currentLineIndex < currentDialogue.lines.Count)
        {
            return currentDialogue.lines[currentLineIndex];
        }
        return null;
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        currentDialogue = null;
        currentLineIndex = 0;
        waitingForChoice = false;

        OnDialogueEnded?.Invoke();
    }
}
