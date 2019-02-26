using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour, ISpawn
{
    private GameObject player;
    public SpriteRenderer sr;
    private float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
     
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        if (transform.position.x + 7.5f < player.transform.position.x) gameObject.SetActive(false);
    }
    public void Spawn()
    {
        moveSpeed = Random.Range(-2f, 2f);
        float color = Random.Range(0.6f, 1.0f);
        float alpha = Random.Range(0.01f, 0.6f);
        float sizeX = Random.Range(0.1f, 0.5f);
        float sizeY = Random.Range(0.1f, 0.5f);
        transform.localScale = new Vector3(sizeX, sizeY, sizeX);
        sr.color = new Color(color, color, color, alpha);
    }
}
