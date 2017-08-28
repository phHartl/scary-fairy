using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class CasualEnemy : Npc
{

    UnityEngine.UI.Slider slider;

    private Boolean isBurning = false;
    private Boolean durationRefreshed = false;
    public Rigidbody2D potion;


    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        slider = GetComponentInChildren<UnityEngine.UI.Slider>();
        this._hitpoints = Constants.CASUAL_ENEMY_MAX_HEALTH;
        this._damage = Constants.CASUAL_ENEMY_BASE_DAMAGE;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
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

    
    /* 
     * applyDamage (int damage) takes only an integer as an argument and should be used for
     * standard damage.
     * apply damage (int damage, string enchantment) needs both an integer and a string as arguments
     * and is used to apply special effects from enchantments (eg. slow from ice attacks)
     */

    public override void applyDamage(int damage)
    {
        _hitpoints -= damage;
        checkDeath();
        print("Enemy took damage, health: " + _hitpoints);
    }

    public void applyDamage(int damage, string enchantment)
    {
        _hitpoints -= damage;
        checkDeath();
        print("Enemy took damage, health: " + _hitpoints);

        if(enchantment == Constants.FIRE_ENCHANTMENT)
        {
            if (!isBurning)
            {
                StartCoroutine(applyBurnDamage());
            }
            if (isBurning)
            {
                durationRefreshed = true;
            }
        }
    }

    private IEnumerator applyBurnDamage()
    {
        isBurning = true;
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        for (int i = 0; i < Constants.BURN_DAMAGE_DURATION; i++)
        {
            if (durationRefreshed)
            {
                print("Burn Refreshed");
                i = 0;
                durationRefreshed = false;
            }
            _hitpoints -= 2;
            print("Enemy got burned");
            yield return new WaitForSeconds(Constants.BURN_DAMAGE_TICKRATE);
        }
        isBurning = false;
        GetComponent<Renderer>().material.color = Color.white;
    }

   
}
