using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedPlayer : Player
{

    public Rigidbody2D arrow;
    private float timeToTravel = 1f;

    // Use this for initialization
    void Start()
    {
        base.Start();
        this.baseDamage = 10;
        this.attackCD = 0.5f;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown("m") && !isOnCoolDown)
        {
            StartCoroutine(Attack()); //Coroutines don't need to be finished within the updateframe
        }
    }

    void FixedUpdate()
    {
        base.FixedUpdate();
    }

    //An IEnumerator works similar to a function in this case (Coroutine), but you can pause with a yield
    //This function generates an arrow and then checks which way it should fly depending on the direction the player is facing
    IEnumerator Attack()
    {
        checkForEnchantment();
        isAttacking = true;
        Rigidbody2D arrowClone = arrow.GetComponent<Arrow>().createArrow(rb2D.position, transform.rotation, timeToTravel);
        arrowClone.transform.SetParent(this.transform);
        if (currentDir == 2)
        {
            arrowClone.velocity = (transform.right * 10f);
        }
        if (currentDir == 3)
        {
            arrowClone.transform.Rotate(0, 0, -90);
            arrowClone.velocity = (-transform.up * 10f);
        }
        if (currentDir == 4)
        {
            arrowClone.transform.Rotate(180, 0, 180);
            arrowClone.velocity = (-transform.right * 10f);
        }
        if (currentDir == 1)
        {
            arrowClone.transform.Rotate(0, 0, 90);
            arrowClone.velocity = (transform.up * 10f);
        }
        isOnCoolDown = true;
        yield return new WaitForSeconds(0.25f); //Waiting for animation
        isAttacking = false; //After animation has finished, player isn't attacking anymore
        yield return new WaitForSeconds(attackCD); //Waiting for the cooldown
        isOnCoolDown = false;
    }
}
