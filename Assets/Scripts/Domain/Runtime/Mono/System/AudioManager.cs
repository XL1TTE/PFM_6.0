using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(0.1f, 3f)]
        public float pitch = 1f;
        public bool loop = false;

        [HideInInspector]
        public AudioSource source;
    }

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Sound Settings")]
    [SerializeField] private Sound[] sounds;
    [SerializeField] private Sound[] musicTracks;

    [Header("Music Transition Settings")]
    [SerializeField] private float musicFadeDuration = 3f;
    [SerializeField] private float musicDuckVolume = 0.3f;

    [Header("Default Volume Settings")]
    [SerializeField] private float defaultMasterVolume = 0.8f;
    [SerializeField] private float defaultMusicVolume = 0.8f;
    [SerializeField] private float defaultSFXVolume = 0.8f;

    public const string buttonClickSound = "ButtonClick";
    public const string buttonErrorSound = "ButtonError";
    public const string putSound = "UIPut";
    public const string whooshSound = "Whoosh";

    public const string createMonsterSound = "CreateMonster";
    public const string deleteMonsterSound = "DeleteMonster";

    public const string moveSound = "MoveSound";
    public const string turnSound = "TurnSound";
    //public const string playerMoveSound = "PlayerMoveSound";
    //public const string enemyMoveSound = "EnemyMoveSound";
    public const string monsterTakeDamageSound = "MonsterTakeDamage";
    public const string enemyTakeDamageSound = "EnemyTakeDamage";
    public const string monsterDieSound = "MonsterDie";
    public const string enemyDieeSound = "EnemyDie";
    public const string winSound = "WinSound";
    public const string lossSound = "LossSound";

    private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();
    private Dictionary<string, Sound> musicDictionary = new Dictionary<string, Sound>();

    private Sound currentMusic;
    private Sound nextMusic;
    private Coroutine musicTransitionCoroutine;

    private Dictionary<string, float> musicPlaybackPositions = new Dictionary<string, float>();

    // Volume parameters
    private const string MASTER_VOLUME_PARAM = "MasterVolume";
    private const string MUSIC_VOLUME_PARAM = "MusicVolume";
    private const string SFX_VOLUME_PARAM = "SFXVolume";

    // PlayerPrefs keys
    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    public static AudioManager Instance { get; private set; }

    // ������� ��� ��������� ���������
    public System.Action<float> OnMasterVolumeChanged;
    public System.Action<float> OnMusicVolumeChanged;
    public System.Action<float> OnSFXVolumeChanged;

    private float currentMasterVolume;
    private float currentMusicVolume;
    private float currentSFXVolume;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudio();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void InitializeAudio()
    {
        foreach (Sound sound in sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.volume = sound.volume;
            source.pitch = sound.pitch;
            source.loop = sound.loop;
            source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];

            sound.source = source;
            soundDictionary[sound.name] = sound;
        }

        foreach (Sound music in musicTracks)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = music.clip;
            source.volume = 1.0f;
            source.pitch = music.pitch;
            source.loop = music.loop;
            source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];

            music.source = source;
            musicDictionary[music.name] = music;
        }

        LoadVolumeSettings();

        CheckAudioMixerConnection();
    }


    private void LoadVolumeSettings()
    {
        bool hasSavedSettings = PlayerPrefs.HasKey(MASTER_VOLUME_KEY);

        if (hasSavedSettings)
        {
            currentMasterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, defaultMasterVolume);
            currentMusicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, defaultMusicVolume);
            currentSFXVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, defaultSFXVolume);
        }
        else
        {
            currentMasterVolume = defaultMasterVolume;
            currentMusicVolume = defaultMusicVolume;
            currentSFXVolume = defaultSFXVolume;

            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, currentMasterVolume);
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, currentMusicVolume);
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, currentSFXVolume);
            PlayerPrefs.Save();
        }

        ApplyVolumeSettings();
    }

    private void ApplyVolumeSettings()
    {
        SetMasterVolumeImmediate(currentMasterVolume);
        SetMusicVolumeImmediate(currentMusicVolume);
        SetSFXVolumeImmediate(currentSFXVolume);
    }
    public float GetMasterVolumeDirect() => currentMasterVolume;
    public float GetMusicVolumeDirect() => currentMusicVolume;
    public float GetSFXVolumeDirect() => currentSFXVolume;

    public float GetMasterVolumeLinear()
    {
        if (audioMixer.GetFloat(MASTER_VOLUME_PARAM, out float volumeDb))
        {
            float linear = ConvertDbToLinear(volumeDb);
            return linear;
        }
        return currentMasterVolume;
    }

    public float GetMusicVolumeLinear()
    {
        if (audioMixer.GetFloat(MUSIC_VOLUME_PARAM, out float volumeDb))
        {
            float linear = ConvertDbToLinear(volumeDb);
            return linear;
        }
        return currentMusicVolume;
    }

    public float GetSFXVolumeLinear()
    {
        if (audioMixer.GetFloat(SFX_VOLUME_PARAM, out float volumeDb))
        {
            float linear = ConvertDbToLinear(volumeDb);
            return linear;
        }
        return currentSFXVolume;
    }
    private void SetMasterVolumeImmediate(float linearVolume)
    {
        float dbVolume = ConvertLinearToDb(linearVolume);
        audioMixer.SetFloat(MASTER_VOLUME_PARAM, dbVolume);
    }

    private void SetMusicVolumeImmediate(float linearVolume)
    {
        float dbVolume = ConvertLinearToDb(linearVolume);
        audioMixer.SetFloat(MUSIC_VOLUME_PARAM, dbVolume);
    }

    private void SetSFXVolumeImmediate(float linearVolume)
    {
        float dbVolume = ConvertLinearToDb(linearVolume);
        audioMixer.SetFloat(SFX_VOLUME_PARAM, dbVolume);
    }

    private void SaveVolumeSettings()
    {
        float masterVolume = GetMasterVolumeLinear();
        float musicVolume = GetMusicVolumeLinear();
        float sfxVolume = GetSFXVolumeLinear();

        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, masterVolume);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, musicVolume);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, sfxVolume);
        PlayerPrefs.Save();
    }

    public void PlaySound(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            soundDictionary[soundName].source.Play();
        }
    }

    public void PlaySound(string soundName, Vector3 position)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            AudioSource.PlayClipAtPoint(soundDictionary[soundName].clip, position);
        }
    }

    public void PlayMusic(string musicName, bool forceRestart = false, bool resumeFromLastPosition = false, bool waitForSceneLoad = false)
    {
        if (!musicDictionary.ContainsKey(musicName))
        {
            return;
        }

        Sound newMusic = musicDictionary[musicName];

        if (currentMusic == newMusic && !forceRestart)
            return;

        if (musicTransitionCoroutine != null)
            StopCoroutine(musicTransitionCoroutine);

        if (currentMusic != null)
        {
            musicPlaybackPositions[currentMusic.name] = currentMusic.source.time;
        }

        nextMusic = newMusic;

        if (waitForSceneLoad)
        {
            musicTransitionCoroutine = StartCoroutine(DelayedMusicTransition(resumeFromLastPosition));
        }
        else
        {
            musicTransitionCoroutine = StartCoroutine(MusicTransitionCoroutine(resumeFromLastPosition));
        }
    }

    private IEnumerator DelayedMusicTransition(bool resumeFromLastPosition = false)
    {
        yield return new WaitUntil(() => !IsSceneLoading());

        yield return new WaitForSeconds(0.1f);

        yield return StartCoroutine(MusicTransitionCoroutine(resumeFromLastPosition));
    }

    private bool IsSceneLoading()
    {
        return LoadingScreen.Instance != null && LoadingScreen.Instance.GetIsLoading();
    }

    private IEnumerator MusicTransitionCoroutine(bool resumeFromLastPosition = false)
    {
        if (currentMusic != null)
        {
            yield return StartCoroutine(FadeMusicViaMixer(currentMusic, -80f, musicFadeDuration * 0.5f));
            currentMusic.source.Pause();
        }

        nextMusic.source.volume = 1.0f;
        nextMusic.source.Play();

        float targetVolumeDb = ConvertLinearToDb(currentMusicVolume);
        yield return StartCoroutine(FadeMusicViaMixer(nextMusic, targetVolumeDb, musicFadeDuration));

        currentMusic = nextMusic;
        nextMusic = null;
        musicTransitionCoroutine = null;
    }

    private IEnumerator FadeMusicViaMixer(Sound music, float targetVolumeDb, float duration)
    {
        audioMixer.GetFloat(MUSIC_VOLUME_PARAM, out float startVolumeDb);

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float currentVolumeDb = Mathf.Lerp(startVolumeDb, targetVolumeDb, timer / duration);
            audioMixer.SetFloat(MUSIC_VOLUME_PARAM, currentVolumeDb);
            yield return null;
        }

        audioMixer.SetFloat(MUSIC_VOLUME_PARAM, targetVolumeDb);
    }

    public void UpdateCurrentMusicVolume()
    {
        if (currentMusic != null && currentMusic.source != null)
        {
            float targetVolumeDb = ConvertLinearToDb(currentMusicVolume);
            audioMixer.SetFloat(MUSIC_VOLUME_PARAM, targetVolumeDb);
            Debug.Log($"Updated music mixer volume to: {targetVolumeDb}dB (setting: {currentMusicVolume})");
        }
    }

    public void SetMusicVolume(float linearVolume)
    {
        currentMusicVolume = Mathf.Clamp01(linearVolume);
        SetMusicVolumeImmediate(currentMusicVolume);
        UpdateCurrentMusicVolume();
        SaveVolumeSettings();
    }

    public void SetMasterVolume(float linearVolume)
    {
        currentMasterVolume = Mathf.Clamp01(linearVolume);
        SetMasterVolumeImmediate(currentMasterVolume);
        UpdateCurrentMusicVolume();
        SaveVolumeSettings();
    }

    public void SetSFXVolume(float linearVolume)
    {
        currentSFXVolume = Mathf.Clamp01(linearVolume);
        SetSFXVolumeImmediate(currentSFXVolume);
        SaveVolumeSettings();
    }

    private float ConvertDbToLinear(float db)
    {
        if (db <= -80f)
            return 0f;

        return Mathf.Pow(10f, db / 20f);
    }

    private float ConvertLinearToDb(float linear)
    {
        if (linear <= 0.0001f)
            return -80f;

        return Mathf.Log10(linear) * 20f;
    }

    public void ResetToDefaultVolumes()
    {
        SetMasterVolume(defaultMasterVolume);
        SetMusicVolume(defaultMusicVolume);
        SetSFXVolume(defaultSFXVolume);
    }

    public void StopMusic()
    {
        if (musicTransitionCoroutine != null)
            StopCoroutine(musicTransitionCoroutine);

        if (currentMusic != null)
        {
            StartCoroutine(FadeMusicViaMixer(null, -80f, musicFadeDuration));
            currentMusic.source.Stop();
            currentMusic = null;
        }
    }

    private void CheckAudioMixerConnection()
    {
        var groups = audioMixer.FindMatchingGroups("");;

        audioMixer.GetFloat(MASTER_VOLUME_PARAM, out float masterDb);
        audioMixer.GetFloat(MUSIC_VOLUME_PARAM, out float musicDb);
        audioMixer.GetFloat(SFX_VOLUME_PARAM, out float sfxDb);
    }
    public void PauseMusic()
    {
        if (currentMusic != null)
            currentMusic.source.Pause();
    }

    public void ResumeMusic()
    {
        if (currentMusic != null)
            currentMusic.source.UnPause();
    }
}
