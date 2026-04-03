using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Header("Data to Reset")]
    [SerializeField] private PlayerData playerData;
    [SerializeField] private QuestCondition[] questConditions;

    private const int DefaultHealth = 100;

    private void Awake()
    {
        if (playerData == null) return;

        if (playerData.isReturningFromMiniGame)
        {
            playerData.isReturningFromMiniGame = false;
            Debug.Log($"Retour du mini-jeu. Clés : {playerData.keyCount}/2, Vie : {playerData.savedHealth}");
        }
        else
        {
            // Nouvelle partie ou Game Over → reset complet
            playerData._cellNumber = 0;
            playerData.keyCount = 0;
            playerData.savedHealth = DefaultHealth; // ← toujours 100, sans condition

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

    /// <summary>Applique le savedHealth du ScriptableObject sur le HealthSystem de la scène.</summary>
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
