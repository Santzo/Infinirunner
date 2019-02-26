using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
 
{
    private GameObject player;
    private PlayerScript plr;
    private Color oriColor;
    private Image oriImg;
    private float curJump;
    private bool gonnaJump;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        plr = player.GetComponent<PlayerScript>();
        oriColor = GetComponent<Image>().color;
        oriImg = GetComponent<Image>();


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
       
    }
    void FixedUpdate()
    {
        if (gonnaJump)
        {
            PlayerStats.jumpForce += curJump * Time.fixedDeltaTime;
            curJump -= 0.05f;
            plr.anim.speed = Mathf.Clamp(2.2f - (PlayerStats.jumpForce * 1.5f), 0.45f, 2.2f - (PlayerStats.jumpForce * 1.5f));
            plr.rb.AddForce(transform.up * PlayerStats.jumpForce, ForceMode2D.Impulse);
            if (PlayerStats.jumpForce > PlayerStats.maxJumpForce) gonnaJump = false;
        }
    }
    // Update is called once per frame

    public void OnPointerDown(PointerEventData eventData)
    {
        
        oriImg.color = new Color(oriColor.r - 0.3f, oriColor.g - 0.3f, oriColor.b - 0.3f, 1f);

        if (this.tag == "RunButton" && PlayerScript.isGrounded) PlayerStats.runSpeed = PlayerStats.speedAdd;
        if (this.tag == "JumpButton" && PlayerScript.isGrounded)
        {
            AudioManager.am._audio.PlayOneShot(AudioManager.am._jump, 0.5f);
            gonnaJump = true;
            curJump = 4f;
            plr.Jump();

        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (this.tag == "JumpButton")
        {
            PlayerStats.jumpForce = 0f;
            gonnaJump = false;
            
        }
        oriImg.color = oriColor;
    }

}
