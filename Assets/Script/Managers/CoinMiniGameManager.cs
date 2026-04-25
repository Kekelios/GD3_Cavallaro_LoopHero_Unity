using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinMiniGameManager : MonoBehaviour
{
    private const string MainSceneName = "LoopHeroScene";
    private const int CatchDamage = 25;
    private const float VictoryDelay = 2f;

    [SerializeField] private PlayerData playerData;

    [Tooltip("Nombre total de pièces à ramasser dans la scène.")]
    [SerializeField] private int totalCoins;

    private int _coinsCollected;

    public static CoinMiniGameManager Instance { get; private set; }

    /// <summary>Nombre de pièces ramassées depuis le début de la scène.</summary>
    public int CoinsCollected => _coinsCollected;

    /// <summary>Nombre total de pièces dans la scène.</summary>
    public int TotalCoins => totalCoins;

    private void Awake()
    {
        Instance = this;
        _coinsCollected = 0;
    }

    /// <summary>Appelé par CoinPickup quand le joueur ramasse une pièce.</summary>
    public void OnCoinCollected()
    {
        _coinsCollected++;
        Debug.Log($"Pièce ramassée ! Collectées : {_coinsCollected}/{totalCoins}");

        if (_coinsCollected >= totalCoins)
            OnAllCoinsCollected();
    }

    /// <summary>Appelé quand toutes les pièces ont été ramassées.</summary>
    public void OnAllCoinsCollected()
    {
        // Sauvegarde la vie courante du HealthSystem avant de quitter
        HealthSystem healthSystem = FindFirstObjectByType<HealthSystem>();
        if (healthSystem != null)
            playerData.savedHealth = healthSystem.CurrentHealth;

        playerData.keyCount++;
        playerData.isCoinMiniGameCompleted = true;
        playerData.isReturningFromCoinMiniGame = true;

        Debug.Log($"Toutes les pièces collectées ! Clés : {playerData.keyCount}/3 – Vie conservée : {playerData.savedHealth}");

        AudioManager.Instance?.PlayChestOpenSound();
        AudioManager.Instance?.PlayVictorySFX();

        foreach (EnemyAI enemy in FindObjectsByType<EnemyAI>(FindObjectsSortMode.None))
        {
            enemy.gameObject.SetActive(false);
        }

        MiniGamePlayerController player = FindFirstObjectByType<MiniGamePlayerController>();
        if (player != null)
            player.TriggerVictory();

        Invoke(nameof(LoadMainScene), VictoryDelay);
    }

    /// <summary>Appelé par EnemyAI quand l'ennemi attrape le joueur.</summary>
    public void OnPlayerCaught()
    {
        playerData.savedHealth = Mathf.Max(0, playerData.savedHealth - CatchDamage);
        Debug.Log($"Attrapé ! Vie restante : {playerData.savedHealth}");

        AudioManager.Instance?.PlayTakeDamageSound();

        if (playerData.savedHealth <= 0)
        {
            AudioManager.Instance?.PlayGameOverSound();
            playerData.isReturningFromCoinMiniGame = false;
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
