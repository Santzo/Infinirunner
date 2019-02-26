using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesScript : MonoBehaviour, ISpawn
{
    public int size;
    public bool noSpawn;
    private GameObject player;
    private HealthBarScript hbs;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        hbs = GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<HealthBarScript>();
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x + GameManager.gm.wsW < player.transform.position.x)
        {
            noSpawn = false;
            gameObject.SetActive(false);
        }
    }

    public void Spawn()
    {
        size = Random.Range(1, 3);
        if (!noSpawn && size > 1)
        {
            GameObject _spawn = ObjectPooler.op.Spawn("Spikes", new Vector2(transform.position.x + 0.5f, transform.position.y), null, null, true);
        }

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("PlayerHitbox") && !PlayerStats.invincible && PlayerStats.boost <= 0f)
        {
            hbs.TakeDamage(1);
            ObjectPooler.op.Spawn("BloodSplatter", collision.GetContact(0).point);
        }
    }
}
