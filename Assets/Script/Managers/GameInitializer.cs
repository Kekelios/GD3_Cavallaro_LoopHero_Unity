using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Header("Data to Reset")]
    [SerializeField] private PlayerData playerData;
    [SerializeField] private QuestCondition[] questConditions;

    private void Awake()
    {
        if (playerData != null)
        {
            playerData._cellNumber = 0;
            Debug.Log("PlayerData réinitialisé : position = 0");
        }

        foreach (var quest in questConditions)
        {
            if (quest != null)
            {
                quest.Reset();
            }
        }

        Debug.Log("Toutes les quêtes ont été réinitialisées.");
    }
}
