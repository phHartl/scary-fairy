using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBoss : Npc, IObserver
{

    private bool isBurning = false;
    private bool durationRefreshed = false;
    private bool activated = false;

    Animator animator;
    PolygonCollider2D damageTrigger;
    AIMove moveScript;

    // Use this for initialization
    void Start ()
    {
        base.Start();
        Subject.AddObserver(this);
        this._hitpoints = 500;
        this._damage = 0;
        setupGolem();
    }


    private void setupGolem()
    {
        animator = GetComponent<Animator>();
        moveScript = GetComponent<AIMove>();
        animator.speed = 0;
        rb2D.bodyType = RigidbodyType2D.Static;
        moveScript.canMove = false;
        moveScript.canSearch = false;
    }

    private void activateGolem()
    {
        animator.speed = 1;
        this._damage = 30;
        rb2D.bodyType = RigidbodyType2D.Dynamic;
        moveScript.canMove = true;
        moveScript.canSearch = true;
    }

    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();
	}

    public override void applyDamage(int damage)
    {
        print("Golem invulnerable to non-fire attacks");
    }

    public override void applyDamage(int damage, string enchantment)
    {
        if (enchantment == FIRE_ENCHANTMENT)
        {
            if (!activated)
            {
                Subject.Notify("GolemActivated");
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

    public void OnNotify(String gameEvent)
    {
        switch (gameEvent)
        {
            case "GolemActivated":
                activateGolem();
                break;
        }
    }
}
