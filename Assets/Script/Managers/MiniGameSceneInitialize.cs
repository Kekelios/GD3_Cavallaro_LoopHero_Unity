using UnityEngine;

/// <summary>
/// Lance la musique du mini-jeu au démarrage de la scène.
/// À placer sur un GameObject dans MiniGameSceneName.
/// </summary>
public class MiniGameSceneInitializer : MonoBehaviour
{
    private void Start()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayMiniGameMusic();
    }
}
