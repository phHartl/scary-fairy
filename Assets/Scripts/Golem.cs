using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Npc {

    private bool activated = false;
    private AIMove moveScript;
    private Rigidbody2D rigid;
    public string vulnerableEnchantment;

    public EnemyProjectile projectileObject;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        this._hitpoints = 250;
        this._damage = 0;
        setupGolem();
    }

    /*
     *  This method sets up the golem in its frozen state. It makes the golem deal no damage
     *  and unable to move before being activated.
     */
    private void setupGolem()
    {
        animator = GetComponent<Animator>();
        moveScript = GetComponent<AIMove>();
        animator.speed = 0;
        rb2D.bodyType = RigidbodyType2D.Static;
        rb2D.freezeRotation = true;
        moveScript.canMove = false;
        moveScript.canSearch = false;
        rb2D.mass = 5000;
    }

    /*
     * This method activates the golem upon being hit by an attack with the enchantment specified in the
     * VULNERABLE_ENCHANTMENT constant. After being hit, the golem can move, deal damage on contact and shoot
     * projectiles via the startProjectileAttack() Coroutine
     */

    private void activateGolem()
    {
        activated = true;
        animator.speed = 1;
        this._damage = 30;
        moveScript.canSearch = true;
        moveScript.canMove = true;
        StartCoroutine(startProjectileAttack());
        rb2D.bodyType = RigidbodyType2D.Dynamic;
        rb2D.mass = 1;
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    /*
     * Instantiates a projectile object every 5 seconds and sets its position to the golems current position
     */
    protected IEnumerator startProjectileAttack()
    {
        while (_hitpoints > 0)
        {
            Rigidbody2D projectileClone = projectileObject.CreateProjectile(transform.position, transform.rotation, UnityEngine.Random.Range(2f, 5f));
            yield return new WaitForSeconds(5f);
        }
    }

    public override void applyDamage(int damage)
    {
        print("immune");
    }

    public override void applyDamage(int damage, string enchantment)
    {
        if(enchantment == vulnerableEnchantment)
        {
            switch (enchantment)
            {
                case "FIRE_ENCHANTMENT":
                    print("I faild this city");
                    applyFireDamage(damage);
                    break;

                case "ICE_ENCHANTMENT":
                    applyIceDamage(damage);
                    break;
            }
        }
    }

    private void applyIceDamage(int damage)
    {
        if (!activated && vulnerableEnchantment.Equals(ICE_ENCHANTMENT))
        {
            activateGolem();
        }
        _hitpoints -= damage;
        checkDeath();
    }

    private void applyFireDamage(int damage)
    {
        if (!activated && vulnerableEnchantment.Equals(FIRE_ENCHANTMENT))
        {
            activateGolem();
        }
        _hitpoints -= damage;
        checkDeath();
        print("Enemy took damage, health: " + _hitpoints);


        if (!isBurning)
        {
            StartCoroutine(applyBurnDamage());
        }
        if (isBurning)
        {
            durationRefreshed = true;
        }
    }

}
