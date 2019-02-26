using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KillTextScript : MonoBehaviour, ISpawn
{
    public float moveSpeed;
    private float xDir;
    private float alpha = 1f;
    private TextMeshPro color;
    public bool up;
    // Start is called before the first frame update
    void Start()
    {
        color = GetComponent<TextMeshPro>();
      

    }

    // Update is called once per frame
    void Update()
    {
        if (alpha <= 0f) gameObject.SetActive(false);
        color.color = new Color(color.color.r, color.color.g, color.color.b, alpha);
        alpha -= 0.015f;
        if (up) transform.Translate(Vector3.up * moveSpeed * Time.deltaTime + Vector3.right * xDir * Time.deltaTime);
        else transform.Translate(-Vector3.up * moveSpeed * Time.deltaTime + Vector3.right * xDir * Time.deltaTime);
    }

    public void Spawn()
    {
        xDir = Random.Range(-3f, 3f);
        moveSpeed = Random.Range(moveSpeed, moveSpeed + 1f);
        alpha = 1f;
    }
}
