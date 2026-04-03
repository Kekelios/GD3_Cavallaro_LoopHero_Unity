using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGamesManager : MonoBehaviour
{
    private const string MainSceneName = "LoopHeroScene";
    private const int CatchDamage = 25;
    private const float VictoryDelay = 2f;

    [SerializeField] private PlayerData playerData;

    public static MiniGamesManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>Appelé par ChestInteraction quand le joueur ouvre le coffre.</summary>
    public void OnKeyCollected()
    {
        playerData.keyCount++;
        playerData.isReturningFromMiniGame = true;
        Debug.Log($"Clé récupérée ! Total : {playerData.keyCount}/2 – Vie conservée : {playerData.savedHealth}");

        // Sons coffre + victoire
        AudioManager.Instance?.PlayChestOpenSound();
        AudioManager.Instance?.PlayVictorySFX();

        // Désactive tous les ennemis immédiatement
        foreach (EnemyAI enemy in FindObjectsByType<EnemyAI>(FindObjectsSortMode.None))
        {
            enemy.gameObject.SetActive(false);
        }

        // Animation Victory
        MiniGamePlayerController player = FindFirstObjectByType<MiniGamePlayerController>();
        if (player != null)
            player.TriggerVictory();

        Invoke(nameof(LoadMainScene), VictoryDelay);
    }

    /// <summary>Appelé par EnemyAI quand l'ennemi touche le joueur.</summary>
    public void OnPlayerCaught()
    {
        playerData.savedHealth = Mathf.Max(0, playerData.savedHealth - CatchDamage);
        Debug.Log($"Attrapé ! Vie restante : {playerData.savedHealth}");

        AudioManager.Instance?.PlayTakeDamageSound();

        if (playerData.savedHealth <= 0)
        {
            AudioManager.Instance?.PlayGameOverSound();
            playerData.isReturningFromMiniGame = false;
            Debug.Log("Game Over ! Retour au début de la partie.");
            SceneManager.LoadScene(MainSceneName);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene(MainSceneName);
    }
}
