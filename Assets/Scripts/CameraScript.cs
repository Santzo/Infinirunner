using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        Screen.fullScreen = true;
        player = GameObject.FindGameObjectWithTag("Player");
        offset = player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(player.transform.position.x - offset.x, transform.position.y, transform.position.z), ref velocity, 0.05f);
        if (!PlayerStats.dead) transform.position = new Vector3(player.transform.position.x - offset.x, transform.position.y, transform.position.z);
    }
}
