using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGolemBoss : Npc {

    private bool activated = false;
    private AIMove moveScript;

    public GameObject projectileObject;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        this._hitpoints = 100;
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
        print("Golem invulnerable to non-ice attacks");
    }

    public override void applyDamage(int damage, string enchantment)
    {
        if (enchantment == ICE_ENCHANTMENT)
        {
            if (!activated)
            {
                activateGolem();
            }
            _hitpoints -= damage;
            checkDeath();
        }
    }

}
