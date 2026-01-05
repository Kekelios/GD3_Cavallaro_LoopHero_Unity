using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button nextButton;

    private void Start()
    {
        DialogueManager.Instance.OnDialogueStarted += ShowDialoguePanel;
        DialogueManager.Instance.OnDialogueEnded += HideDialoguePanel;
        DialogueManager.Instance.OnDialogueLineChanged += UpdateDialogueUI;

        nextButton.onClick.AddListener(OnNextButtonClicked);

        HideDialoguePanel();
    }

    private void OnDestroy()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStarted -= ShowDialoguePanel;
            DialogueManager.Instance.OnDialogueEnded -= HideDialoguePanel;
            DialogueManager.Instance.OnDialogueLineChanged -= UpdateDialogueUI;
        }

        nextButton.onClick.RemoveListener(OnNextButtonClicked);
    }

    private void ShowDialoguePanel()
    {
        dialoguePanel.SetActive(true);
    }

    private void HideDialoguePanel()
    {
        dialoguePanel.SetActive(false);
    }

    private void UpdateDialogueUI(DialogueLine line)
    {
        characterNameText.text = line.characterName;
        dialogueText.text = line.text;
    }

    private void OnNextButtonClicked()
    {
        DialogueManager.Instance.ShowNextLine();
    }
}
