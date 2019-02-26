using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipParticlesScript : MonoBehaviour, ISpawn
{
    private ParticleSystem ps;
    private GameObject player;
    private Vector3 scale;
    // Start is called before the first frame update
    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        scale = transform.localScale;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        if (transform.position.x + 3.5f <= player.transform.position.x) gameObject.transform.SetParent(null);
    }
    // Update is called once per frame
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.7f);
        gameObject.transform.SetParent(null);
        yield return new WaitForSeconds(0.6f);
        if (transform.localScale != scale) transform.localScale = scale;
        gameObject.SetActive(false);

    }
    public void Spawn()
    {
        ps.Clear();
        ps.Play();
        StartCoroutine("Destroy");

    }
}
