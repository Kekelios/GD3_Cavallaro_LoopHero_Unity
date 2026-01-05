using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string characterName;
    [TextArea(2, 5)]
    public string text;
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Scriptable Objects/DialogueData")]
public class DialogueData : ScriptableObject
{
    public List<DialogueLine> lines = new List<DialogueLine>();
}
