using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] AudioMixer mixer;
    [SerializeField] MenuNavigator settingsNavigator;

    [Header("Master Volume")]
    [SerializeField] Slider sliderMasterVolume;
    [SerializeField] TMP_Text textMasterVolume;

    [Header("Music Volume")]
    [SerializeField] Slider sliderMusicVolume;
    [SerializeField] TMP_Text textMusicVolume;

    [Header("SFX Volume")]
    [SerializeField] Slider sliderSfxVolume;
    [SerializeField] TMP_Text textSfxVolume;

    [Header("Ambient Volume")]
    [SerializeField] Slider sliderAmbientVolume;
    [SerializeField] TMP_Text textAmbientVolume;

    [Header("Fullscreen")]
    [SerializeField] TMP_Text textFullscreen;
    bool isFullscreen = true;

    [Header("Language")]
    [SerializeField] TMP_Text textLanguage;

    void Start()
    {
        ApplyVolume(sliderMasterVolume, "MasterVolume", textMasterVolume);
        ApplyVolume(sliderMusicVolume, "MusicVolume", textMusicVolume);
        ApplyVolume(sliderSfxVolume, "SfxVolume", textSfxVolume);
        ApplyVolume(sliderAmbientVolume, "AmbientVolume", textAmbientVolume);
        ApplyFullscreen();
    }

    void ApplyVolume(Slider slider, string volumeName, TMP_Text info )
    {
        float db = Mathf.Log10(slider.value) * 20f;
        mixer.SetFloat(volumeName, db);
        info.text = Mathf.RoundToInt(slider.value * 100 ) + " %";
    }

    void ApplyFullscreen()
    {
        Screen.fullScreen = isFullscreen;

        if (isFullscreen)
        {
            textFullscreen.text = "On";
        }
        else
        {
            textFullscreen.text = "Off";
        }
    }
    void Update()
    {
        int h = BattleInput.HorizontalHeld();
        if (h == 0)
            return;

        if (settingsNavigator.index == 0) // Master Volume
        {
            sliderMasterVolume.value += h * 0.5f * Time.deltaTime;
            ApplyVolume(sliderMasterVolume, "MasterVolume", textMasterVolume);
        }

        if (settingsNavigator.index == 1) // Music Volume
        {
            sliderMusicVolume.value += h * 0.5f * Time.deltaTime;
            ApplyVolume(sliderMusicVolume, "MusicVolume", textMusicVolume);
        }

        if (settingsNavigator.index == 2) // SFX Volume
        {
            sliderSfxVolume.value += h * 0.5f * Time.deltaTime;
            ApplyVolume(sliderSfxVolume, "SFXVolume", textSfxVolume);
        }

        if (settingsNavigator.index == 3) // Ambient Volume
        {
            sliderAmbientVolume.value += h * 0.5f * Time.deltaTime;
            ApplyVolume(sliderAmbientVolume, "AmbientVolume", textAmbientVolume);
        }

        if (settingsNavigator.index == 4) // Fullscreen
        {
            if (BattleInput.Horizontal() != 0)
            {
                isFullscreen = !isFullscreen;
                ApplyFullscreen();
            }
        }

        if (settingsNavigator.index == 5) // Language
        {
            //Null
        }
    }
}
