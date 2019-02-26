using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

public class HighScoresScript : MonoBehaviour
{
    public float scaler;
    public GameObject content;
    public GameObject position;
    public GameObject retrieveData;
    public GameObject pbText;
    private float dataTime;
    private bool errorShown;
    private Vector3 pos;
    private bool pbUpdate;
    public int scorePos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!SaveDataManager.sdm.dataLoaded)
        {
            if (!SaveDataManager.sdm.error)
            {
                dataTime += Time.deltaTime;
                if (dataTime > 1f)
                {
                    retrieveData.GetComponent<TextMeshProUGUI>().text += ".";
                    if (retrieveData.GetComponent<TextMeshProUGUI>().text.Substring(retrieveData.GetComponent<TextMeshProUGUI>().text.Length - 4, 4) == "....")
                    {
                        retrieveData.GetComponent<TextMeshProUGUI>().text = retrieveData.GetComponent<TextMeshProUGUI>().text.Substring(0, retrieveData.GetComponent<TextMeshProUGUI>().text.Length - 4);
                    }
                    dataTime = 0f;

                }
            }
            
            if (SaveDataManager.sdm.error && !errorShown)
            {
                errorShown = true;
                retrieveData.GetComponent<TextMeshProUGUI>().text = "There was an error retrieving high score data from the server.";
            }
        }

        if (SaveDataManager.sdm.dataLoaded && !SaveDataManager.sdm.error && !SaveDataManager.sdm.pbError)
        {
            retrieveData.SetActive(false);
            ShowScores();
        }
    }

    public void ShowScores()
    {
        if (!SaveDataManager.sdm.pbError)
        {
            foreach (GameObject posClone in GameObject.FindGameObjectsWithTag("PositionClone"))
            {
                Destroy(posClone);
            }
            SaveDataManager.sdm.dataLoaded = false;

            scorePos = 1000;
            int i = 0;
            SaveDataManager.sdm.previousData.score = 0;

            foreach (SaveDataManager.HighScores scores in SaveDataManager.sdm.highScores._score)
            {
                if (scores.data == SystemInfo.deviceUniqueIdentifier && !pbUpdate)
                {
                    scorePos = i;
                    SaveDataManager.sdm.previousData.score = scores.score;
                    SaveDataManager.sdm.previousData.name = scores.name;
                    if (scores.name != SaveDataManager.sdm.data.name && scores.score == SaveDataManager.sdm.data.score)
                    {
                        SaveDataManager.sdm.SaveScore(scores.name, scores.score);
                    }
                    pbUpdate = true;
                }
                i++;
            }

            if (SaveDataManager.sdm.previousData.score > SaveDataManager.sdm.data.score)
            {
                SaveDataManager.sdm.SaveScore(SaveDataManager.sdm.previousData.name, SaveDataManager.sdm.previousData.score);
                pbText.GetComponent<TextMeshProUGUI>().text = SaveDataManager.sdm.data.name + " " + SaveDataManager.sdm.data.score;

            }

            if (SaveDataManager.sdm.previousData.score < SaveDataManager.sdm.data.score)
            {
                SaveDataManager.sdm.scoreNotUpdated = true;

                if (pbUpdate)
                {
                    for (int a = scorePos; a < SaveDataManager.sdm.highScores._score.Count - 1; a++)
                    {
                        SaveDataManager.sdm.highScores._score[a].name = SaveDataManager.sdm.highScores._score[a + 1].name;
                        SaveDataManager.sdm.highScores._score[a].score = SaveDataManager.sdm.highScores._score[a + 1].score;
                        SaveDataManager.sdm.highScores._score[a].data = SaveDataManager.sdm.highScores._score[a + 1].data;
                    }
                }

                for (int a = 0; a < SaveDataManager.sdm.highScores._score.Count; a++)
                {
                    if (SaveDataManager.sdm.data.score > SaveDataManager.sdm.highScores._score[a].score)
                    {
                        scorePos = a;
                        break;
                    }
                }

                if (scorePos < 1000)
                {
                    for (int a = SaveDataManager.sdm.highScores._score.Count - 1; a >= scorePos; a--)
                    {
                        if (a > scorePos)
                        {
                            SaveDataManager.sdm.highScores._score[a].name = SaveDataManager.sdm.highScores._score[a - 1].name;
                            SaveDataManager.sdm.highScores._score[a].score = SaveDataManager.sdm.highScores._score[a - 1].score;
                            SaveDataManager.sdm.highScores._score[a].data = SaveDataManager.sdm.highScores._score[a - 1].data;
                        }
                        if (a == scorePos)
                        {
                            SaveDataManager.sdm.highScores._score[a].name = SaveDataManager.sdm.data.name;
                            SaveDataManager.sdm.highScores._score[a].score = SaveDataManager.sdm.data.score;
                            SaveDataManager.sdm.highScores._score[a].data = SystemInfo.deviceUniqueIdentifier;
                        }
                    }
                }

                pbUpdate = false;
            }
            SaveDataManager.sdm.UpdateData(SaveDataManager.sdm.highScores);
        }
        int ab = 0;
        scaler = GameObject.Find("UI").GetComponent<UIMainMenuScript>().canvas.scaleFactor;
        pos = position.transform.position;
        position.SetActive(false);
        bool highlight = false;
        var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        nfi.NumberGroupSeparator = " ";
        foreach (SaveDataManager.HighScores scores in SaveDataManager.sdm.highScores._score)
        {
            
            GameObject _position = Instantiate(position);
            _position.transform.SetParent(content.transform);
            _position.transform.localScale = new Vector3(1f, 1f, 1f);
            _position.transform.position = new Vector2(pos.x, pos.y - (ab * 50f * scaler));

            _position.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = ab + 1 + ".";
            _position.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = scores.name;
            _position.transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = scores.score.ToString("#,0", nfi);
            _position.transform.SetParent(GameObject.Find("UI").transform);


            content.GetComponent<RectTransform>().sizeDelta = new Vector2(content.GetComponent<RectTransform>().sizeDelta.x, content.GetComponent<RectTransform>().sizeDelta.y + 50f);
            _position.SetActive(true);
            if (scores.data == SystemInfo.deviceUniqueIdentifier && !highlight)
            {
                highlight = true;
                _position.gameObject.GetComponent<Image>().color = new Color(0.3f, 0.4f, 0.95f, 0.31f);
            }
            ab++;
           
        }

        foreach (GameObject _pos in GameObject.FindGameObjectsWithTag("Position"))
        {
            _pos.transform.SetParent(content.transform);
        }
    }

   
    }

