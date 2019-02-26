using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public GameObject healthBar;
    private HealthButtonScript healthButton;
    private GameObject player;
    private GameObject[] activeHealth;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        healthButton = GameObject.FindGameObjectWithTag("HealthButton").GetComponent<HealthButtonScript>();
        activeHealth = new GameObject[6];
        for (int i = 1; i < 6; i++)
        {
            activeHealth[i] = Instantiate(healthBar);
            activeHealth[i].transform.SetParent(transform);
            RectTransform rT = activeHealth[i].GetComponent<RectTransform>();
            rT.localPosition = new Vector2((i - 1) * 105f, 0f);
            

            activeHealth[i].transform.localScale = new Vector3(8f,6f,1f);
            if (i > PlayerStats.health) activeHealth[i].GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 0.3f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int dmg, int invlenght = 15)
    {

        if (!PlayerStats.invincible && dmg > 0 || dmg < 0)
        {
            PlayerStats.health -= dmg;
            if (dmg < 0) AudioManager.am._audio.PlayOneShot(AudioManager.am._ohYeah, 0.4f);

            if (PlayerStats.health <= 0)
            {

                int a = Random.Range(1, 3);
                if (a <= 1) AudioManager.am._audio.PlayOneShot(AudioManager.am._manScream1);
                else AudioManager.am._audio.PlayOneShot(AudioManager.am._manScream2);
                GameManager.gm.Die();

            }
            if (dmg > 0)
            {
                int a = Random.Range(1, 3);
                if (a <= 1) AudioManager.am._audio.PlayOneShot(AudioManager.am._grunt1);
                else AudioManager.am._audio.PlayOneShot(AudioManager.am._grunt1);
            }

            if (PlayerStats.health >= PlayerStats.maxHealth) PlayerStats.health = PlayerStats.maxHealth;
            for (int i = 1; i < 6; i++)
            {
                if (i > PlayerStats.health) activeHealth[i].GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 0.3f);
                else activeHealth[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            }
            if (invlenght > 0) player.GetComponent<PlayerScript>().GetHit(invlenght);
        }
    }


}
