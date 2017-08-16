using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class CasualEnemy : Npc
{

    UnityEngine.UI.Slider slider;

    
    public Rigidbody2D potion;


    // Use this for initialization
    void Start()
    {
        base.Start();
        slider = GetComponentInChildren<UnityEngine.UI.Slider>();
        //animator = GetComponent<Animator>();
        this._hitpoints = 100;
        this._damage = 30;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        slider.value = _hitpoints;
        base.Update();
    }

    protected override Vector2 Move()
    {
        return rb2D.position;
    }

  

   
}
