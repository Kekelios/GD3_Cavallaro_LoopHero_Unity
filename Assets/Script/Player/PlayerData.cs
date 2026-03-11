using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public int _cellNumber;

    // Vie actuelle sauvegardée avant d'entrer dans le mini-jeu
    public int savedHealth;

    // True si le joueur a récupéré la clé dans le mini-jeu
    public bool hasKey;
}
