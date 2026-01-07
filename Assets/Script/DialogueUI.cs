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

    private TextMeshProUGUI choiceCharacterNameText;
    private TextMeshProUGUI choiceDialogueText;
    private List<Button> activeChoiceButtons = new List<Button>();
    private DialogueLine lastDisplayedLine;

    private void Start()
    {
        FindChoicePanelTexts();

        DialogueManager.Instance.OnDialogueStarted += ShowDialoguePanel;
        DialogueManager.Instance.OnDialogueEnded += HideDialoguePanel;
        DialogueManager.Instance.OnDialogueLineChanged += UpdateDialogueUI;
        DialogueManager.Instance.OnChoicesAvailable += ShowChoices;

        nextButton.onClick.AddListener(OnNextButtonClicked);

        characterNameText.text = "";
        dialogueText.text = "";

        if (choiceCharacterNameText != null)
            choiceCharacterNameText.text = "";
        if (choiceDialogueText != null)
            choiceDialogueText.text = "";

        HideDialoguePanel();
        HideChoicesPanel();
    }

    private void FindChoicePanelTexts()
    {
        if (choicesPanel != null)
        {
            TextMeshProUGUI[] texts = choicesPanel.GetComponentsInChildren<TextMeshProUGUI>(true);

            foreach (TextMeshProUGUI text in texts)
            {
                if (text.gameObject.name == "CharacterNameText")
                {
                    choiceCharacterNameText = text;
                }
                else if (text.gameObject.name == "DialogueText")
                {
                    choiceDialogueText = text;
                }
            }
        }
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
        lastDisplayedLine = line;
        characterNameText.text = line.characterName;
        dialogueText.text = line.text;
        nextButton.gameObject.SetActive(true);
        HideChoicesPanel();
    }

    private void ShowChoices(List<DialogueChoice> choices)
    {
        dialoguePanel.SetActive(false);
        choicesPanel.SetActive(true);

        if (choiceCharacterNameText != null && lastDisplayedLine != null)
        {
            choiceCharacterNameText.text = lastDisplayedLine.characterName;
            choiceDialogueText.text = lastDisplayedLine.text;
        }

        ClearChoiceButtons();

        for (int i = 0; i < choices.Count; i++)
        {
            int choiceIndex = i;
            Button choiceButton = Instantiate(choiceButtonPrefab, choicesContainer);
            choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = choices[i].choiceText;
            choiceButton.onClick.AddListener(() => OnChoiceSelected(choiceIndex));
            choiceButton.gameObject.SetActive(true);
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
            if (button != null)
            {
                Destroy(button.gameObject);
            }
        }
        activeChoiceButtons.Clear();
    }

    private void OnChoiceSelected(int choiceIndex)
    {
        DialogueManager.Instance.SelectChoice(choiceIndex);
        HideChoicesPanel();
    }

    private void OnNextButtonClicked()
    {
        DialogueManager.Instance.ShowNextLine();
    }
}
