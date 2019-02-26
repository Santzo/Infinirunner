using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundButtonSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float wsH = Camera.main.orthographicSize * 2;
        float wsW = wsH / Screen.height * Screen.width;
        SpriteRenderer sr;
        sr = GetComponent<SpriteRenderer>();

        transform.localScale = new Vector3(wsW / sr.sprite.bounds.size.x, wsH / sr.sprite.bounds.size.y, 1);
        transform.position = new Vector2(transform.position.x * transform.localScale.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
