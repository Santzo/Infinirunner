using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienScript : MonoBehaviour, ISpawn, IEnemyDamage
{

    private EnemyStats stats;
    private Rigidbody2D rb;
    private GameObject player;
    private GameObject plr;
    private bool playerDetected;
    private float timeToShoot;
    private bool dead;
    private Gradient gradient;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    // Start is called before the first frame update


    void Start()
    {
        plr = GameObject.FindGameObjectWithTag("Player");
        player = GameObject.FindGameObjectWithTag("PlayerCenter");
        gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(new Color(0.7f,0.7f,0.7f), 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
        Spawn();
    }

    void FixedUpdate()
    {
 
        if (!playerDetected)
        {
            rb.MovePosition(rb.position + Vector2.left * stats.moveSpeed * Time.fixedDeltaTime);
        }
        if (!playerDetected && player.transform.position.x + GameManager.gm.wsW / 1.5f > transform.position.x)
        {
            playerDetected = true;
            timeToShoot = stats.droppingFreq - 0.5f;
        }
        if (playerDetected && player.transform.position.x + GameManager.gm.wsW / 1.45f < transform.position.x)
        {
            playerDetected = false;
        }
        if (playerDetected && !PlayerStats.dead)
        {
            timeToShoot += Time.deltaTime;
            if (timeToShoot > stats.droppingFreq)
            {
                AudioManager.am._audio.PlayOneShot(AudioManager.am._laserClip, 0.5f);
                timeToShoot = 0f;
                GameObject _bullet = ObjectPooler.op.Spawn("AlienAmmo", transform.position, Quaternion.identity);
                _bullet.transform.right = player.transform.position - transform.position;
                _bullet.GetComponent<BulletScript>().moveSpeed = 62.5f;

                _bullet.GetComponent<TrailRenderer>().colorGradient = gradient;
            }
        }


    }
    // Update is called once per frame
    void Update()
    {
        
        if (transform.position.x + 7.5f < player.transform.position.x) gameObject.SetActive(false);
        if (PlayerStats.dead && AudioManager.am._ufo.isPlaying) AudioManager.am._ufo.Stop();
        if (!PlayerStats.dead)
        {
            if (transform.position.x - GameManager.gm.wsW < player.transform.position.x && !AudioManager.am._ufo.isPlaying) AudioManager.am._ufo.Play();
            else if (transform.position.x - GameManager.gm.wsW > player.transform.position.x && AudioManager.am._ufo.isPlaying || PlayerStats.dead) AudioManager.am._ufo.Stop();
        }
    }

    public void Spawn()
    {

        stats = new EnemyStats(Random.Range(4f, 6f), 7, 0, Random.Range(0.3f, 0.5f), 250);
        timeToShoot = 0f;
        dead = false;
        playerDetected = false;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.layer = LayerMask.NameToLayer("Enemy");
        }

    }
    public void EnemyDamage(float dmg)
    {
        stats.health -= dmg;
        float multiplier = 1f;
        if (plr.GetComponent<Rigidbody2D>().velocity.x > 0f) multiplier = plr.GetComponent<Rigidbody2D>().velocity.x * (plr.GetComponent<Rigidbody2D>().velocity.x / 2.75f);

        if (multiplier > 1f) stats.points += multiplier * 625f;
        else stats.points += 625f;
        if (stats.health <= 0 && !dead)
        {
            AudioManager.am._audio.PlayOneShot(AudioManager.am._explosion, 0.5f);
            AudioManager.am._ufo.Stop();
            GameManager.gm.AddPoints(stats.points, transform.position);

            gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
            foreach (Transform child in GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
            }
            dead = true;
            ObjectPooler.op.Spawn("Explosion", transform.position);
            gameObject.SetActive(false);
        }
    }
}
