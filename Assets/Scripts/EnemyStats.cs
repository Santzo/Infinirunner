using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats
{
    public float moveSpeed;
    public GameObject player;
    public Animator anim;
    public float health;
    public int repAmmo;
    public bool dead = false;
    public Rigidbody2D rb;
    public float droppingFreq;
    public float points;
    public float turnRate;
    public float timeAdd;

    public EnemyStats(float _moveSpeed, float _health, int _repAmmo, float _droppingFreq, float _points, int _timeAdd = 0)
    {
        moveSpeed = _moveSpeed;
        health = _health;
        repAmmo = _repAmmo;
        dead = false;
        timeAdd = _timeAdd;
        droppingFreq = _droppingFreq;
        points = _points;
    }
 
}
