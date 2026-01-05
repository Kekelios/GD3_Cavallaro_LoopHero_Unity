using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioClip victoryMusic;
    [SerializeField] private float musicVolume = 0.5f;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip diceRollSound;
    [SerializeField] private AudioClip takeDamageSound;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private float sfxVolume = 0.7f;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetupAudioSources()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.playOnAwake = false;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.volume = sfxVolume;
        sfxSource.playOnAwake = false;
    }

    public void PlayMenuMusic()
    {
        if (menuMusic != null)
        {
            PlayMusic(menuMusic);
        }
    }

    public void PlayGameMusic()
    {
        if (gameMusic != null)
        {
            PlayMusic(gameMusic);
        }
    }

    public void PlayVictoryMusic()
    {
        if (victoryMusic != null)
        {
            PlayMusic(victoryMusic);
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayDiceRollSound()
    {
        PlaySFX(diceRollSound);
    }

    public void PlayTakeDamageSound()
    {
        PlaySFX(takeDamageSound);
    }

    public void PlayGameOverSound()
    {
        PlaySFX(gameOverSound);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
    }
}
