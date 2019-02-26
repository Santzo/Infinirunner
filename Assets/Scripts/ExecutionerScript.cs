using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionerScript : MonoBehaviour, ISpawn, IEnemyDamage
{
    private EnemyStats stats;
    private Rigidbody2D rb;
    private GameObject player;
    private Animator anim;
    private HealthBarScript hbs;
    private float timer;
    private bool dead;
    private bool axeSwing;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        hbs = GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<HealthBarScript>();
        player = GameObject.FindGameObjectWithTag("Player");

        Spawn();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!dead)
        {
            rb.MovePosition(transform.position + -transform.right * stats.moveSpeed * Time.fixedDeltaTime);
            float sound = Mathf.Round(anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 * 10f) / 10f;
            if (transform.position.x - (GameManager.gm.wsW - 2.82f) <= player.transform.position.x && !PlayerStats.dead)
            {
                if (!axeSwing && sound == 1f)
                {
                    AudioManager.am._audio.PlayOneShot(AudioManager.am._axeSwing, 0.2f);
                    axeSwing = true;
                }
                if (axeSwing && sound < 1f) axeSwing = false;
            }
        }
        if (dead)
        {
            timer += Time.deltaTime;
            if (timer > 5f) gameObject.SetActive(false);
        }
    }
    void Update()
    {
      
        if (transform.position.x + 5.5f < player.transform.position.x) gameObject.SetActive(false);
    }

    public void Spawn()
    {
        timer = 0f;
        rb.isKinematic = true;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        stats = new EnemyStats(Random.Range(2f + PlayerStats.difficulty, 8f + PlayerStats.difficulty), 3f, 3, 0f, 0);
        stats.moveSpeed += PlayerStats.difficulty / 2f;
        anim.speed = 1f + (stats.moveSpeed / 10f);
        dead = false;
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
        if (player.GetComponent<Rigidbody2D>().velocity.x > 0f) multiplier = player.GetComponent<Rigidbody2D>().velocity.x * (player.GetComponent<Rigidbody2D>().velocity.x / 3f);
        if (multiplier > 1f) stats.points += multiplier * 145f;
        else stats.points += 145f;
        if (stats.health <= 0 && !dead)
        {
            int a = Random.Range(1, 3);
            if (a <= 1) AudioManager.am._audio.PlayOneShot(AudioManager.am._manScream1);
            else AudioManager.am._audio.PlayOneShot(AudioManager.am._manScream2);
            GameManager.gm.AddPoints(stats.points, transform.position);
            anim.speed = 0f;
            rb.isKinematic = false;
            float force = Random.Range(-1000f, 1000f);
            float vForce = Random.Range(-1000f, 1000f);
            float torque = Random.Range(-500f, 500f);
            GameObject _bss = ObjectPooler.op.Spawn("BloodSplatter", transform.position);
            _bss.transform.localScale = new Vector2(_bss.transform.localScale.x * 2.5f, _bss.transform.localScale.y * 2.5f);
            rb.AddForce(Vector2.right * force + Vector2.up * vForce);
            rb.AddTorque(torque);
            gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
            foreach (Transform child in GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
            }
            dead = true;
        }
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("PlayerHitbox") && !dead && !PlayerStats.invincible && PlayerStats.boost <= 0f)
        {
            if (collision.otherCollider.tag == "Axe")
            {
                hbs.TakeDamage(1, 10);
                ObjectPooler.op.Spawn("BloodSplatter", collision.GetContact(0).point);
            }
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(-1.25f * stats.moveSpeed, player.GetComponent<Rigidbody2D>().velocity.y);

        }
    }
}
