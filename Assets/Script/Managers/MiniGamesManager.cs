using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gère le déroulement du mini-jeu : victoire (clé trouvée) et défaite (attrapé).
/// À placer sur un GameObject vide "MiniGameManager" dans la scène MiniGameScene.
/// </summary>
public class MiniGamesManager : MonoBehaviour
{
    private const string MainSceneName = "LoopHeroScene";
    private const int CatchDamage = 25;

    [SerializeField] private PlayerData playerData;

    // Référence statique simple pour que les autres scripts puissent l'appeler
    public static MiniGamesManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Appelé par ChestInteraction quand le joueur ouvre le coffre.
    /// </summary>
    public void OnKeyCollected()
    {
        playerData.hasKey = true;
        Debug.Log("Clé récupérée ! Retour au jeu principal.");
        SceneManager.LoadScene(MainSceneName);
    }

    /// <summary>
    /// Appelé par EnemyAI quand l'ennemi touche le joueur.
    /// </summary>
    public void OnPlayerCaught()
    {
        // On retire 25 PV de la vie sauvegardée
        playerData.savedHealth = Mathf.Max(0, playerData.savedHealth - CatchDamage);
        Debug.Log($"Joueur attrapé ! Vie restante : {playerData.savedHealth}");

        // On recharge la scène mini-jeu pour recommencer
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
