using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour, ISpawn
{
    public float moveSpeed;
    public GameObject bloodSplatter;
    public Rigidbody2D rbPlayer;
    public bool boostBullet;
    private HealthBarScript hbs;
    private GameObject player;
    private Rigidbody2D rb;
    private float playerX;
    public TrailRenderer trail;
    private int hit = 0;
    private LayerMask enemy;
    // Start is called before the first frame update
    void Start()
    {
        hbs = GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<HealthBarScript>();
        enemy = LayerMask.NameToLayer("Enemy");
        player = GameObject.FindGameObjectWithTag("RifleHead");
        rbPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.right * moveSpeed * Time.fixedDeltaTime);
        if (tag == "Bullet")
        {
            if (transform.position.x - GameManager.gm.wsW + (4.82f) > playerX || transform.position.y - GameManager.gm.wsH > player.transform.position.y)
            {
                float minus = 0f;
                if (!boostBullet) minus = Mathf.Round(PlayerStats.points * 0.02f);
                else minus = Mathf.Round(PlayerStats.points * 0.01f);
                PlayerStats.points -= minus;
                GameManager.gm.ShowText("-" + minus, new Vector2(-1f, 0f), "#D70D00", 3f, false);
              
                gameObject.SetActive(false);
            }
        }
    }
  

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("FFHitbox") && tag == "EnemyBullet" && PlayerStats.boost > 0f)
        {
            GameObject _bloodSplatter = ObjectPooler.op.Spawn("ShipParticles", collision.GetContact(0).point, null, collision.collider.gameObject.transform.parent.root);
            gameObject.SetActive(false);
        }

        if (collision.collider.CompareTag("PlayerHitbox") && tag=="EnemyBullet" && PlayerStats.boost <= 0f && !PlayerStats.invincible)
        {
            hbs.TakeDamage(1, 15);
            GameObject _bloodSplatter = ObjectPooler.op.Spawn("BloodSplatter", collision.GetContact(0).point, null, collision.collider.gameObject.transform.parent.root);
            gameObject.SetActive(false);
        }

        if (collision.collider.CompareTag("EnemyHitBox") && hit == 0 && tag=="Bullet")
        {
            string tagi = collision.collider.gameObject.transform.root.tag;
           

            if (hit == 0 && collision.collider.gameObject.layer == enemy)
            {
                AudioManager.am._audio.PlayOneShot(AudioManager.am._splat);
                PlayerStats.shots++;
                hit++;

                if (tagi != "WalkerAlien" && tagi != "Alien")
                {
                    GameObject _bloodSplatter = ObjectPooler.op.Spawn("BloodSplatter", collision.GetContact(0).point, null, collision.collider.gameObject.transform.parent.root);
                }
                else if (tagi == "Alien")
                {
                    GameObject _ship = ObjectPooler.op.Spawn("ShipParticles", collision.GetContact(0).point, Quaternion.Euler(0f,0f, -transform.eulerAngles.z), collision.collider.gameObject.transform.parent.root);
                }
                else
                {
                    GameObject _bloodSplatter = ObjectPooler.op.Spawn("AlienSplatter", collision.GetContact(0).point, null, collision.collider.gameObject.transform.parent.root);
                }
                collision.collider.gameObject.transform.root.GetComponent<IEnemyDamage>().EnemyDamage(PlayerStats.damage); 
                gameObject.SetActive(false);

            }
            
        }
    }
    public void Spawn()
    {
        boostBullet = false;
        playerX = transform.position.x;
        if (tag=="Bullet") moveSpeed = 50f;
        hit = 0;
        if (tag=="Bullet") trail.Clear();

    }


 
}
