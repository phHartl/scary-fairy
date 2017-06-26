using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedPlayer : Player {

    public Rigidbody2D arrow;
    private float timeToTravel = 1f;
    private float attackCD = 0.01f;
    private float attackTimer = 0;

    // Use this for initialization
    void Start () {
        base.Start();
        this._damage = 10;
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        base.Update();
        Attack();
    }

    void FixedUpdate()
    {
        base.FixedUpdate();
    }

    void Attack()
    {
        if (Input.GetKeyDown("m") && !isAttacking)
        {
            isAttacking = true;
            attackTimer = attackCD;
        }
        if (isAttacking)
        {
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
                Rigidbody2D arrowClone = arrow.GetComponent<Arrow>().createArrow(rb2D.position, transform.rotation);
                arrowClone.transform.SetParent(this.transform);
                if (currentDir == 2)
                {
                    arrowClone.velocity = (transform.right * 10f);
                }
                if (currentDir == 3)
                {
                    arrowClone.velocity = (-transform.up * 10f);
                }
                if (currentDir == 4)
                {
                    arrowClone.velocity = (-transform.right * 10f);
                }
                if (currentDir == 1)
                {
                    arrowClone.velocity = (transform.up * 10f);
                }
                Destroy(arrowClone.gameObject, timeToTravel);
            }
            else
            {
                isAttacking = false;
            }
        }
    }
}
