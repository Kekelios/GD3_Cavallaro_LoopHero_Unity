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
    public Action onChoiceSelected;
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Scriptable Objects/DialogueData")]
public class DialogueData : ScriptableObject
{
    public List<DialogueLine> lines = new List<DialogueLine>();
    public bool hasChoices = false;

    [HideInInspector]
    public List<DialogueChoice> runtimeChoices = new List<DialogueChoice>();
}
