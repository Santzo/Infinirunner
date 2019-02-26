using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Net;
using UnityEngine.Networking;

public class SaveDataManager : MonoBehaviour
{

    [Serializable]
    public class Data
    {
        public string name;
        public float score;
        public string data;
    }

    [Serializable]
    public class HighScores
    {
        public string name;
        public float score;
        public string data;
    }
    [Serializable]
    public class Scores
    {
        public List<HighScores> _score;
    }

    public static SaveDataManager sdm;

   

    public Vector2 runPos;
    public Vector2 jumPos;
    public Vector2 healPos;
    public Vector2 boostPos;


    public float yMulti;
    public float xMulti;

    public Scores highScores;
    public HighScores bonus;
    public Data data = new Data();
    public Data previousData = new Data();
    public bool saveDataExists;
    public bool dataLoaded;
    public bool scoreNotUpdated;
    public bool error;
    public bool pbError;
    public string jsonText;
    public string pbFile;
    string SetURL = ""; // Upload URL goes here
    string getURL = ""; // Download URL goes here


    private void Awake()
    {
        if (sdm == null)
        {
            sdm = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
        xMulti = Screen.width / 1000f;
        yMulti = Screen.height / 1000f;
    }

    void Start()
    {
        pbFile = Application.persistentDataPath + "local.dat";
        //ResetHighScores();

        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call<bool>("moveTaskToBack", true);
        }

    }
    public void LoadScore()
    {
        string loadData;
        if (File.Exists(pbFile))
        {
            saveDataExists = true;
            loadData = File.ReadAllText(pbFile);
            try
            {
                data = JsonUtility.FromJson<Data>(loadData);
            }
            catch
            {
                pbError = true;
            }

            if (DeCrypt(data.data) != data.score.ToString())
            {
                pbError = true;
            }
        }

    }


    public void SaveScore(string name, float score)
    {

        data.name = name;
        data.score = score;
        data.data = Crypt(score);

        string json = JsonUtility.ToJson(data);

        if (!File.Exists(pbFile))
        {
            File.Create(pbFile).Dispose();

        }
        File.WriteAllText(pbFile, json);
      
        
    }

    public void ResetHighScores()
    {

            highScores = new Scores();
            highScores._score = new List<HighScores>();

            for (int i = 0; i < 60; i++)
            {
                bonus = new HighScores();
                bonus.name = "Empty";
                bonus.score = 60 - (i);
                bonus.data = "abc123";
                highScores._score.Add(bonus);

            }

            

            string json = JsonUtility.ToJson(highScores);
            StartCoroutine(Upload(json));
         
    
    }

    public string Crypt(float aInput, bool crypt = true)
    {

            aInput =  Mathf.Round(aInput);
            string input = aInput.ToString();
            string mix = "QDXkW<_(V?cqK.lJ>-*y&zv9rpf8biYCFeMxBm6ZnG3H4uOS1UaI5TwtoA#Rs!,7d2@L^gNjh)EP$0";
            char[] result = (input ?? "").ToCharArray();
            for (int i = 0; i < result.Length; i++)
            {
                int j = mix.IndexOf(result[i]);
                result[i] = (j < 0) ? result[i] : mix[(j + 39) % 78];
            }
            return new string(result);

    }

    public string DeCrypt(string input, bool crypt = true)
    {

        var mix = "QDXkW<_(V?cqK.lJ>-*y&zv9rpf8biYCFeMxBm6ZnG3H4uOS1UaI5TwtoA#Rs!,7d2@L^gNjh)EP$0";
        var result = (input ?? "").ToCharArray();
        for (int i = 0; i < result.Length; i++)
        {
            int j = mix.IndexOf(result[i]);
            result[i] = (j < 0) ? result[i] : mix[(j + 39) % 78];
        }
        float number;
        bool getNum = float.TryParse(new string (result), out number);
        if (getNum)
        {
            float aInput;
            aInput = Mathf.Round(number);

            string resultA = aInput.ToString();
            return resultA;
        }
        return null;
    }

    public void UpdateData(Scores scoreLoad)
    {
        string json = JsonUtility.ToJson(scoreLoad);
        StartCoroutine(Upload(json));
    }

   

    IEnumerator Upload(string cont)
    {
        string URL = SetURL + cont;
        UnityWebRequest www = new UnityWebRequest(URL);
        www.SetRequestHeader("apikey", "ApiKeyValueInHere");
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            error = true;
        }


    }

    IEnumerator Download()
    {
        string URL = getURL;
        UnityWebRequest www = UnityWebRequest.Get(URL);
        www.SetRequestHeader("apikey", "ApiKeyValueInHere");
        www.downloadHandler = new DownloadHandlerBuffer();
        www.useHttpContinue = false;
        www.timeout = 10;
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            error = true;
        }

        jsonText = www.downloadHandler.text;
        highScores = JsonUtility.FromJson<Scores>(jsonText);
        dataLoaded = true;
        
    }

    public void DestroySave()
    {
        if (File.Exists(pbFile)) File.Delete(pbFile);

    }

}

