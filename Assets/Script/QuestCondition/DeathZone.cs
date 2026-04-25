using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// À placer sur une Plane avec un Collider en mode Trigger.
/// Quand le joueur la touche :
///   - inflige DeathDamage points de dégâts à PlayerData.savedHealth
///   - recharge la scène (retour au point de spawn)
///   - si la vie tombe à 0 → Game Over, retour au menu principal
/// </summary>
public class DeathZone : MonoBehaviour
{
    private const string MainSceneName = "LoopHeroScene";
    private const int DeathDamage = 25;

    [SerializeField] private PlayerData playerData;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerData.savedHealth = Mathf.Max(0, playerData.savedHealth - DeathDamage);
        Debug.Log($"[DeathZone] -{DeathDamage} PV. Vie restante : {playerData.savedHealth}");

        AudioManager.Instance?.PlayDeathZoneSound();

        if (playerData.savedHealth <= 0)
        {
            AudioManager.Instance?.PlayGameOverSound();
            playerData.isReturningFromCoinMiniGame = false;
            Debug.Log("[DeathZone] Game Over ! Retour à la scène principale.");
            SceneManager.LoadScene(MainSceneName);
        }
        else
        {
            // Restart de la scène → CoinMiniGameSceneInitializer restaurera la vie
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
