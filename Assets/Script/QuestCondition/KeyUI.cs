using UnityEngine;

/// <summary>
/// Affiche l'icône de clé dans le HUD si le joueur possède la clé.
/// À placer sur un GameObject "KeyIcon" dans le Canvas.
/// </summary>
public class KeyUI : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameObject keyIcon;  // L'image de la clé dans le Canvas

    private void Update()
    {
        // Active ou désactive l'icône selon si le joueur a la clé
        keyIcon.SetActive(playerData.hasKey);
    }
}
