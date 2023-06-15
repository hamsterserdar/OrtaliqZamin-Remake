using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [HideInInspector]public bool PlayerDeath = false;
    public Image HealthBar;
    public GameObject DeadPanel;
    public TMP_Text OlmeSebebi;
    public float Qepik;
    void Start()
    {
        Instance = this;
        AudioManager.Instance.PlayMusic("MainTheme");
        if(PlayerPrefs.HasKey("Qepik"))
        {
            Qepik = PlayerPrefs.GetFloat("Qepik");
        }
    }
    void Update()
    {
        
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void HealthBarConfigruation(float Health)
    {
        HealthBar.fillAmount = Health/100;
    }
    public void OlmeSebebiTextConfigure(string Yazi)
    {
        OlmeSebebi.text = Yazi;
    }
    public void QepikRefresh()
    {
        Qepik++;
        PlayerPrefs.SetFloat("Qepik",Qepik);
    }
}
