using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string gameSceneName = "LoopHeroScene";

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMenuMusic();
        }
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
