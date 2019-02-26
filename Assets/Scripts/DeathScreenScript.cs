using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using System;

public class DeathScreenScript : MonoBehaviour
{
    private TextMeshProUGUI survBonus;
    private TextMeshProUGUI accBonus;
    private TextMeshProUGUI totalPoints;
    private TextMeshProUGUI avgText;
    public TMP_InputField input;
    private float showPoint;
    private float _totalPoints;

    public GameObject pbText;
    public static float avgSpeed;
    public static float timeSurvived;
    public static float accuracy;
    private bool textShown;
    private static float extraPoints;


    // Start is called before the first frame update
    private void Awake()
    {
        survBonus = transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        avgText = transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        accBonus = transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();
        totalPoints = transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>();

    }
    void Start()
    {
        input.onValueChanged.AddListener(delegate { NameChanged(); });
    }

    // Update is called once per frame
    void Update()
    {

            

        if (!textShown)
        {
            survBonus.text = "Time survived: <color=yellow>" + timeSurvived.ToString() + "<color=white>s";
            avgText.text = "Average speed: <color=yellow>" + avgSpeed.ToString() + "<color=white> km/h";
            accBonus.text = "Accuracy: <color=yellow>" + accuracy.ToString() + "<color=white>%";
            extraPoints = 0f;
            extraPoints += (timeSurvived * 1.5f) * (avgSpeed / 10f) * 100f;
            extraPoints += accuracy * PlayerStats.maxShots * (avgSpeed / 10f);
            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";
            PlayerStats.points = Mathf.Round(PlayerStats.points + extraPoints);
            _totalPoints = Mathf.Round(PlayerStats.points);
            totalPoints.text = "Total Points <color=green>" + PlayerStats.points.ToString("#,0", nfi);
            if (PlayerStats.points > SaveDataManager.sdm.data.score && !textShown)
            {
                AudioManager.am._audio.PlayOneShot(AudioManager.am._fanfare, 0.3f);
                if (SaveDataManager.sdm.data.name != null) input.text = SaveDataManager.sdm.data.name; 
                if (!pbText.activeSelf) pbText.SetActive(true);
                if (SaveDataManager.sdm.data.name != null) SaveDataManager.sdm.SaveScore(SaveDataManager.sdm.data.name, _totalPoints);
                else SaveDataManager.sdm.SaveScore("Noname", _totalPoints);

            }
            textShown = true;
        }
    }

    public static void UpdateScore()
    {
        if (accuracy != 0) accuracy = Mathf.Round(accuracy * 100f * 10f) / 10f;
        timeSurvived = Mathf.Round(timeSurvived * 10f) / 10f;
        avgSpeed = Mathf.Round(avgSpeed * 10f) / 10f;
      
    }
    void NameChanged()
    {
        SaveDataManager.sdm.SaveScore(input.text, _totalPoints);
    }

}
