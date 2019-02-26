using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public AudioClip _batClip;
    public AudioClip _lambClip;
    public AudioClip _laserClip;
    public AudioClip _manScream1;
    public AudioClip _manScream2;
    public AudioClip _click;
    public AudioClip _grunt1;
    public AudioClip _grunt2;
    public AudioClip _step;
    public AudioClip _eagle;
    public AudioSource _forceField;
    public AudioSource _ufo;
    public AudioClip _ohYeah;
    public AudioClip _jump;
    public AudioSource _countdown;
    public AudioClip _splat;
    public AudioClip _alien;
    public AudioClip _explosion;
    public AudioClip _fanfare;
    public AudioClip _axeSwing;
    public AudioSource _audio;
    public static AudioManager am;
    // Start is called before the first frame update
    private void Awake()
    {
        if (am==null)
        {
            am = this;
            DontDestroyOnLoad(am);
        }
        else if(am!=null)
        {
            Destroy(gameObject);
        }
        _audio = GetComponent<AudioSource>();
        _audio.mute = Convert.ToBoolean(PlayerPrefs.GetInt("Mute", 0));
        _forceField.mute = _ufo.mute =  _countdown.mute = _audio.mute;
    }
     
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     
    }
}
