using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedPlayer : Player {

    public Rigidbody2D arrow;
    private float timeToTravel = 1f;

	// Use this for initialization
	void Start () {
        base.Start();
        this._damage = 10;
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        base.Update();
        if (Input.GetKeyDown("m"))
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        base.FixedUpdate();
    }

    void Attack()
    {
        Rigidbody2D arrowClone = arrow.GetComponent<Arrow>().createArrow(rb2D.position,transform.rotation);
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
}
