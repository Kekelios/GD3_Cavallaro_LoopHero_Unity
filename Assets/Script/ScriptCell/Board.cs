using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Cell[] _cells;

    /// <summary>Retourne la cellule correspondant à l'index donné.</summary>
    public Cell GetCellByNumber(int number)
    {
        return _cells[number];
    }

    /// <summary>Retourne le prochain index valide en bouclant sur le tableau.</summary>
    public int GetNextCellToMove(int cellNumber)
    {
        return cellNumber % _cells.Length;
    }
}
