using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioClip miniGameMusic;
    [SerializeField] private AudioClip victoryMusic;
    [SerializeField] private float musicVolume = 0.5f;

    [Header("Sound Effects – Player")]
    [SerializeField] private AudioClip diceRollSound;
    [SerializeField] private AudioClip takeDamageSound;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip footstepSoundA;
    [SerializeField] private AudioClip footstepSoundB;


    [Header("Sound Effects – Mini-Game")]
    [SerializeField] private AudioClip chestOpenSound;
    [SerializeField] private AudioClip victorySFX;

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

    // ── Music ────────────────────────────────────────────────────────

    /// <summary>Musique de la LoopHeroScene.</summary>
    public void PlayGameMusic() => PlayMusic(gameMusic);

    /// <summary>Musique exclusive au mini-jeu.</summary>
    public void PlayMiniGameMusic() => PlayMusic(miniGameMusic);

    public void PlayMenuMusic() => PlayMusic(menuMusic);

    public void PlayVictoryMusic() => PlayMusic(victoryMusic);

    public void StopMusic() => musicSource.Stop();

    private void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    // ── SFX – Player ─────────────────────────────────────────────────

    public void PlayDiceRollSound() => PlaySFX(diceRollSound);

    public void PlayTakeDamageSound() => PlaySFX(takeDamageSound);

    public void PlayGameOverSound() => PlaySFX(gameOverSound);

    private bool _footstepToggle = false;

    /// <summary>Alterne entre footstepSoundA et footstepSoundB à chaque appel.</summary>
    public void PlayFootstepSound()
    {
        AudioClip clip = _footstepToggle ? footstepSoundB : footstepSoundA;
        _footstepToggle = !_footstepToggle;
        PlaySFX(clip);
    }


    // ── SFX – Mini-Game ───────────────────────────────────────────────

    /// <summary>Son du coffre qui s'ouvre.</summary>
    public void PlayChestOpenSound() => PlaySFX(chestOpenSound);

    /// <summary>Fanfare de victoire quand le coffre est récupéré.</summary>
    public void PlayVictorySFX() => PlaySFX(victorySFX);

    // ── Generic ───────────────────────────────────────────────────────

    /// <summary>Joue un clip SFX quelconque via le source global.</summary>
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip, sfxVolume);
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
