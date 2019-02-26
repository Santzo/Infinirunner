using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UIScript : MonoBehaviour, IPointerClickHandler
{
    private GameObject player;
    private GameObject countdown;
    private Touch touch;
    private HealthButtonScript hbs;
    private GameObject runButton;
    private GameObject jumpButton;
    public static bool cDown;
  
    // Start is called before the first frame update
    void Start()
    {
        countdown = GameObject.FindGameObjectWithTag("Countdown");
        hbs = GameObject.FindGameObjectWithTag("HealthButton").GetComponent<HealthButtonScript>();
        player = GameObject.FindGameObjectWithTag("Player");
        
        runButton = GameObject.FindGameObjectWithTag("RunButton");
        jumpButton = GameObject.FindGameObjectWithTag("JumpButton");
        Vector2 temp = runButton.transform.position;
        int a = PlayerPrefs.GetInt("Reverse", 0);
        if (a == 1)
        {
            runButton.transform.position = jumpButton.transform.position;
            jumpButton.transform.position = temp;
        }
      
        StartCoroutine(CountDown());
    }




    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerEnter.tag == "HealthButton" && PlayerStats.healing > 0)
        {
            hbs.UseHealth();
        }

        if (eventData.pointerEnter.tag == "UI" || eventData.pointerEnter.tag == "SpeedText" || eventData.pointerEnter.tag == "PointsText")
        {
            if (Time.timeScale > 0f) player.GetComponent<PlayerScript>().Shoot();
            
        }
    }
    
    IEnumerator CountDown()
    {
        AudioManager.am._countdown.Play();
        Time.timeScale = 0f;
        float pause = Time.realtimeSinceStartup + 1.2f;

        countdown.GetComponent<TextMeshProUGUI>().text = "Ready!";
        while (Time.realtimeSinceStartup < pause)
        yield return null;
        pause += 1.25f;
        countdown.GetComponent<TextMeshProUGUI>().text = "Set!";
        while (Time.realtimeSinceStartup < pause)
        yield return null;
        countdown.GetComponent<TextMeshProUGUI>().text = "Go!";
        Time.timeScale = 1f;
        pause += .9f;
        while (Time.realtimeSinceStartup < pause)
        yield return null;
        countdown.SetActive(false);
        player.GetComponent<PlayerScript>().ResetStats();
    }
}
