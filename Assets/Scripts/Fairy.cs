using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fairy : MonoBehaviour {

    public MovingObj target;
    public Vector3 FAIRY_DISTANCE;                  //Distance between fairy and other player
    public int damageAOE = 20;                      //Damage of Fairy AOE Attack
    private float attackCD = 0.3f;                  //Duration between AOE Attacks
    private float attackTimer = 0;
    private float enchantmentCD = 10f;              //Duration between enchantments
    private float enchantmentEffectTimer = 0;
    private float enchantmentEffectDuration = 5f;   //Duration of a single enchantment spell
    private float enchantmentTimer = 0;
    private bool isAttacking = false;
    protected CircleCollider2D circleCollider;

 
    void Start () {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;
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


    //Damages enemies if they enter the fairy AOE and the fairy attacks
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isAttacking == true && other.CompareTag("CasualEnemy"))
        {
            CasualEnemy ce = other.GetComponent<CasualEnemy>();
            ce.applyDamage(damageAOE);
            print("Enemy attacked");
        }
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
