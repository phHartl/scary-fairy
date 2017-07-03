using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleePlayer : Player {

    private float attackCD = 0.3f;
    private float attackTimer = 0;
    private BoxCollider2D[] attackColliders = new BoxCollider2D[5];

    // Use this for initialization
    void Start () {
        base.Start();
        SetAxis();
        animator = GetComponent<Animator>();
        attackColliders = GetComponentsInChildren<BoxCollider2D>();
        disableAttackColliders();
        this._hitpoints = 100;
        this._damage = 20;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        base.FixedUpdate();
	}

    protected override void Update()
    {
        base.Update();
        Attack();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isAttacking == true && other.CompareTag("CasualEnemy"))
        {
            CasualEnemy ce = other.GetComponent<CasualEnemy>();
            ce.applyDamage(_damage);
            print("Enemy attacked");
        }
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
}
