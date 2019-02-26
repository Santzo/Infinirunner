using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Globalization;


public class PlayerScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    private TimeSpan finaltime;
    public static bool isGrounded;
    private float jumpAnimSpeed;
    private Transform rifleHead;
    private int speedUpdate;
    private int showScore;
    private Vector2 speed;
    private bool audioPlay;
    private bool audioPlay2;
    private bool audioPlay3;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI timerText;
    public GameObject rifle;
    public GameObject forceField;
    private SpriteRenderer[] colori;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        colori = gameObject.GetComponentsInChildren<SpriteRenderer>();
      
        anim = GetComponent<Animator>();
        speedUpdate = 0;
        rifleHead = GameObject.FindGameObjectWithTag("RifleHead").transform;
        rifle = GameObject.FindGameObjectWithTag("Rifle");
        pointsText = GameObject.FindGameObjectWithTag("PointsText").GetComponent<TextMeshProUGUI>();
        speedText = GameObject.FindGameObjectWithTag("SpeedText").GetComponent<TextMeshProUGUI>();
        pointsText.text = "0000000";
        speedText.text = "0 km/h";
        ResetStats();
        IniClouds();
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        
        if (!PlayerStats.dead) rb.velocity = new Vector2(rb.velocity.x + PlayerStats.runSpeed * PlayerStats.slowDown * Time.fixedDeltaTime, rb.velocity.y);
        
    }
    void Update()
    {
        if (PlayerStats.boost > 0f && !forceField.activeSelf) forceField.SetActive(true);
        if (PlayerStats.boost <= 0f && forceField) forceField.SetActive(false);
        if (PlayerStats.boost > 0f)
        {
            PlayerStats.boost -= Time.deltaTime;
            if (PlayerStats.boost < 0f) PlayerStats.boost = 0f;
        }
        if (rb.velocity.x >= 0f) rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x - (rb.velocity.x / 100f), 2.5f, Mathf.Infinity), rb.velocity.y);
        if (rb.velocity.x > 0f) PlayerStats.points += rb.velocity.x * 0.5f;

        if (speedUpdate > 5 && !PlayerStats.dead && Time.timeScale != 0f)
        {
            if (PlayerStats.boost <= 0f && AudioManager.am._forceField.isPlaying) AudioManager.am._forceField.Stop();
            if (transform.position.y < -5f) gameObject.SetActive(false);
            if (Time.time - (10f + PlayerStats.difficulty * 2) >= PlayerStats.levelTime)
            {
                PlayerStats.difficulty++;
                PlayerStats.levelTime = Time.time;
            }

            PlayerStats.avgSpeed += rb.velocity.x * 2f;
            PlayerStats.speedDivider++;
            float speed = Mathf.Round((rb.velocity.x * 2f) * 10f) / 10f;
            
            PlayerStats.points = Mathf.Round(PlayerStats.points);
                CloudMaker();
                EnemyMaker();

            pointsText.text = ((int) PlayerStats.points).ToString("D7");
            speedText.text = speed.ToString() + " km/h";
            speedUpdate = 0;
        }

        
        speedUpdate++;
       
        anim.SetFloat("Speed", rb.velocity.x);
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            float sound = Mathf.Round(anim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 * 10f) / 10f;
            if (sound == 0.1f && !audioPlay && isGrounded || sound == 0.6f && !audioPlay2 && isGrounded)
            {
                audioPlay = true;
                audioPlay2 = true;
                AudioManager.am._audio.PlayOneShot(AudioManager.am._step);
            }
            if (sound != 0.1f)
            {
                if (audioPlay) audioPlay = false;
            }
            if (sound != 0.6f)
            {
                if (audioPlay2) audioPlay2 = false;
            }



            anim.speed = Mathf.Clamp(rb.velocity.x / 7.5f, 0f, rb.velocity.x / 7.5f);

        
        }


        if (PlayerStats.runSpeed > 0f)
        {
            PlayerStats.runSpeed -= PlayerStats.speedReduction * Time.deltaTime;
            if (PlayerStats.runSpeed < 0f) PlayerStats.runSpeed = 0f;
        }


    }


    public void Dead()
    {
        PlayerStats.dead = true;
        GameManager.gm.totalTime = Time.time - PlayerStats.playTime;
        if (PlayerStats.maxShots > 0) DeathScreenScript.accuracy = (float)PlayerStats.shots / (float)PlayerStats.maxShots;
        else DeathScreenScript.accuracy = 0;
        DeathScreenScript.timeSurvived = Time.time - PlayerStats.playTime;
        DeathScreenScript.avgSpeed = PlayerStats.avgSpeed / PlayerStats.speedDivider;
        DeathScreenScript.UpdateScore();
        rb.gravityScale = 1f;
        GameObject _blood = ObjectPooler.op.Spawn("BloodSplatter", transform.position);
        _blood.transform.localScale = new Vector2(2f, 2f);
        foreach (Transform child in GetComponentInChildren<Transform>(true))
        {
            child.gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
        }
        float force = UnityEngine.Random.Range(0f, 400f);
        float vForce = UnityEngine.Random.Range(0f, 1000f);
        rb.freezeRotation = false;
        rb.AddForce(-transform.right * force + transform.up * vForce);
        rb.AddTorque(20f);
        gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
      
    }
    void CloudMaker()
    {
        float spawn = UnityEngine.Random.Range(0f, 1f);
        float willSpawn = 1f - (rb.velocity.x / 150f);
        if (willSpawn > 0.97f) willSpawn = 0.97f;
        if (spawn > willSpawn)
        {
            float spawnX = UnityEngine.Random.Range(GameManager.gm.wsW + 10f, GameManager.gm.wsW + 25f);
            float cloudY = UnityEngine.Random.Range(-1f, 5.5f);
            ObjectPooler.op.Spawn("Cloud", new Vector2(transform.position.x + spawnX, cloudY), Quaternion.identity);
        }
    }

    void EnemyMaker()
    {
      
        float spawn = UnityEngine.Random.Range(0f, 1f);
        
        float willSpawn = 1f - (rb.velocity.x / 150f);

        if (willSpawn > 0.97f) willSpawn = 0.97f;


        if (spawn > willSpawn)
        {
            GameObject[] _bat = GameObject.FindGameObjectsWithTag("BatMain");
            GameObject[] _lamb = GameObject.FindGameObjectsWithTag("Lamb");
               
            if (_bat.Length <= PlayerStats.difficulty && _bat.Length < 5)
            {
                float spawnX = UnityEngine.Random.Range(GameManager.gm.wsW + 8f, GameManager.gm.wsW + 30f);
                float cloudY = UnityEngine.Random.Range(-1.5f, 4f);
                ObjectPooler.op.Spawn("Bat", new Vector2(transform.position.x + spawnX, cloudY), Quaternion.identity);
            }

            if (_lamb.Length <= PlayerStats.difficulty && _lamb.Length < 5)
            {
                float spawnX = UnityEngine.Random.Range(GameManager.gm.wsW + 8f, GameManager.gm.wsW + 30f);
                ObjectPooler.op.Spawn("Lamb", new Vector2(transform.position.x + spawnX, -4f + willSpawn), Quaternion.identity);
            }
                

            if (Time.time - PlayerStats.startTime > 3f)
            {
                float spawnX = UnityEngine.Random.Range(GameManager.gm.wsW + 8f, GameManager.gm.wsW + 50f);
                ObjectPooler.op.Spawn("Spikes", new Vector2(transform.position.x + spawnX, -4.5f + willSpawn), Quaternion.identity);
                PlayerStats.startTime = Time.time;
            }
        }

        GameObject a = GameObject.FindGameObjectWithTag("HealthSphere");
        if (!a)
        {
            float spawnX = UnityEngine.Random.Range(GameManager.gm.wsW + 8f, GameManager.gm.wsW + 30f);
            float spawnY = UnityEngine.Random.Range(-1.5f, 3f);
            ObjectPooler.op.Spawn("HealthSphere", new Vector2(transform.position.x + spawnX, spawnY), Quaternion.Euler(0f, 0f, 0f));
        }

        if (spawn > 0.99f && rb.velocity.x > 3f)
        {
            a = GameObject.FindGameObjectWithTag("BoostSphere");
            if (!a)
            {
                float spawnX = UnityEngine.Random.Range(GameManager.gm.wsW + 8f, GameManager.gm.wsW + 30f);
                float spawnY = UnityEngine.Random.Range(-1.5f, 3f);
                ObjectPooler.op.Spawn("BoostSphere", new Vector2(transform.position.x + spawnX, spawnY), Quaternion.Euler(0f, 0f, 0f));
            }
        }


        if (PlayerStats.difficulty >= 2)
        {
            GameObject[] _executioner = GameObject.FindGameObjectsWithTag("Executioner");
            if (_executioner.Length <= PlayerStats.difficulty - 2 && _executioner.Length <= 2)
            {
                float spawnX = UnityEngine.Random.Range(GameManager.gm.wsW + 8f, GameManager.gm.wsW + 180f);
                ObjectPooler.op.Spawn("Executioner", new Vector2(transform.position.x + spawnX, -3f + willSpawn), Quaternion.identity);
            }
        }

        if (PlayerStats.difficulty >= 4)
        {
            GameObject[] _eagle = GameObject.FindGameObjectsWithTag("Eagle");
            if (_eagle.Length <= PlayerStats.difficulty - 4 && _eagle.Length <= 1 || PlayerStats.difficulty >= 7 && _eagle.Length <= 2)
            {
                float spawnX = UnityEngine.Random.Range(GameManager.gm.wsW + 8f, GameManager.gm.wsW + 180f);
                float cloudY = UnityEngine.Random.Range(-1f, 5f);
                ObjectPooler.op.Spawn("Eagle", new Vector2(transform.position.x + spawnX, cloudY), Quaternion.identity);
            }
        }

        if (PlayerStats.difficulty >= 5)
        {
            GameObject[] _walker = GameObject.FindGameObjectsWithTag("WalkerAlien");
            if (PlayerStats.difficulty < 8 && _walker.Length <= 1 || PlayerStats.difficulty >= 8 && _walker.Length <= 2)
            {
                float spawnX = UnityEngine.Random.Range(GameManager.gm.wsW + 8f, GameManager.gm.wsW + 180f);
                ObjectPooler.op.Spawn("WalkerAlien", new Vector2(transform.position.x + spawnX, -4.4f + willSpawn), Quaternion.identity);
            }
        }

        if (PlayerStats.difficulty >= 6)
        {
            GameObject alien = GameObject.FindGameObjectWithTag("Alien");
            if (!alien)
            {
                float spawnX = UnityEngine.Random.Range(GameManager.gm.wsW + 40f, GameManager.gm.wsW + 200f);
                ObjectPooler.op.Spawn("Alien", new Vector2(transform.position.x + spawnX, 3.2f), Quaternion.identity);

            }
        }


         
           
      
        
        
    }

    void IniClouds()
    {
        int num = UnityEngine.Random.Range(1, 15);
        for (int i = 1; i <= num; i++)
        {
            float spawnX = UnityEngine.Random.Range(-2f, 30f);
            float cloudY = UnityEngine.Random.Range(-1f, 5.5f);
            ObjectPooler.op.Spawn("Cloud", new Vector2(transform.position.x + spawnX, cloudY), Quaternion.identity);

        }
        float spawnXa = UnityEngine.Random.Range(transform.position.x + 9f, GameManager.gm.wsW + 10f);
        float cloudYa = UnityEngine.Random.Range(-1.5f, 4f);
        ObjectPooler.op.Spawn("Bat", new Vector2(transform.position.x + spawnXa, cloudYa), Quaternion.identity);

        spawnXa = UnityEngine.Random.Range(transform.position.x + 9f, GameManager.gm.wsW + 10f);
        ObjectPooler.op.Spawn("Lamb", new Vector2(transform.position.x + spawnXa, -3f), Quaternion.identity);
        spawnXa = UnityEngine.Random.Range(GameManager.gm.wsW, GameManager.gm.wsW + 10f);
        ObjectPooler.op.Spawn("Spikes", new Vector2(transform.position.x + spawnXa, -4.0f), Quaternion.identity);
        PlayerStats.startTime = Time.time;
    }

    public void Shoot()
    {

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x >= rifleHead.transform.position.x && !PlayerStats.dead) 
        {
            AudioManager.am._audio.PlayOneShot(AudioManager.am._laserClip);
            PlayerStats.maxShots++;
            GameObject _bullet = ObjectPooler.op.Spawn("Bullet", rifleHead.transform.position, Quaternion.identity);
            _bullet.transform.right = new Vector2(mousePos.x - rifleHead.transform.position.x, mousePos.y - rifleHead.transform.position.y);
            if (PlayerStats.boost > 0f) _bullet.GetComponent<BulletScript>().boostBullet = true;
        }
    }

    public void Jump()
    {
        anim.SetTrigger("Jump");

        
        jumpAnimSpeed = ((PlayerStats.maxJumpForce / 6.5f) + 0.1f) - (PlayerStats.jumpForce / 10f);
        if (jumpAnimSpeed < 0.01f) jumpAnimSpeed = 0.01f;
        anim.speed = jumpAnimSpeed;
        rb.gravityScale = 1f;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "GroundCheck")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "GroundCheck")
        {
            isGrounded = false;
        }
    }

    public void ResetStats()
    {
        rb.freezeRotation = true;
        PlayerStats.runSpeed = 0f;
        PlayerStats.difficulty = 1;
        PlayerStats.speedAdd = 37.5f;
        PlayerStats.maxJumpForce = 1.28f;
        PlayerStats.jumpForce = 0f;
        PlayerStats.invincible = false;
        PlayerStats.dead = false;
        PlayerStats.speedReduction = 250f;
        PlayerStats.damage = 1f;
        PlayerStats.points = 0f;
        showScore = (int) PlayerStats.points;
        PlayerStats.maxHealth = 5;
        PlayerStats.health = PlayerStats.maxHealth;
        PlayerStats.playTime = PlayerStats.startTime = PlayerStats.levelTime = Time.time;
        PlayerStats.maxTime = 30;
        PlayerStats.slowDown = 1f;
        PlayerStats.healing = 1;
        PlayerStats.maxShots = PlayerStats.shots = 0;
        PlayerStats.avgSpeed = 0f;
        PlayerStats.speedDivider = 0;
        PlayerStats.boost = 0f;
        GameObject.FindGameObjectWithTag("HealthButton").GetComponent<HealthButtonScript>().UpdateHealth();
    }



    public void GetHit(int invlenght)
    {
        PlayerStats.invincible = true;
        StopCoroutine("Blink");
        StartCoroutine("Blink", invlenght);
    }

    public void Boost()
    {

    }

    IEnumerator Blink(int invlenght)
    {

        float change = 0.05f - (invlenght * 0.001f);
        rb.velocity = new Vector2(0f, rb.velocity.y);
        for (int a = 1; a <= 3; a++)
        {
            for (int i = 1; i <= invlenght; i++)
            {
                foreach (SpriteRenderer color in colori)
                {
                    color.color = new Color(color.color.r - change, color.color.g - change, color.color.b - change, color.color.a - change);
                }
                yield return null;
            }
            for (int i = 1; i <= invlenght; i++)
            {
                foreach (SpriteRenderer color in colori)
                {
                    color.color = new Color(color.color.r + change, color.color.g + change, color.color.b + change, color.color.a + change);
                }
                yield return null;
            }
        }
        PlayerStats.invincible = false;
        yield return null;

    }
    public void StartSlowDown(float amount, float duration)
    {
        StopCoroutine(SlowDown(0f,0f));
        StartCoroutine(SlowDown(amount, duration));
    }
    public IEnumerator SlowDown(float amount, float duration)
    {
        PlayerStats.slowDown = amount;
        yield return new WaitForSeconds(2f);
        PlayerStats.slowDown = 1f;
        yield return null;
    }
}
