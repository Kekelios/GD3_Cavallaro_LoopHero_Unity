using UnityEngine;

/// <summary>
/// À placer sur chaque pièce collectable de la scène CoinMiniGameScene.
/// Le GameObject doit avoir un Collider en mode Trigger.
/// Le player doit avoir un Rigidbody kinematic pour que OnTriggerEnter soit déclenché.
/// </summary>
public class CoinPickup : MonoBehaviour
{
    private bool _collected = false;

    private void OnTriggerEnter(Collider other)
    {
        TryCollect(other);
    }

    // Filet de sécurité : si OnTriggerEnter est manqué (physique rapide ou frame skip)
    private void OnTriggerStay(Collider other)
    {
        TryCollect(other);
    }

    private void TryCollect(Collider other)
    {
        if (_collected) return;
        if (!other.CompareTag("Player")) return;

        _collected = true;
        Debug.Log($"[CoinPickup] Pièce collectée par {other.name}");
        AudioManager.Instance?.PlayCoinPickupSound();
        CoinMiniGameManager.Instance.OnCoinCollected();
        gameObject.SetActive(false);
    }
}
