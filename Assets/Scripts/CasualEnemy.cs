using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class CasualEnemy : Npc
{

    UnityEngine.UI.Slider slider;

    public float iceEnchantSlowModifier = 0.5f;

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

    protected override Vector2 Move()
    {
        return rb2D.position;
    }

    
    /* 
     * applyDamage (int damage) takes only an integer as an argument and should be used for
     * standard damage.
     * apply damage (int damage, string enchantment) needs both an integer and a string as arguments
     * and is used to apply special effects from enchantments (eg. slow from ice attacks)
     */

    public void applyDamage(int damage)
    {
        _hitpoints -= damage;
        print("Enemy took damage, health: " + _hitpoints);
    }

    public void applyDamage(int damage, string enchantment)
    {
        _hitpoints -= damage;
        print("Enemy took damage, health: " + _hitpoints);

    }


}
