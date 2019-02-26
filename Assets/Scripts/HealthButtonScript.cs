using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HealthButtonScript : MonoBehaviour
{
    private Image img;
    private TextMeshProUGUI txt;
    private HealthBarScript hbs;
    // Start is called before the first frame update
    void Start()
    {
        hbs = GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<HealthBarScript>();
        img = GetComponent<Image>();
        txt = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
 
            float x = PlayerPrefs.GetFloat("HealthX", 90f * SaveDataManager.sdm.xMulti);
            float y = PlayerPrefs.GetFloat("HealthY", 400f * SaveDataManager.sdm.yMulti);
            transform.position = new Vector2(x, y);
  
    }

    public void UpdateHealth()
    {
        txt.text = PlayerStats.healing.ToString();
        if (PlayerStats.healing <= 0)
        {
            img.color = new Color(0.4f, 0.4f, 0.4f, 0.4f);
            txt.color = new Color(0.4f, 0.4f, 0.4f, 0.4f);
        }
        else
        {
            img.color = new Color(1f, 1f, 1f, 1f);
            txt.color = new Color(1f, 1f, 1f, 1f);
        }
    }
    public void UseHealth()
    {
        if (PlayerStats.healing > 0 && PlayerStats.health < PlayerStats.maxHealth)
        {
            hbs.TakeDamage(-1, 0);
            PlayerStats.healing--;
            UpdateHealth();
        }
    }


}
