using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Case spéciale qui lance le mini-jeu de cache-cache.
/// Hérite de Cell, comme toutes les cases du plateau.
/// </summary>
public class MiniGameCell : Cell
{
    // Nom exact de la scène mini-jeu dans Build Settings
    private const string MiniGameSceneName = "MiniGameScene";

    [SerializeField] private PlayerData playerData;
    [SerializeField] private HealthSystem healthSystem;

    /// <summary>
    /// Appelé automatiquement par Pawn.cs quand le joueur arrive sur cette case.
    /// </summary>
    public override void Activate(Pawn currentPawn)
    {
        // Si la clé est déjà obtenue, la case est désactivée
        if (playerData.hasKey)
        {
            Debug.Log("Clé déjà obtenue, case ignorée.");
            return;
        }

        // On sauvegarde la vie actuelle dans PlayerData avant de changer de scène
        playerData.savedHealth = healthSystem.CurrentHealth;

        Debug.Log("Mini-jeu lancé !");
        SceneManager.LoadScene(MiniGameSceneName);
    }
}
