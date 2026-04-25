using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public int _cellNumber;
    public int savedHealth;
    public int keyCount;

    // True quand on revient du mini-jeu → GameInitializer ne reset pas
    public bool isReturningFromMiniGame;

    // True une fois le mini-jeu pièces terminé avec succès
    public bool isCoinMiniGameCompleted;

    // Flag de retour du mini-jeu pièces, même pattern que isReturningFromMiniGame
    public bool isReturningFromCoinMiniGame;
}
