using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    Slider musicSlider;

    [SerializeField]
    Slider sfxSlider;

    [SerializeField]
    AudioMixer mixer;

    [SerializeField]
    EventSystem eventSystem;

    [SerializeField]
    Toggle toggle;

    // Start is called before the first frame update
    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVol", 1);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVol", 1);
    }

    // Update is called once per frame
    void Update()
    {       
        mixer.SetFloat("MusicVol", Mathf.Log10(musicSlider.value) * 20);

        mixer.SetFloat("SFXVol", Mathf.Log10(sfxSlider.value) * 20);        
    }

    public void ResetHighscore()
    {
        PlayerPrefs.SetFloat("highscore", 0);
    }
    public void OnDestroy()
    {
        PlayerPrefs.SetFloat("musicVol", musicSlider.value);
        PlayerPrefs.SetFloat("sfxVol", sfxSlider.value);
        PlayerPrefs.Save();
    }

    public void Fullscreen()
    {
        if (toggle.isOn)
        {
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
        }
    }
}
