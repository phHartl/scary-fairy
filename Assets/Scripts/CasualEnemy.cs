using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class CasualEnemy : Npc
{

    UnityEngine.UI.Slider slider;
 

    // Use this for initialization
    void Start()
    {
        base.Start();
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
    }

    // Pathfinding has yet to be implemented (I think there is a Unity-Plugin that can be used for pathfinding)
    protected override Vector2 Move()
    {
        return rb2D.position;
    }

    public void applyDamage(int damage)
    {
        _hitpoints -= damage;
        print("Enemy took damage, health: " + _hitpoints);
    }
}
