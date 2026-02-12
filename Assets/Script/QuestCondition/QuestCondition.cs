using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestCondition", menuName = "Scriptable Objects/QuestCondition")]
public class QuestCondition : ScriptableObject
{
    public bool isActive = false;
    public bool isCompleted = false;

    public void Activate()
    {
        isActive = true;
        Debug.Log($"Quête activée : {name}");
    }

    public void Complete()
    {
        if (!isActive)
        {
            Debug.LogWarning($"Impossible de compléter '{name}' : la quête n'est pas active.");
            return;
        }

        isCompleted = true;
        Debug.Log($"Quête complétée : {name}");
    }

    public void Reset()
    {
        isActive = false;
        isCompleted = false;
    }
}
