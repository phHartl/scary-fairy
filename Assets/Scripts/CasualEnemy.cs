using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class CasualEnemy : Npc
{

    UnityEngine.UI.Slider slider;

    private bool isKnockedBack = false;
    private Rigidbody2D rb;
    public Vector2 startMarker;
    public Vector2 endMarker;
    public float speed = 5;
    private float startTime;
    private float journeyLength;

    // Use this for initialization
    void Start()
    {
        base.Start();
        startTime = Time.time;
        rb2D = GetComponent<Rigidbody2D>();
        slider = GetComponentInChildren<UnityEngine.UI.Slider>();

        // hitpoints placeholder
        this._hitpoints = 100;

        // damage placeholder
        this._damage = 69;

        //movespeed placeholder
        this.moveSpeed = 0;
    }

    private void OnPathComplete(Path p)
    {
        Debug.Log("We did it" + p.error);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       //    base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();
        slider.value = _hitpoints;
        if (isKnockedBack)
        {
            float distCovered = (Time.time - startTime) * 50;
            float fracJourney = distCovered / journeyLength;
            if((Vector2)transform.position == endMarker)
            {
                isKnockedBack = false;
            }
            transform.position = Vector2.Lerp(startMarker, endMarker, fracJourney);
        }
    }

    // Pathfinding has yet to be implemented (I think there is a Unity-Plugin that can be used for pathfinding)
    protected override Vector2 Move()
    {
        return rb2D.position;
    }

    public void knockBack(Vector2 force)
    {
        startTime = Time.time;
        startMarker = transform.position;
        endMarker = startMarker + force;
        journeyLength = Vector2.Distance(startMarker, endMarker);
        isKnockedBack = true;
    }

    public void applyDamage(int damage)
    {
        _hitpoints -= damage;
        print("Enemy took damage, health: " + _hitpoints);
    }
}
