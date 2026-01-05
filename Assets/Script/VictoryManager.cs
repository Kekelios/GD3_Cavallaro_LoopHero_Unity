using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager Instance { get; private set; }

    [Header("Victory Settings")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private float delayBeforeVictory = 2f;
    [SerializeField] private string nextSceneName = "MainMenu";

    private bool hasWon = false;

    public bool HasWon => hasWon;

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
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    public void TriggerVictory()
    {
        if (hasWon) return;

        hasWon = true;
        Debug.Log("Victoire ! Niveau terminé !");

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayVictoryMusic();
        }

        StartCoroutine(VictorySequence());
    }

    private IEnumerator VictorySequence()
    {
        if (DiceButtonManager.Instance != null)
        {
            DiceButtonManager.Instance.HideDiceButton();
        }

        yield return new WaitForSeconds(delayBeforeVictory);

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneName);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
