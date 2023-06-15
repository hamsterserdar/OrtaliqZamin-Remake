using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class UİSettingsManager : MonoBehaviour
{
    [Header("Graphics")]
    public TMP_Dropdown GraphicsDropdown;
    [Header("PauseMenu")]
    public GameObject PauseMenu;
    [HideInInspector]public bool OpenPauseMenu,PlayerDie = false;
    [Header("Resulotion")]
    public TMP_Dropdown resulotionDropdown;
    private Resolution[] resolutions;
    public List<Resolution> filteredresolutiouns;
    int currentResulotionIndex = 0;
    [Header("Audio")]
    public AudioMixer audioMixer;
    public Slider MasterSlider,MusicSlider,SfxSlider;
    public TMP_Text MasterVolText,MusicVolText,SfxVolText;
    [Header("Physics")]
    public GameObject Player;
    public Slider Gravity;
    public Slider Suret,TullanmaGucu;
    public TMP_Text GravityText,SuretText,TullanmaGucuText;
    void Awake()
    {
        MusicSaveCheck();
        Resulotion();
        GraphicsCheck();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !GameManager.Instance.PlayerDeath)
        {
            PauseMenuOpenOff();
        }
        TullanmaGucuText.text = ((int)TullanmaGucu.value).ToString();
        GravityText.text = ((int)Gravity.value).ToString();
        SuretText.text = ((int)Suret.value).ToString();
    }
    public void SetMasterVol()
    {
        MasterVolText.text = ((int)(MasterSlider.value * 100)).ToString();
        audioMixer.SetFloat("Master",Mathf.Log10(MasterSlider.value)*20);
        PlayerPrefs.SetFloat("MasterVol",MasterSlider.value);
    }
    public void SetMusicVol()
    {
        MusicVolText.text = ((int)(MusicSlider.value*100)).ToString();
        audioMixer.SetFloat("Music",Mathf.Log10(MusicSlider.value)*20);
        PlayerPrefs.SetFloat("MusicVol",MusicSlider.value);
    }
    public void SetResulotion(int resulotionIndex)
    {
        Resolution resulotion = filteredresolutiouns[resulotionIndex];
        Screen.SetResolution(resulotion.width,resulotion.height,true);
    }
    public void SetSfxVol()
    {
        SfxVolText.text = ((int)(SfxSlider.value * 100)).ToString();
        audioMixer.SetFloat("Sfx",Mathf.Log10(SfxSlider.value)*20);
        PlayerPrefs.SetFloat("SfxVol",SfxSlider.value);
    }
    void MusicSaveCheck()
    {
        if(PlayerPrefs.HasKey("MasterVol"))
        {
            audioMixer.SetFloat("Master",PlayerPrefs.GetFloat("MasterVol"));
            Debug.Log("master vol var");
        }
        if(PlayerPrefs.HasKey("MusicVol"))
        {
            audioMixer.SetFloat("Music",PlayerPrefs.GetFloat("MusicVol"));
        }
        if(PlayerPrefs.HasKey("SfxVol"))
        {
            audioMixer.SetFloat("Sfx",PlayerPrefs.GetFloat("SfxVol"));
        }
        float vol = 0f;
        audioMixer.GetFloat("Master",out vol);
        MasterSlider.value = vol;
        audioMixer.GetFloat("Music",out vol);
        MusicSlider.value = vol;
        audioMixer.GetFloat("Sfx",out vol);
        SfxSlider.value = vol;
        MasterVolText.text = ((int)(MasterSlider.value * 100)).ToString();
        MusicVolText.text = ((int)(MusicSlider.value*100)).ToString();
        SfxVolText.text = ((int)(SfxSlider.value * 100)).ToString();
    }
    void Resulotion()
    {
        resolutions = Screen.resolutions;
        filteredresolutiouns = new List<Resolution>();
        resulotionDropdown.ClearOptions();
        for(int i = 0; i < resolutions.Length;i++)
        {
            filteredresolutiouns.Add(resolutions[i]);
        }
        List<string> options = new List<string>();
        for(int i = 0; i < filteredresolutiouns.Count;i++)
        {
            string resulotionOption = filteredresolutiouns[i].width + "x" + filteredresolutiouns[i].height + " ";
            options.Add(resulotionOption);
            if(filteredresolutiouns[i].width == Screen.width && filteredresolutiouns[i].height == Screen.height)
            {
                currentResulotionIndex = i;
            }
        }
        resulotionDropdown.AddOptions(options);
        resulotionDropdown.value = currentResulotionIndex;
        resulotionDropdown.RefreshShownValue();
    }
    void GraphicsCheck()
    {
        if(PlayerPrefs.HasKey("GraphicsQuality"))
        {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("GraphicsQuality"));
            GraphicsDropdown.value = PlayerPrefs.GetInt("GraphicsQuality");
        }
    }
    public void PauseMenuOpenOff()
    {
        OpenPauseMenu = !OpenPauseMenu;
        PauseMenu.SetActive(OpenPauseMenu);
    }
    public void SetGraphicsQuality(int value)
    {
        QualitySettings.SetQualityLevel(value);
        PlayerPrefs.SetInt("GraphicsQuality",value);
    }
    public void ButtonSound()
    {
        AudioManager.Instance.PlaySfx("UIButton");
    }
    public void PhysicsApply()
    {
        Player.GetComponent<CharacterController2D>().rigidgravity = Gravity.value;
        Player.GetComponent<CharacterController2D>().speed = Suret.value;
        Player.GetComponent<CharacterController2D>().jumpingPower = TullanmaGucu.value;
    }
}
