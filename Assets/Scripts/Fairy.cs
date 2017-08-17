﻿using System;
using System.Collections;
using UnityEngine;

public class Fairy : Player, CooldownObserver {

    public MovingObj target;
    public Vector3 FAIRY_DISTANCE;                  //Distance between fairy and other player
    private float enchantmentCD = 5f;              //Duration between enchantments
    private float enchantmentEffectTimer = 0;
    private float enchantmentEffectDuration = 5f;   //Duration of a single enchantment spell
    private float enchantmentTimer = 0;

    private float speedBoostPower = 1.8f;
    private float speedBoostDuration = 5f;
    private float speedBoostCD = 8f;
    private bool speedBoostOnCd = false;

    protected CircleCollider2D circleCollider;
    private Animator novaAnimator;
 

    private void Start () {
        base.Start();
        isOnCoolDown = cdManager.GetFairyCooldown();
        circleCollider = GetComponent<CircleCollider2D>();
        novaAnimator = GetComponentsInChildren<Animator>()[1];
        circleCollider.enabled = false;
        this.baseDamage = 20; //Damage of Fairy AOE Attack
    }

    private void FixedUpdate()
    {
        
    }

    protected override void Update()
    {
        //animator.SetFloat("MoveX", Input.GetAxisRaw(target.GetComponent<Player>().axisHorizontal));
        //animator.SetFloat("MoveY", Input.GetAxisRaw(target.GetComponent<Player>().axisVertical));
        animator.SetFloat("LastMoveX", target.GetComponent<Player>().lastMove.x);
        animator.SetFloat("LastMoveY", target.GetComponent<Player>().lastMove.y);
        novaAnimator.SetBool("NovaAttack", isAttacking);
    }

    private void LateUpdate () {
        transform.position = target.transform.position + FAIRY_DISTANCE;
    }

    protected override void Attack()
    {
        _damage = baseDamage;
        isAttacking = true;
        circleCollider.enabled = true;
        isOnCoolDown[0] = true;
        cdManager.StartCooldown(0, 2);
    }

    //Called trough child object because of how an animation event works
    public void AttackOver()   
    {
        isAttacking = false;
        circleCollider.enabled = false;
    }



    public IEnumerator applyFireEnchantment()
    {
        if (!target.getOnEnchantmentCD())
        {
            print("Fire enchantment activated");
            target.activateFireEnchantment();
            yield return new WaitForSeconds(enchantmentEffectDuration);
            target.resetEnchantments();
            yield return new WaitForSeconds(enchantmentCD);
            target.resetEnchantmentCooldown();
        }
        
    }


    public IEnumerator applyIceEnchantment()
    {
        if (!target.getOnEnchantmentCD())
        {
            print("Ice enchantment activated");
            target.activateIceEnchantment();
            yield return new WaitForSeconds(enchantmentEffectDuration);
            target.resetEnchantments();
            yield return new WaitForSeconds(enchantmentCD);
            target.resetEnchantmentCooldown();
        }
    }

    public void speedBoost()
    {
        StartCoroutine(SpeedBoostCoroutine());
    }

    private IEnumerator SpeedBoostCoroutine()
    {
        if (!speedBoostOnCd)
        {
            speedBoostOnCd = true;
            target.moveSpeed = target.moveSpeed * speedBoostPower;
            yield return new WaitForSeconds(speedBoostDuration);
            target.moveSpeed = target.moveSpeed / speedBoostPower;
            yield return new WaitForSeconds(speedBoostCD);
            speedBoostOnCd = false;
        }
    }

    //Checks if enchantment spell is ready again, 10 second Cooldown (default value)
    private void CheckEnchantmentCD()
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

    //Checks remaining enchantment duration, disables enchantment after 5 seconds (default value)
    private void CheckEnchantmentDuration()
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

    public void OnNotify(string gameEvent, int cooldownIndex)
    {
        switch (gameEvent)
        {
            case "FairyCDOver":
                isOnCoolDown[cooldownIndex] = false;
                break;
        }
    }
}
