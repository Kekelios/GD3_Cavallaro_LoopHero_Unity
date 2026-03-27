using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameCell : Cell
{
    private const string MiniGameSceneName = "MiniGameSceneName";

    [SerializeField] private PlayerData playerData;
    [SerializeField] private HealthSystem healthSystem;

    /// <summary>Lance le mini-jeu si la clé du mini-jeu n'a pas encore été obtenue.</summary>
    public override void Activate(Pawn currentPawn)
    {
        // La clé du mini-jeu est la deuxième (index 1) — on vérifie qu'on ne l'a pas encore
        if (playerData.keyCount >= 2)
        {
            Debug.Log("Clé du mini-jeu déjà obtenue, case ignorée.");
            return;
        }

        playerData.savedHealth = healthSystem.CurrentHealth;
        SceneManager.LoadScene(MiniGameSceneName);
    }
}
