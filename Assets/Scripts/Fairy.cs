using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fairy : Player {

    public MovingObj target;
    public Vector3 FAIRY_DISTANCE;                  //Distance between fairy and other player
    private float enchantmentCD = 10f;              //Duration between enchantments
    private float enchantmentEffectTimer = 0;
    private float enchantmentEffectDuration = 5f;   //Duration of a single enchantment spell
    private float enchantmentTimer = 0;
    protected CircleCollider2D circleCollider;

 
    void Start () {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;
        this.attackCD = 0.3f;
        this.attackTimer = 0;
        this._damage = 20; //Damage of Fairy AOE Attack
        print(target);
    }


    void LateUpdate () {
        transform.position = target.transform.position + FAIRY_DISTANCE;
        Attack();
        enchantAttacks();
    }


    //copy pasta MeleePlayer
    private void Attack()
    {
        if (Input.GetKeyDown ("q") && !isAttacking)
        {
            isAttacking = true;
            attackTimer = attackCD;
        }
        if (isAttacking)
        {
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
                circleCollider.enabled = true;
            }
            else
            {
                isAttacking = false;
                circleCollider.enabled = false;
            }
        }
    }

    private new void FixedUpdate()
    {

    }


    /*
     * Press Button "1" to enchant the other player with ice
     * Press Button "2" to enchant the other player with fire
     * Fire deals more damage than ice, ice does currently not have any extra effects. Maybe add slow.
    */
    private void enchantAttacks()
    {
        if (Input.GetKeyDown("1") && !target.getOnEnchantmentCD())
        {
            enchantmentEffectTimer = enchantmentEffectDuration;
            enchantmentTimer = enchantmentCD;
            target.activateIceEnchantment();
        }
        if (Input.GetKeyDown("2") && !target.getOnEnchantmentCD()){
            enchantmentEffectTimer = enchantmentEffectDuration;
            enchantmentTimer = enchantmentCD;
            target.activateFireEnchantment();
        }
        checkEnchantmentCD();
        checkEnchantmentDuration();
    }

    //Checks remaining enchantment duration, disables enchantment after 5 seconds (default value)
    private void checkEnchantmentDuration()
    {
        if(enchantmentEffectTimer > 0)
        {
            enchantmentEffectTimer -= Time.deltaTime;
        }
        else
        {
            target.resetEnchantments();
        }
    }


    //Checks if enchantment spell is ready again, 10 second Cooldown (default value)
    private void checkEnchantmentCD()
    {
        if (target.getOnEnchantmentCD())
        {
            if (enchantmentTimer > 0)
            {
                enchantmentTimer -= Time.deltaTime;
            }
            else
            {
                target.resetEnchantmentCooldown();
            }
        }
    }
}
