using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private Pawn _pawn;

    /// <summary>Lancé par le bouton UI — ignoré si le pawn est déjà en déplacement.</summary>
    public void RollTheDice()
    {
        if (_pawn.IsMoving) return;

        int value = Random.Range(1, 3);
        Debug.Log($"Le dé a fait {value}");

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayDiceRollSound();

        _pawn.TryMoving(value);
    }
}
