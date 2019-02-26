using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BoostButtonScript : MonoBehaviour
{
    private Image img;
    private TextMeshProUGUI txt;
    // Start is called before the first frame update
    void Awake()
    {
        img = GetComponent<Image>();
        txt = GetComponentInChildren<TextMeshProUGUI>();
    }
    void Start()
    {
 
            float x = PlayerPrefs.GetFloat("BoostX", 90f * SaveDataManager.sdm.xMulti);
            float y = PlayerPrefs.GetFloat("BoostY", 650f * SaveDataManager.sdm.yMulti);
            transform.position = new Vector2(x, y);


    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerStats.boost > 0f) txt.text = string.Format("{0:F1}", Mathf.Round(PlayerStats.boost * 10f) / 10f) + "s";
        else txt.text = "";

        if (PlayerStats.boost <= 0f && img.color.r != 0.4f)
        {
            img.color = new Color(0.4f, 0.4f, 0.4f, 0.4f);
            txt.color = new Color(0.4f, 0.4f, 0.4f, 0.4f);
        }
        if (PlayerStats.boost > 0f && img.color.r != 1f)
        {
            img.color = new Color(1f, 1f, 1f, 1f);
            txt.color = new Color(1f, 1f, 1f, 1f);
        }
    }

}
