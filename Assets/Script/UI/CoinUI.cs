using UnityEngine;
using TMPro;

/// <summary>
/// Affiche le compteur de pièces collectées dans le mini-jeu pièces.
/// Format : "X / N".
/// </summary>
public class CoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinCountText;

    private void Update()
    {
        if (coinCountText == null || CoinMiniGameManager.Instance == null)
            return;

        coinCountText.text = $"{CoinMiniGameManager.Instance.CoinsCollected} / {CoinMiniGameManager.Instance.TotalCoins}";
    }
}
