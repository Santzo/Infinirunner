using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Image img;
    private Color oriColor;
    private bool pressed;


    // Start is called before the first frame update
    void Awake()
    {
        img = GetComponent<Image>();
        oriColor = img.color;
       
    }


    void Start()
    {

        if (tag == "RunButton")
        {
            float x = PlayerPrefs.GetFloat("RunX", Screen.width - 90f * SaveDataManager.sdm.xMulti);
            float y = PlayerPrefs.GetFloat("RunY", 150f * SaveDataManager.sdm.yMulti);
            transform.position = new Vector2(x, y);
        }

        if (tag == "JumpButton")
        {
            float x = PlayerPrefs.GetFloat("JumpX", 90f * SaveDataManager.sdm.xMulti);
            float y = PlayerPrefs.GetFloat("JumpY", 150f * SaveDataManager.sdm.yMulti);
            transform.position = new Vector2(x, y);
        }
        if (tag == "HealthButton")
        {
            float x = PlayerPrefs.GetFloat("HealthX", 90f * SaveDataManager.sdm.xMulti);
            float y = PlayerPrefs.GetFloat("HealthY", 400f * SaveDataManager.sdm.yMulti);
            transform.position = new Vector2(x, y);
        }
        if (tag == "BoostButton")
        {
            float x = PlayerPrefs.GetFloat("BoostX", 90f * SaveDataManager.sdm.xMulti);
            float y = PlayerPrefs.GetFloat("BoostY", 650f * SaveDataManager.sdm.yMulti);
            transform.position = new Vector2(x, y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        img.color = new Color(oriColor.r - 0.5f, oriColor.g - 0.5f, oriColor.b - 0.5f, oriColor.a - 0.5f);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (pressed)
        {
            if (tag == "RunButton")
            {
                PlayerPrefs.SetFloat("RunX", transform.position.x);
                PlayerPrefs.SetFloat("RunY", transform.position.y);
            }

            if (tag == "JumpButton")
            {
                PlayerPrefs.SetFloat("JumpX", transform.position.x);
                PlayerPrefs.SetFloat("JumpY", transform.position.y);
            }
            if (tag == "HealthButton")
            {
                PlayerPrefs.SetFloat("HealthX", transform.position.x);
                PlayerPrefs.SetFloat("HealthY", transform.position.y);
            }
            if (tag == "BoostButton")
            {
                PlayerPrefs.SetFloat("BoostX", transform.position.x);
                PlayerPrefs.SetFloat("BoostY", transform.position.y);
            }
        }
        pressed = false;
        img.color = oriColor;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (pressed)
        {
            float x = img.rectTransform.rect.size.x / 2f;
            float y = img.rectTransform.rect.size.y / 2f;
            transform.position = new Vector2(Mathf.Clamp(Input.mousePosition.x, 0f + x, Screen.width - x), Mathf.Clamp(Input.mousePosition.y, 0f + y, Screen.height - y));
        }
    }
}
