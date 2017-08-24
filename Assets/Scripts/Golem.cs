using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Npc {

    private bool activated = false;
    private AIMove moveScript;
    public string vulnerableEnchantment;

    public GameObject projectileObject;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        this._hitpoints = 250;
        this._damage = 0;
        setupGolem();
    }


    private void setupGolem()
    {
        animator = GetComponent<Animator>();
        moveScript = GetComponent<AIMove>();
        animator.speed = 0;
        rb2D.bodyType = RigidbodyType2D.Static;
        rb2D.freezeRotation = true;
        moveScript.canMove = false;
        moveScript.canSearch = false;
    }

    private void activateGolem()
    {
        activated = true;
        animator.speed = 1;
        this._damage = 30;
        rb2D.bodyType = RigidbodyType2D.Dynamic;
        moveScript.canMove = true;
        moveScript.canSearch = true;
        StartCoroutine(startProjectileAttack());
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected IEnumerator startProjectileAttack()
    {
        while (_hitpoints > 0)
        {
            GameObject projectileClone = projectileObject;
            projectileClone.transform.position = this.transform.position;
            Instantiate(projectileClone);
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

    private IEnumerator applyBurnDamage()
    {
        isBurning = true;
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        for (int i = 0; i < BURN_DAMAGE_DURATION; i++)
        {
            if (durationRefreshed)
            {
                print("Burn Refreshed");
                i = 0;
                durationRefreshed = false;
            }
            _hitpoints -= 2;
            print("Enemy got burned");
            yield return new WaitForSeconds(BURN_TICKRATE);
        }
        isBurning = false;
        GetComponent<Renderer>().material.color = Color.white;
    }
}
