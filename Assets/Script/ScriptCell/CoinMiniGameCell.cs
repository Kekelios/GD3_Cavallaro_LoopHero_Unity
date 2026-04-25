using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinMiniGameCell : Cell
{
    private const string CoinMiniGameSceneName = "CoinMiniGameScene";

    [SerializeField] private PlayerData playerData;
    [SerializeField] private HealthSystem healthSystem;

    /// <summary>Lance le mini-jeu pièces si celui-ci n'a pas encore été complété.</summary>
    public override void Activate(Pawn currentPawn)
    {
        if (playerData.isCoinMiniGameCompleted)
        {
            Debug.Log("Mini-jeu pièces déjà complété, case ignorée.");
            return;
        }

        playerData.savedHealth = healthSystem.CurrentHealth;
        SceneManager.LoadScene(CoinMiniGameSceneName);
    }
}
