using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SoundSettings : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [Header("Text Labels")]
    [SerializeField] private TMP_Text masterVolumeText;
    [SerializeField] private TMP_Text musicVolumeText;
    [SerializeField] private TMP_Text sfxVolumeText;

    [Header("Reset Button")]
    [SerializeField] private Button resetToDefaultsButton;



    private bool isInitialized = false;

    private void Start()
    {
        StartCoroutine(InitializeAfterAudioManager());
    }

    private IEnumerator InitializeAfterAudioManager()
    {
        yield return new WaitUntil(() => AudioManager.Instance != null);

        yield return new WaitForSeconds(0.1f);

        InitializeSliders();
        SetupCallbacks();
        isInitialized = true;

        Debug.Log("SoundSettings initialized with current volumes: " +
                 $"Master: {AudioManager.Instance.GetMasterVolumeDirect()}, " +
                 $"Music: {AudioManager.Instance.GetMusicVolumeDirect()}, " +
                 $"SFX: {AudioManager.Instance.GetSFXVolumeDirect()}");
    }

    private void OnEnable()
    {
        if (isInitialized && AudioManager.Instance != null)
        {
            RefreshSliders();
        }
    }

    private void InitializeSliders()
    {
        masterVolumeSlider.minValue = 0f;
        masterVolumeSlider.maxValue = 1f;

        musicVolumeSlider.minValue = 0f;
        musicVolumeSlider.maxValue = 1f;

        sfxVolumeSlider.minValue = 0f;
        sfxVolumeSlider.maxValue = 1f;

        RefreshSliders();

        if (resetToDefaultsButton != null)
        {
            resetToDefaultsButton.onClick.AddListener(ResetToDefaults);
        }
    }

    private void RefreshSliders()
    {
        if (AudioManager.Instance != null)
        {
            float masterVolume = AudioManager.Instance.GetMasterVolumeDirect();
            float musicVolume = AudioManager.Instance.GetMusicVolumeDirect();
            float sfxVolume = AudioManager.Instance.GetSFXVolumeDirect();

            SetSliderValueWithoutNotify(masterVolumeSlider, masterVolume);
            SetSliderValueWithoutNotify(musicVolumeSlider, musicVolume);
            SetSliderValueWithoutNotify(sfxVolumeSlider, sfxVolume);

            UpdateVolumeTexts();

            Debug.Log($"Sliders refreshed - Master: {masterVolume}, Music: {musicVolume}, SFX: {sfxVolume}");
        }
    }

    private void SetSliderValueWithoutNotify(Slider slider, float value)
    {
        slider.SetValueWithoutNotify(value);
    }

    private void SetupCallbacks()
    {
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void SetMasterVolume(float linearVolume)
    {
        AudioManager.Instance?.SetMasterVolume(linearVolume);
        UpdateVolumeTexts();
    }

    private void SetMusicVolume(float linearVolume)
    {
        AudioManager.Instance?.SetMusicVolume(linearVolume);
        UpdateVolumeTexts();
    }

    private void SetSFXVolume(float linearVolume)
    {
        AudioManager.Instance?.SetSFXVolume(linearVolume);
        UpdateVolumeTexts();
    }

    private void UpdateVolumeTexts()
    {
        if (masterVolumeText != null)
            masterVolumeText.text = $"{masterVolumeSlider.value * 100:F0}%";

        if (musicVolumeText != null)
            musicVolumeText.text = $"{musicVolumeSlider.value * 100:F0}%";

        if (sfxVolumeText != null)
            sfxVolumeText.text = $"{sfxVolumeSlider.value * 100:F0}%";
    }

    private void ResetToDefaults()
    {
        AudioManager.Instance?.PlaySound(AudioManager.buttonClickSound);
        AudioManager.Instance?.ResetToDefaultVolumes();

        StartCoroutine(RefreshAfterReset());
    }

    private IEnumerator RefreshAfterReset()
    {
        yield return new WaitForSeconds(0.1f);
        RefreshSliders();
    }

    private void OnDestroy()
    {
        masterVolumeSlider.onValueChanged.RemoveListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.RemoveListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.RemoveListener(SetSFXVolume);

        if (resetToDefaultsButton != null)
        {
            resetToDefaultsButton.onClick.RemoveListener(ResetToDefaults);
        }
    }
}