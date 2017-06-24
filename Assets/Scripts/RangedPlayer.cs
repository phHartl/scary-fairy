using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedPlayer : Player {

    public Rigidbody2D arrow;

	// Use this for initialization
	void Start () {
        base.Start();
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}

    void FixedUpdate()
    {
        base.FixedUpdate();
        if (Input.GetKeyDown("m"))
        {
            Attack();
        }
    }

    void Attack()
    {
        Rigidbody2D arrowClone;
        arrowClone = Instantiate(arrow, transform.position, transform.rotation) as Rigidbody2D;
        BoxCollider2D arrowCollider = arrowClone.GetComponent<BoxCollider2D>();
        arrowClone.transform.parent = transform;
        arrowCollider.enabled = true;
        if (currentDir == 2)
        {
            arrowClone.velocity = transform.right * 10f;
        }
        if (currentDir == 3)
        {
            arrowClone.velocity = -transform.up * 10f;
        }
        if (currentDir == 4)
        {
            arrowClone.velocity = -transform.right * 10f;
        }
        if (currentDir == 1)
        {
            arrowClone.velocity = transform.up * 10f;
        }
    }
}
