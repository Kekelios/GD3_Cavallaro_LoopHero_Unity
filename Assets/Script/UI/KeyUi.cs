using UnityEngine;
using TMPro;

/// <summary>Affiche le compteur de cl�s X/2 dans le HUD.</summary>
public class KeyUI : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private TextMeshProUGUI keyCountText;

    private void Update()
    {
        if (keyCountText != null)
            keyCountText.text = $"{playerData.keyCount}/3";
    }
}
    