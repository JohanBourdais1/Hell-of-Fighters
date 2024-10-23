using System;
using System.Collections;
using System.Collections.Generic;
using Menu;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class OptionScript : MonoBehaviour
{
    private AudioSource _muqiueplayer;
    public Slider Slider;

    private void Start()
    {
        _muqiueplayer= FindObjectOfType<AudioSource>();
        Slider.value = _muqiueplayer.volume;
    }

    public void SetFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void returnmenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void SetQuality(int Qualityindex)
    {
        QualitySettings.SetQualityLevel(Qualityindex);
    }

    public void SetVolume(float vol)
    {
        _muqiueplayer.volume = vol;
        Slider.value = _muqiueplayer.volume;
    }
}
