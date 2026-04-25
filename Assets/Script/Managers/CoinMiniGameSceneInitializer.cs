using UnityEngine;

/// <summary>
/// Initialiseur de la scène CoinMiniGameScene.
/// Restaure la vie du joueur depuis PlayerData.savedHealth à chaque (re)chargement de la scène,
/// que ce soit l'entrée initiale ou un restart après une DeathZone.
/// </summary>
public class CoinMiniGameSceneInitializer : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    private void Start()
    {
        RestoreHealth();
        AudioManager.Instance?.PlayCoinMiniGameMusic();
    }

    /// <summary>Applique savedHealth sur le HealthSystem du joueur présent dans la scène.</summary>
    private void RestoreHealth()
    {
        if (playerData == null) return;

        HealthSystem healthSystem = FindFirstObjectByType<HealthSystem>();
        if (healthSystem != null)
        {
            healthSystem.SetCurrentHealth(playerData.savedHealth);
            Debug.Log($"[CoinMiniGame] Vie restaurée : {playerData.savedHealth}/{healthSystem.MaxHealth}");
        }
    }
}
