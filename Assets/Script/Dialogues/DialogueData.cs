using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class DialogueLine
{
    public string characterName;
    [TextArea(2, 5)]
    public string text;
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public ChoiceOutcome outcome;
}

public enum ChoiceOutcomeType
{
    None,
    StartDialogue,
    SetFlag
}

[System.Serializable]
public class ChoiceOutcome
{
    public ChoiceOutcomeType outcomeType;

    // Pour StartDialogue
    public DialogueData nextDialogue;

    // Pour SetFlag (exemple simple)
    public string flagID;
}


[CreateAssetMenu(fileName = "NewDialogue", menuName = "Scriptable Objects/DialogueData")]
public class DialogueData : ScriptableObject
{
    public List<DialogueLine> lines = new List<DialogueLine>();
    public bool hasChoices = false;

    [HideInInspector]
    public List<DialogueChoice> runtimeChoices = new List<DialogueChoice>();
}
