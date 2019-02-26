using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatDroppingScript : MonoBehaviour, ISpawn
{
    private float moveSpeed;
    private Rigidbody2D rb;
    private HealthBarScript playerHealth;
    public int damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<HealthBarScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(transform.position + Vector3.down * moveSpeed * Time.fixedDeltaTime);
    }

    void Update()
    {
        if (transform.position.y < -5f) gameObject.SetActive(false);
    }

    public void Spawn()
    {
        moveSpeed = Random.Range(6f, 12f);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "PlayerHitbox" && !PlayerStats.invincible && PlayerStats.boost <= 0f)
        {
            ObjectPooler.op.Spawn("BloodSplatter", transform.position, null, collision.collider.gameObject.transform);
            playerHealth.TakeDamage(damage);
        }
    }
}
