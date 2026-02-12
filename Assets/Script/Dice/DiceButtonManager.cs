using UnityEngine;

public class DiceButtonManager : MonoBehaviour
{
    public static DiceButtonManager Instance { get; private set; }

    [SerializeField] private GameObject diceButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SubscribeToDialogueEvents();
        ShowDiceButton();
    }

    private void SubscribeToDialogueEvents()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStarted += HideDiceButton;
            DialogueManager.Instance.OnDialogueEnded += ShowDiceButton;
            DialogueManager.Instance.OnChoicesAvailable += OnChoicesShown;
        }
    }

    private void OnDestroy()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStarted -= HideDiceButton;
            DialogueManager.Instance.OnDialogueEnded -= ShowDiceButton;
            DialogueManager.Instance.OnChoicesAvailable -= OnChoicesShown;
        }
    }

    private void OnChoicesShown(System.Collections.Generic.List<DialogueChoice> choices)
    {
        HideDiceButton();
    }

    public void ShowDiceButton()
    {
        if (diceButton != null)
        {
            diceButton.SetActive(true);
        }
    }

    public void HideDiceButton()
    {
        if (diceButton != null)
        {
            diceButton.SetActive(false);
        }
    }
}
