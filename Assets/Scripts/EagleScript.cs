using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleScript : MonoBehaviour, ISpawn, IEnemyDamage
{
    private Rigidbody2D rb;
    private GameObject player;
    private GameObject playerCenter;
    private Animator anim;
    public EnemyStats stats;
    private bool hitPlayer;
    private float yPos;
    private float xPos;
    private HealthBarScript hbs;
    public bool dead;
    private bool detect;
    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        hbs = GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<HealthBarScript>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerCenter = GameObject.FindGameObjectWithTag("PlayerCenter");
        Spawn();
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        if (!dead)
        {
            if (!detect && !hitPlayer) rb.MovePosition(transform.position + -transform.right * stats.moveSpeed * Time.fixedDeltaTime);
            if (detect && !hitPlayer)
            {
                Vector2 pos = Vector2.MoveTowards(transform.position, playerCenter.transform.position, stats.moveSpeed * Time.fixedDeltaTime);
                rb.MovePosition(pos);
            }
            if (!detect && hitPlayer)
            {
                Vector2 pos = Vector2.MoveTowards(transform.position, new Vector2(xPos, yPos), stats.moveSpeed * Time.fixedDeltaTime);
                rb.MovePosition(pos);
            }


            if (player.transform.position.x + (GameManager.gm.wsW / 2) > transform.position.x && !detect && !hitPlayer)
            {
                detect = true;
                stats.moveSpeed *= 2.5f;
                anim.SetTrigger("Attack");
            }
        }

        if (transform.position.x + 7f < player.transform.position.x || transform.position.y + 2f > GameManager.gm.wsH) gameObject.SetActive(false);
    }
    void Update()
    {
        
    }

    public void Spawn()
    {
        
        stats = new EnemyStats(8f + PlayerStats.difficulty, 2, 0, 0, 0);
        hitPlayer = false;
        detect = false;
        anim.speed = 1f;
        dead = false;
        yPos = transform.position.y + GameManager.gm.wsH;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.layer = LayerMask.NameToLayer("Enemy");
        }

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("PlayerHitbox") && !dead && !hitPlayer)
        {
            detect = false;
            anim.SetTrigger("Fly");
            hitPlayer = true;
            stats.moveSpeed /= 2.5f;
            xPos = playerCenter.transform.position.x - GameManager.gm.wsW / 2f;
            if (!PlayerStats.invincible && PlayerStats.boost <= 0f)
            {
                hbs.TakeDamage(1, 15);
                ObjectPooler.op.Spawn("BloodSplatter", collision.GetContact(0).point);
            }
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(-.5f * stats.moveSpeed, player.GetComponent<Rigidbody2D>().velocity.y);

        }

        if (collision.collider.CompareTag("FFHitbox") && !dead && !hitPlayer)
        {
            detect = false;
            anim.SetTrigger("Fly");
            hitPlayer = true;
            stats.moveSpeed /= 2.5f;
            xPos = playerCenter.transform.position.x - GameManager.gm.wsW / 2f;
            ObjectPooler.op.Spawn("ShipParticles", collision.GetContact(0).point);
        }
    }

    public void EnemyDamage(float dmg)
    {
        stats.health -= dmg;
        float multiplier = 1f;
        if (player.GetComponent<Rigidbody2D>().velocity.x > 0f) multiplier = player.GetComponent<Rigidbody2D>().velocity.x * (player.GetComponent<Rigidbody2D>().velocity.x / 3f);
        if (multiplier > 1f) stats.points += multiplier * 235f;
        else stats.points += 235f;
        if (stats.health <= 0 && !dead)
        {
            AudioManager.am._audio.PlayOneShot(AudioManager.am._eagle);
            GameManager.gm.AddPoints(stats.points, transform.position);
            anim.speed = 0f;
            rb.isKinematic = false;
            float torque = Random.Range(-100f, 100f);
            GameObject _bss = ObjectPooler.op.Spawn("BloodSplatter", transform.position);
            _bss.transform.localScale = new Vector2(_bss.transform.localScale.x * 1.5f, _bss.transform.localScale.y * 2.5f);
           
            gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
            foreach (Transform child in GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
            }
            dead = true;
            rb.AddTorque(torque);
        }
    }
}
