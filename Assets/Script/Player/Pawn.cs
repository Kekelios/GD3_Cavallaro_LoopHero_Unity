using System.Collections;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private Board _board;

    [Header("Animation")]
    [SerializeField] private Transform visualRoot;
    [SerializeField] private float moveSpeed = 5f;

    private Animator _animator;
    private int _speedParam;

    /// <summary>Vrai pendant qu'un déplacement est en cours.</summary>
    public bool IsMoving { get; private set; } = false;

    private void Start()
    {
        _speedParam = Animator.StringToHash("Speed");

        if (_board == null) return;

        if (visualRoot != null)
            _animator = visualRoot.GetComponent<Animator>();

        MoveToCell();
    }

    /// <summary>Téléporte instantanément au démarrage sur la bonne case.</summary>
    private void MoveToCell()
    {
        Transform newPos = _board.GetCellByNumber(_playerData._cellNumber).transform;
        transform.position = newPos.position;
        transform.rotation = newPos.rotation;
    }

    /// <summary>Appelé par Dice — ignore si un déplacement est déjà en cours.</summary>
    public void TryMoving(int value)
    {
        if (IsMoving) return;

        _playerData._cellNumber = _board.GetNextCellToMove(_playerData._cellNumber + value);
        StartCoroutine(MoveAndActivate());
    }

    private IEnumerator MoveAndActivate()
    {
        IsMoving = true;
        DiceButtonManager.Instance?.HideDiceButton();

        Transform targetCell = _board.GetCellByNumber(_playerData._cellNumber).transform;
        Vector3 targetPosition = targetCell.position;
        Vector3 direction = targetPosition - transform.position;

        if (visualRoot != null && direction.sqrMagnitude > 0.001f)
        {
            visualRoot.rotation = Quaternion.LookRotation(direction);
        }

        if (_animator != null)
            _animator.SetFloat(_speedParam, 1f);

        while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;

        if (_animator != null)
            _animator.SetFloat(_speedParam, 0f);

        IsMoving = false;
        DiceButtonManager.Instance?.ShowDiceButton(); // ← ligne manquante
        ActivateCell();
    }


    private void ActivateCell()
    {
        Cell cell = _board.GetCellByNumber(_playerData._cellNumber);
        cell.Activate(CurrentPawn: this);
    }
}
