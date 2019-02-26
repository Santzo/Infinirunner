using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Image img;
    private Image border;
    private TextMeshProUGUI text;
    private Color oriColor;
    private Color boriColor;
    private bool pointerDown;
    private Color toriColor;

    
    // Start is called before the first frame update
    void Awake()
    {
        img = GetComponent<Image>();

    }
    void Start()
    {

        border = transform.GetChild(1).gameObject.GetComponent<Image>();
        boriColor = border.color;
        text = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        toriColor = text.color;
        oriColor = img.color;
        if (this.name == "AudioMute") transform.GetChild(2).transform.GetChild(1).GetComponent<Image>().enabled = AudioManager.am._audio.mute;
        if (this.name == "Reverse")
        {
            transform.GetChild(2).transform.GetChild(1).GetComponent<Image>().enabled = Convert.ToBoolean(PlayerPrefs.GetInt("Reverse", 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
       

    }

    public void OnPointerDown (PointerEventData eventData)
    {
        AudioManager.am._audio.PlayOneShot(AudioManager.am._click, 0.7f);
        img.color = new Color(oriColor.r - 0.5f, oriColor.g - 0.5f, oriColor.b - 0.5f, oriColor.a);
        border.color = new Color(boriColor.r - 0.5f, boriColor.g - 0.5f, boriColor.b - 0.5f, boriColor.a);
        text.color = new Color(toriColor.r - 0.5f, toriColor.g - 0.5f, toriColor.b - 0.5f, toriColor.a);
        pointerDown = true;
    }

    public void OnPointerUp (PointerEventData eventData)
    {
    
        img.color = oriColor;
        border.color = boriColor;
        text.color = toriColor;

        if (pointerDown)
        {
            if (this.name == "NewGame" && !SaveDataManager.sdm.pbError)
            {
                SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
            }
            if (this.name == "OkButton" && SaveDataManager.sdm.pbError) SaveDataManager.sdm.pbError = false;
            if (this.name == "QuitGame" && !SaveDataManager.sdm.pbError) Application.Quit();
            if (this.name == "HowToPlay" && !SaveDataManager.sdm.pbError || this.name == "OkPanelButton" && !SaveDataManager.sdm.pbError)
            {
                GameObject.Find("UI").GetComponent<UIMainMenuScript>().HowToPanel();
            }
            if (this.name == "MainMenu") SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            if (this.name == "TryAgain" && !SaveDataManager.sdm.pbError) GameManager.gm.GameRestart();

            if (this.name == "AudioMute" && !SaveDataManager.sdm.pbError)
            {
                AudioManager.am._audio.mute = !AudioManager.am._audio.mute;
                AudioManager.am._forceField.mute = !AudioManager.am._forceField.mute;
                AudioManager.am._ufo.mute = !AudioManager.am._ufo.mute;
                AudioManager.am._countdown.mute = !AudioManager.am._countdown.mute;
                transform.GetChild(2).transform.GetChild(1).GetComponent<Image>().enabled = AudioManager.am._audio.mute;
                int mute = AudioManager.am._audio.mute ? 1 : 0;
                PlayerPrefs.SetInt("Mute", mute);
            }

            if (this.name == "ButtonSettings" && !SaveDataManager.sdm.pbError)
            {
                SceneManager.LoadScene("ButtonSettings", LoadSceneMode.Single);
            }
            if (name =="Done" && !SaveDataManager.sdm.pbError)
            {
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            }
        }
    }


}
