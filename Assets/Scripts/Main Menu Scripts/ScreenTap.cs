using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenTap : MonoBehaviour
{
    private TextMeshProUGUI txt;
    private bool lighten;
    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!lighten)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a - 0.025f);
            if (txt.color.a < 0.1f) lighten = true;
        }
        if (lighten)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a + 0.025f);
            if (txt.color.a > 1f) lighten = false;
        }
        if (Input.GetMouseButton(0))
        {
            AudioManager.am._audio.PlayOneShot(AudioManager.am._click, 0.8f);
            gameObject.SetActive(false);
        }
    }
}
