using UnityEngine;

/// <summary>
/// Déclenche la victoire quand le joueur entre dans la zone du coffre.
/// À placer sur le GameObject "Chest" avec un Collider en mode Trigger.
/// </summary>
public class ChestInteraction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // On vérifie que c'est bien le joueur qui touche le coffre
        if (other.CompareTag("Player"))
        {
            MiniGamesManager.Instance.OnKeyCollected();
        }
    }
}
