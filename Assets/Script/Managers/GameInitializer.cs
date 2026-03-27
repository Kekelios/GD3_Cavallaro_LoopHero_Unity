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
            // Retour du mini-jeu : on conserve tout, on restaure juste la vie
            playerData.isReturningFromMiniGame = false;
            RestoreHealth();
            Debug.Log($"Retour du mini-jeu. Clés : {playerData.keyCount}/2, Case : {playerData._cellNumber}");
        }
        else
        {
            // Vrai démarrage : reset complet
            playerData._cellNumber = 0;
            playerData.keyCount = 0;
            playerData.savedHealth = 0;

            foreach (var quest in questConditions)
            {
                if (quest != null)
                    quest.Reset();
            }

            Debug.Log("Nouvelle partie : données réinitialisées.");
        }
    }

    /// <summary>Applique la vie sauvegardée au HealthSystem du joueur.</summary>
    private void RestoreHealth()
    {
        if (playerData.savedHealth <= 0) return;

        HealthSystem healthSystem = FindFirstObjectByType<HealthSystem>();
        if (healthSystem != null)
            healthSystem.SetCurrentHealth(playerData.savedHealth);
    }
}
