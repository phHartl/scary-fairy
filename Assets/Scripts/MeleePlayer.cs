using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleePlayer : Player {

    private BoxCollider2D[] attackColliders = new BoxCollider2D[5];

    // Use this for initialization
    void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        attackColliders = GetComponentsInChildren<BoxCollider2D>();
        disableAttackColliders();
        this._hitpoints = 100;
        this.attackCD = 0.5f;
        this.attackTimer = 0;
        this._damage = 20;
    }

    protected override void Update()
    {
        base.Update();
        checkForEnchantment();
        Attack();
    }


    private void Attack()
    {
        if (Input.GetKeyDown("f") && !isAttacking)
        {
            isAttacking = true;
            attackTimer = attackCD;
        }
        if (isAttacking)
        {
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
                attackColliders[currentDir].enabled = true;
            }
            else
            {
                isAttacking = false;
                attackColliders[currentDir].enabled = false;
            }
        }
    }

    private void disableAttackColliders()
    {
        for (int i = 1; i < attackColliders.Length; i++)
        {
            attackColliders[i].enabled = false;
        }
    }


    //Checks if player is enchanted by fairy and calculates new attack damage
    private void checkForEnchantment()
    {
        _damage = 20;
        if(iceEnchantment)
        {
            _damage = _damage * 2;
        }
        else if (fireEnchantment)
        {
            _damage = _damage * 3;
        }
    }
}
