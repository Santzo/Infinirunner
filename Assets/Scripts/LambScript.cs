using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LambScript : MonoBehaviour, IEnemyDamage, ISpawn
{
    private EnemyStats stats;
    private Rigidbody2D rb;
    private Animator anim;
    private GameObject player;
    private HealthBarScript playerHealth;
    // Start is called before the first frame update

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<HealthBarScript>();
        player = GameObject.FindGameObjectWithTag("Player");

        rb = GetComponent<Rigidbody2D>();
        Spawn();
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        if (!stats.dead)
        {
            rb.MovePosition(transform.position + transform.localScale.x * -transform.right * stats.moveSpeed * Time.fixedDeltaTime);
        }
    }
    void Update()
    {
        
        if (!stats.dead)
        {
            float turn = Random.Range(0f, 1f);
            if (turn >= stats.turnRate)
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            }
        }
        if (transform.position.x + 5.5f < player.transform.position.x || transform.position.y < -7f) gameObject.SetActive(false);
    }

    public void EnemyDamage(float dmg)
    {
        if (!stats.dead)
        {
            AudioManager.am._audio.PlayOneShot(AudioManager.am._lambClip);
            stats.dead = true;
            anim.SetTrigger("Die");
            float multiplier = 1f;
            if (player.GetComponent<Rigidbody2D>().velocity.x > 0f) multiplier = player.GetComponent<Rigidbody2D>().velocity.x * (player.GetComponent<Rigidbody2D>().velocity.x / 15f);
            if (multiplier > 1f) stats.points += multiplier * 200f;
            else stats.points += 200f;
            GameManager.gm.AddPoints(stats.points, transform.position);

            foreach (Transform child in GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
            }
        }
    }

    public void Spawn()
    {
        stats = new EnemyStats(Random.Range(0.5f, 1f), 1f, 2, 0, 0f, 1);
        stats.turnRate = 0.99f;
        if (gameObject.layer == LayerMask.NameToLayer("DeadEnemy"))
        {
            foreach (Transform child in GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = LayerMask.NameToLayer("Enemy");
            }
        }

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("PlayerHitbox") && !PlayerStats.invincible && gameObject.layer != LayerMask.NameToLayer("DeadEnemy") && PlayerStats.boost <= 0f)
        {
            playerHealth.TakeDamage(1, 5);
            if (PlayerStats.slowDown == 1f)
            {
                player.GetComponent<PlayerScript>().StartSlowDown(0.5f, 2f);
              
            }
        }
    }
}
