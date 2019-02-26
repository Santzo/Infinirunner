using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject killText;
    public float totalTime;
    private GameObject player;
    public float width;
    public float ratio;
    public float wsW;
    public float wsH;
    public bool death;
    public GameObject deathScreen;
    public GameObject pointsText;
    public Vector2 timerPos;
    public Vector2 pointsPos;
    public static GameManager gm;
    private Color colori;


    private void Awake()
    {
        ratio = Screen.width / 1920f;
        wsH = Camera.main.orthographicSize * 2;
        wsW = wsH / Screen.height * Screen.width;

        if (gm == null)
        {
            gm = this;
        }
        else
        {
            Destroy(gameObject);
        }


    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        deathScreen.transform.localScale = new Vector2(0f, 0f);
        deathScreen.SetActive(false);
        pointsPos = new Vector2(-1f, 0f);
        timerPos = new Vector2(0f, 0f);
        
    }
    public void Update()
    {
        if (death)
        {
            if (deathScreen.transform.localScale.x < 1f) deathScreen.transform.localScale = new Vector2(deathScreen.transform.localScale.x + Time.deltaTime * 2.5f, deathScreen.transform.localScale.y + Time.deltaTime * 2.5f);
            if (deathScreen.transform.localScale.x > 1f) deathScreen.transform.localScale = new Vector2(1f, 1f);
        }
    }

    public void Die()
    {
        player.GetComponent<PlayerScript>().Dead();
        deathScreen.SetActive(true);
        death = true;
    }

    public void ShowText(string text, Vector2 position, string color, float speed = 3f, bool up = true)
    {
        if (position == pointsPos) position = Camera.main.ScreenToWorldPoint(pointsText.transform.position);

        GameObject _text = ObjectPooler.op.Spawn("KillText", position, null, null);

        ColorUtility.TryParseHtmlString(color, out colori);
        _text.GetComponent<TextMeshPro>().color = colori;
        _text.GetComponent<TextMeshPro>().text = text;
        _text.GetComponent<KillTextScript>().moveSpeed = speed;
        _text.GetComponent<KillTextScript>().up = up;
   
    }
    public void AddPoints(float points, Vector2 location)
    {
        float multiplier = points * (0.96f + (PlayerStats.difficulty / 100f) * 4f);
        PlayerStats.points += multiplier;
        if (points > 0) ShowText("+ " + Mathf.Round(multiplier), location, "#69FF4C");


    }

    public void GameRestart()
    {
        SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
    }
   

}
