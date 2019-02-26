using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBlockScript : MonoBehaviour
{
    public float start;
    public float inc;
    public bool darker;
    private Image clr;
    // Start is called before the first frame update
    void Start()
    {
        darker = true;
        start = 0.2f;
        inc = 0.005f;
        clr = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (darker)
        {
            start -= 0.01f;
            clr.color = new Color(clr.color.r - inc, clr.color.g - inc, clr.color.b - inc, clr.color.a);
            if (start <= 0f) darker = !darker;
        }
        if (!darker)
        {
            start += 0.01f;
            clr.color = new Color(clr.color.r + inc, clr.color.g + inc, clr.color.b + inc, clr.color.a);
            if (start >= 0.2f) darker = !darker;
        }

    }
}
