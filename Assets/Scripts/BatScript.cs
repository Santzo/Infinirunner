using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatScript : MonoBehaviour, ISpawn, IEnemyDamage
{
    private float moveSpeed;
    private GameObject player;
    private Animator anim;
    public float health;
    public int repAmmo;
    private Rigidbody2D rb;
    private float droppingFreq;
    public float points;
    public SpriteRenderer eyes;
    public SpriteRenderer deadEyes;
    public SpriteRenderer bSplatter;
    private EnemyStats stats;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        stats = new EnemyStats(0f, 2f, 2, Random.Range(0.98f, 1.0f), 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.dead)
        {
            rb.MovePosition(transform.position + Vector3.down * 10f * Time.deltaTime);
        }

        float shoot = Random.Range(0f, 1.0f);
        if (shoot >= stats.droppingFreq && !stats.dead)
        {
            Shoot();
        }
        if (stats.health <= 0f && !stats.dead)
        {
            AudioManager.am._audio.PlayOneShot(AudioManager.am._batClip);
            GameManager.gm.AddPoints(stats.points, transform.position);
            eyes.enabled = false;
            deadEyes.enabled = true;
            bSplatter.enabled = true;
            transform.localScale = new Vector2(transform.localScale.x, -transform.localScale.y);
            gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
            foreach (Transform child in GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
            }
            anim.SetTrigger("Dead");
            stats.dead = true;
        }

        if (transform.position.x + 5.5f < player.transform.position.x || transform.position.y < -7f) gameObject.SetActive(false);
    }

    public void Spawn()
    {
        stats = new EnemyStats(0f, 2f, 2, Random.Range(0.98f, 1.0f), 0f);
        eyes.enabled = true;
        deadEyes.enabled = false;
        bSplatter.enabled = false;
        if (transform.localScale.y < 0) transform.localScale = new Vector2(transform.localScale.x, -transform.localScale.y);
        if (gameObject.layer == LayerMask.NameToLayer("DeadEnemy"))
        {
            foreach (Transform child in GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = LayerMask.NameToLayer("Enemy");
            }
        }

    }

    void Shoot()
    {
        ObjectPooler.op.Spawn("BatDropping", transform.position, Quaternion.identity);

    }
    public void EnemyDamage(float dmg)
    {
        stats.health -= dmg;
        float multiplier = 1f;
        if (player.GetComponent<Rigidbody2D>().velocity.x > 0f) multiplier = player.GetComponent<Rigidbody2D>().velocity.x * (player.GetComponent<Rigidbody2D>().velocity.x / 2.5f);
        if (multiplier > 1f) stats.points = multiplier * 135f;
        else stats.points += 135f;
    }
}
