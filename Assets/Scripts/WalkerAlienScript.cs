using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerAlienScript : MonoBehaviour, IEnemyDamage, ISpawn
{
    private EnemyStats stats;
    private Rigidbody2D rb;
    private GameObject player;
    private Animator anim;
    private GameObject plr;
    private bool playerDetected;
    private float timeToShoot;
    private bool dead;
    private Gradient gradient;
    public Transform gunHead;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }
    // Start is called before the first frame update

        
    void Start()
    {
        plr = GameObject.FindGameObjectWithTag("Player");
        player = GameObject.FindGameObjectWithTag("PlayerCenter");
        gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.white, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
        Spawn();
    }

    void FixedUpdate()
    {
        if (player.transform.position.x - 0.5f > transform.position.x && transform.localScale.x > 0f) transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        if (player.transform.position.x - 0.5f <= transform.position.x && transform.localScale.x < 0f) transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        if (!playerDetected)
        {
            rb.MovePosition(rb.position + Vector2.left * stats.moveSpeed * Time.fixedDeltaTime);
        }
        if (!playerDetected && player.transform.position.x + GameManager.gm.wsW / 1.5f > transform.position.x)
        {
            playerDetected = true;
            timeToShoot = stats.droppingFreq - 0.5f;
            anim.SetTrigger("Shoot");
        }
        if (playerDetected && player.transform.position.x + GameManager.gm.wsW / 1.45f < transform.position.x)
        {
            playerDetected = false;
            anim.SetTrigger("Walk");
        }
        if (playerDetected)
        {
            timeToShoot += Time.deltaTime;
            if (timeToShoot > stats.droppingFreq)
            {
                AudioManager.am._audio.PlayOneShot(AudioManager.am._laserClip);
                anim.SetTrigger("Shot");
                timeToShoot = 0f;
                GameObject _bullet = ObjectPooler.op.Spawn("AlienAmmo", gunHead.position, Quaternion.identity);
                _bullet.transform.right = new Vector2(player.transform.position.x - gunHead.position.x, player.transform.position.y - gunHead.transform.position.y);
                _bullet.GetComponent<BulletScript>().moveSpeed = 8f;
                _bullet.GetComponent<TrailRenderer>().colorGradient = gradient;
            }
        }


    }

    void Update()
    {
        if (transform.position.x + 7.5f < player.transform.position.x) gameObject.SetActive(false);
    }

    public void Spawn()
    {

        stats = new EnemyStats(Random.Range(6f, 9f), 2,0,Random.Range(1.8f - (PlayerStats.difficulty / 30f), 3.2f - (PlayerStats.difficulty / 30f)), 250);
        if (stats.droppingFreq < 0.5f) stats.droppingFreq = 0.5f;
        anim.speed = stats.moveSpeed * 0.15f;
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
        if (plr.GetComponent<Rigidbody2D>().velocity.x > 0f) multiplier = plr.GetComponent<Rigidbody2D>().velocity.x * (plr.GetComponent<Rigidbody2D>().velocity.x / 3f);
        if (multiplier > 1f) stats.points += multiplier * 295f;
        else stats.points += 295f;
        if (stats.health <= 0 && !dead)
        {
            AudioManager.am._audio.PlayOneShot(AudioManager.am._alien);
            GameManager.gm.AddPoints(stats.points, transform.position);
          
            gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
            foreach (Transform child in GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
            }
            dead = true;
            ObjectPooler.op.Spawn("AlienDeath", transform.position);
            gameObject.SetActive(false);
        }
    }
}
