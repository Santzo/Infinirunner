using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereScript : MonoBehaviour, ISpawn
{
    private HealthBarScript hbs;
    private bool healthMove;
    private float healthSpeed = 10f;
    private float threshold = 1f;
    private HealthButtonScript healthButton;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        healthButton = GameObject.FindGameObjectWithTag("HealthButton").GetComponent<HealthButtonScript>();
        hbs = GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<HealthBarScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x + 5.5f < player.transform.position.x) gameObject.SetActive(false);
        if (healthMove)
        {
            Vector3 dest = Camera.main.ScreenToWorldPoint(healthButton.transform.position);
            transform.right = new Vector2 (dest.x - transform.position.x, dest.y -  transform.position.y);
            transform.position += transform.right * Time.deltaTime * healthSpeed;
            if (Mathf.Abs(transform.position.x - dest.x) < threshold && Mathf.Abs(transform.position.y - dest.y) < threshold)
            {
                healthMove = false;
                PlayerStats.healing++;
                healthButton.UpdateHealth();
                ObjectPooler.op.Spawn("PickUp", transform.position);
                gameObject.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.tag == "HealthSphere" && collision.collider.CompareTag("PlayerHitbox"))
        {
            if (PlayerStats.health >= PlayerStats.maxHealth)
            {
                healthMove = true;
                AudioManager.am._audio.PlayOneShot(AudioManager.am._ohYeah, 0.4f);
            }
            else
            {

                ObjectPooler.op.Spawn("PickUp", transform.position);
                hbs.TakeDamage(-1, 0);
                gameObject.SetActive(false);
            }
            
        }

        if (this.tag == "BoostSphere" && collision.collider.CompareTag("PlayerHitbox"))
        {
            AudioManager.am._forceField.Play();
            ObjectPooler.op.Spawn("PickUp", transform.position);
            PlayerStats.boost += 5f;
            player.GetComponent<PlayerScript>().Boost();
            gameObject.SetActive(false);
        }
    }

    public void Spawn()
    {

    }
}
