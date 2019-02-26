using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    private Material material;
    private Vector2 savedOffset;
    private PlayerScript player;
    private GameObject plr;
    private Rigidbody2D playerRB;
    private float multiplier;
    private SpriteRenderer sr;
    private bool ori;
    private bool ori2;

   

    void Start()
    {

        plr = GameObject.FindGameObjectWithTag("Player");
        playerRB = plr.GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        material = GetComponent<Renderer>().material;


        if (!ori) transform.localScale = new Vector3(GameManager.gm.wsW / sr.sprite.bounds.size.x, GameManager.gm.wsH / sr.sprite.bounds.size.y, 1);
        if (!ori) transform.position = new Vector2(transform.position.x * transform.localScale.x, transform.position.y);
        if (!ori || !ori2)
        {

            GameObject _ori = Instantiate(gameObject, new Vector2(transform.position.x + (sr.sprite.bounds.size.x * transform.localScale.x), transform.position.y), Quaternion.identity);
            if (ori) _ori.GetComponent<BackgroundScript>().ori2 = true;
            _ori.GetComponent<BackgroundScript>().ori = true;

        }

    }


    // Update is called once per frame
    void Update()
    {
        if (!PlayerStats.dead)
        {
            if (transform.position.x + (sr.sprite.bounds.size.x * 1.5f * transform.localScale.x) < plr.transform.position.x)
            {
                transform.position = new Vector2(transform.position.x + (sr.sprite.bounds.size.x * transform.localScale.x * 3), transform.position.y);
            }
            if (playerRB.velocity.x < 0f && transform.position.x - (sr.sprite.bounds.size.x * 1.5f * transform.localScale.x) > player.transform.position.x)
            {
                transform.position = new Vector2(transform.position.x - (sr.sprite.bounds.size.x * transform.localScale.x * 3), transform.position.y);
            }
        }
    }


}
