using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour, ISpawn
{
    private ParticleSystem ps;
    private GameObject player;
    // Start is called before the first frame update

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x + 5.5f < player.transform.position.x) gameObject.SetActive(false);
    }

    public void Spawn()
    {
        ps.Clear();
    }
}
