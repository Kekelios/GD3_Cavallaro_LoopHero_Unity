using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Header("Data to Reset")]
    [SerializeField] private PlayerData playerData;
    [SerializeField] private QuestCondition[] questConditions;

    private void Awake()
    {
        if (playerData == null) return;

        if (playerData.isReturningFromMiniGame)
        {
            playerData.isReturningFromMiniGame = false;

            Debug.Log($"Retour du mini-jeu. Clés : {playerData.keyCount}/2, Vie à restaurer : {playerData.savedHealth}");
        }
        else
        {
            playerData._cellNumber = 0;
            playerData.keyCount = 0;

            // ⚠️ IMPORTANT : on met la vie par défaut
            if (playerData.savedHealth <= 0)
            {
                playerData.savedHealth = 100;
            }

            foreach (var quest in questConditions)
            {
                if (quest != null)
                    quest.Reset();
            }

            Debug.Log("Nouvelle partie : données réinitialisées.");
        }
    }

    private void Start()
    {
        RestoreHealth();
    }

    private void RestoreHealth()
    {
        if (playerData == null) return;

        HealthSystem healthSystem = FindFirstObjectByType<HealthSystem>();

        if (healthSystem != null)
        {
            healthSystem.SetCurrentHealth(playerData.savedHealth);
            Debug.Log($"Vie restaurée : {playerData.savedHealth}");
        }
    }
}