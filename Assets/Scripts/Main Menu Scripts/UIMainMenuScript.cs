using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMainMenuScript : MonoBehaviour
{
    public GameObject screenTap;
    public GameObject titleText;
    public Canvas canvas;
    private bool menuActive;
    private bool subMenuActive;
    private float titleWidth;
    private Vector3 target;

    private float xMultiplier;
    private float yMultiplier;

    private bool loadData;
    private bool loadAd;
    private bool howToActive;
    private Vector3 panelOri;
    private Vector3 panelDest;
    private GameObject newGame;
    private GameObject howToPlayPanel;
    private GameObject howToPlay;
    private GameObject highScores;
    private GameObject copyright;
    private GameObject quitGame;
    private GameObject pb;
    private GameObject pbError;
    private bool dataDeleted;

  

    private void Awake()
    {
        
        loadData = false;
        xMultiplier = Screen.width / 1000f;
        yMultiplier = Screen.height / 1000f;

    }
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("UI").GetComponent<Canvas>();
        target = new Vector3(80f * xMultiplier, Screen.height - 80f * canvas.scaleFactor);
        newGame = GameObject.Find("NewGame");
        highScores = GameObject.Find("HighScores");
        pb = GameObject.Find("PB");
        pbError = GameObject.Find("PBError");
        pbError.SetActive(false);
        howToPlay = GameObject.Find("HowToPlay");
        copyright = GameObject.Find("Copyright");
        howToPlayPanel = GameObject.Find("HowToPlayPanel");
        quitGame = GameObject.Find("QuitGame");

        newGame.transform.position = new Vector2(-Screen.width / 2f, Screen.height - (235f * canvas.scaleFactor));
        howToPlay.transform.position = new Vector2(newGame.transform.position.x, newGame.transform.position.y - (120f * canvas.scaleFactor));
        quitGame.transform.position = new Vector2(newGame.transform.position.x, newGame.transform.position.y - (240f * canvas.scaleFactor));
        pb.transform.position = new Vector2(newGame.transform.position.x, newGame.transform.position.y - (360f * canvas.scaleFactor));
        copyright.transform.position = new Vector2(newGame.transform.position.x, pb.transform.position.y - 250f * canvas.scaleFactor);
        highScores.transform.position = new Vector2(Screen.width * 2f, highScores.transform.position.y);
        howToPlayPanel.transform.position = new Vector2(40f * canvas.scaleFactor, Screen.height * 2f);
        panelOri = howToPlayPanel.transform.position;
        panelDest = new Vector2(howToPlayPanel.transform.position.x, Screen.height - 40f * canvas.scaleFactor);

        titleWidth = titleText.GetComponent<RectTransform>().rect.size.x;
        

    }

    // Update is called once per frame
    void Update()
    {
      
        if (!loadData)
        {
            SaveDataManager.sdm.dataLoaded = false;
            //SaveDataManager.sdm.ResetHighScores();
            SaveDataManager.sdm.LoadScore();
            loadData = true;
        }
        if (!screenTap.activeSelf && !menuActive)
        {
            titleText.transform.position = Vector2.MoveTowards(titleText.transform.position, target, 2000f * Time.deltaTime);
            float checkX = Mathf.Abs(titleText.transform.position.x) - Mathf.Abs(target.x);
            if (checkX < 2f)
            {
                subMenuActive = true;
                menuActive = false;
                if (SaveDataManager.sdm.saveDataExists && SaveDataManager.sdm.data.score > 0)
                {
                    pb.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Personal Best<color=white> \n";
                    pb.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = SaveDataManager.sdm.data.name + "\n" + SaveDataManager.sdm.data.score;
                }
                else
                {
                    pb.GetComponentInChildren<TextMeshProUGUI>().text = "No personal best found.";
                    pb.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
            }

        }

        if (subMenuActive)
        {
            
            if (!SaveDataManager.sdm.pbError && pbError.activeSelf) pbError.SetActive(false);
            if (SaveDataManager.sdm.pbError)
            {
                if (!dataDeleted)
                {
                    pbError.SetActive(true);
                    SaveDataManager.sdm.DestroySave();
                    SaveDataManager.sdm.SaveScore("Nobody", 0f);
                    dataDeleted = true;
                }
                
            }
            newGame.transform.position = Vector2.MoveTowards(newGame.transform.position, new Vector2(titleText.transform.position.x + 220f * canvas.scaleFactor, newGame.transform.position.y), 3000f * canvas.scaleFactor * Time.deltaTime);
            howToPlay.transform.position = Vector2.MoveTowards(howToPlay.transform.position, new Vector2(titleText.transform.position.x + 220f * canvas.scaleFactor, howToPlay.transform.position.y), 2900f * canvas.scaleFactor * Time.deltaTime);
            quitGame.transform.position = Vector2.MoveTowards(quitGame.transform.position, new Vector2(titleText.transform.position.x + 220f * canvas.scaleFactor, quitGame.transform.position.y), 2700f * canvas.scaleFactor * Time.deltaTime);
            pb.transform.position = Vector2.MoveTowards(pb.transform.position, new Vector2(titleText.transform.position.x + 220f * canvas.scaleFactor, pb.transform.position.y), 2600f * canvas.scaleFactor * Time.deltaTime);
            copyright.transform.position = Vector2.MoveTowards(copyright.transform.position, new Vector2(titleText.transform.position.x + 220f * canvas.scaleFactor, copyright.transform.position.y), 2500f * canvas.scaleFactor * Time.deltaTime);
            highScores.transform.position = Vector2.MoveTowards(highScores.transform.position, new Vector2(titleText.transform.position.x + 1020f * canvas.scaleFactor, highScores.transform.position.y), 3000f * canvas.scaleFactor * Time.deltaTime);

        }

            if (howToActive && howToPlayPanel.transform.position != panelDest) howToPlayPanel.transform.position = Vector2.MoveTowards(howToPlayPanel.transform.position, panelDest, 3000f * canvas.scaleFactor * Time.deltaTime);
            if (!howToActive && howToPlayPanel.transform.position != panelOri) howToPlayPanel.transform.position = Vector2.MoveTowards(howToPlayPanel.transform.position, panelOri, 3000f * canvas.scaleFactor * Time.deltaTime);
    }

    public void HowToPanel()
    {
        howToActive = !howToActive;
    }


}
