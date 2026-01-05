using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueUI : MonoBehaviour
{
    [Header("Main UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button nextButton;

    [Header("Choices UI")]
    [SerializeField] private GameObject choicesPanel;
    [SerializeField] private Button choiceButtonPrefab;
    [SerializeField] private Transform choicesContainer;

    private List<Button> activeChoiceButtons = new List<Button>();

    private void Start()
    {
        DialogueManager.Instance.OnDialogueStarted += ShowDialoguePanel;
        DialogueManager.Instance.OnDialogueEnded += HideDialoguePanel;
        DialogueManager.Instance.OnDialogueLineChanged += UpdateDialogueUI;
        DialogueManager.Instance.OnChoicesAvailable += ShowChoices;

        nextButton.onClick.AddListener(OnNextButtonClicked);

        HideDialoguePanel();
        HideChoicesPanel();
    }

    private void OnDestroy()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStarted -= ShowDialoguePanel;
            DialogueManager.Instance.OnDialogueEnded -= HideDialoguePanel;
            DialogueManager.Instance.OnDialogueLineChanged -= UpdateDialogueUI;
            DialogueManager.Instance.OnChoicesAvailable -= ShowChoices;
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
        HideChoicesPanel();
    }

    private void UpdateDialogueUI(DialogueLine line)
    {
        characterNameText.text = line.characterName;
        dialogueText.text = line.text;
        nextButton.gameObject.SetActive(true);
        HideChoicesPanel();
    }

    private void ShowChoices(List<DialogueChoice> choices)
    {
        nextButton.gameObject.SetActive(false);
        choicesPanel.SetActive(true);

        ClearChoiceButtons();

        for (int i = 0; i < choices.Count; i++)
        {
            int choiceIndex = i;
            Button choiceButton = Instantiate(choiceButtonPrefab, choicesContainer);
            choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = choices[i].choiceText;
            choiceButton.onClick.AddListener(() => OnChoiceSelected(choiceIndex));
            activeChoiceButtons.Add(choiceButton);
        }
    }

    private void HideChoicesPanel()
    {
        choicesPanel.SetActive(false);
        ClearChoiceButtons();
    }

    private void ClearChoiceButtons()
    {
        foreach (Button button in activeChoiceButtons)
        {
            Destroy(button.gameObject);
        }
        activeChoiceButtons.Clear();
    }

    private void OnChoiceSelected(int choiceIndex)
    {
        DialogueManager.Instance.SelectChoice(choiceIndex);
    }

    private void OnNextButtonClicked()
    {
        DialogueManager.Instance.ShowNextLine();
    }
}
