using System.Collections;
using UnityEngine;

public class RangedPlayer : Player
{
    public Rigidbody2D arrow;
    private float timeToTravel = 1f;
    private ParticleSystem buff;

    // Use this for initialization
    private void Start()
    {
        base.Start();
        this.baseDamage = 10;
        this._hitpoints = 100;
        this.attackCD = 1f;
        buff = GetComponent<ParticleSystem>();
        buff.Play();
        animator = GetComponent<Animator>();
    }

    //An IEnumerator works similar to a function in this case (Coroutine), but you can pause with a yield
    //This function generates an arrow and then checks which way it should fly depending on the direction the player is facing
    protected override IEnumerator Attack()
    {
        CheckForEnchantment();
        isAttacking = true;
        isOnCoolDown = true;
        yield return new WaitForSeconds(attackCD); //Waiting for the cooldown
        isOnCoolDown = false;
    }

    /*This function generates an arrow and then checks which way it should fly depending on the direction the player is facing
     * This function gets called from the animator (see animations events)
     */
    private void createArrow(int currentDir)
    {
        isAttacking = false;
        Rigidbody2D arrowClone = arrow.GetComponent<Arrow>().createArrow(rb2D.position, transform.rotation, timeToTravel);
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
    }
}
