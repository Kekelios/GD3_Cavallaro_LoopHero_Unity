using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGamesManager : MonoBehaviour
{
    private const string MainSceneName = "LoopHeroScene";
    private const int CatchDamage = 25;

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
        // savedHealth est déjà correct : il a été décrémenté par OnPlayerCaught() à chaque capture
        Debug.Log($"Clé récupérée ! Total : {playerData.keyCount}/2 – Vie conservée : {playerData.savedHealth}");
        SceneManager.LoadScene(MainSceneName);
    }

    /// <summary>Appelé par EnemyAI quand l'ennemi touche le joueur.</summary>
    public void OnPlayerCaught()
    {
        playerData.savedHealth = Mathf.Max(0, playerData.savedHealth - CatchDamage);
        Debug.Log($"Attrapé ! Nouvelle vie : {playerData.savedHealth}");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
