using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleePlayer : Player {

    private float attackCD = 0.3f;
    private BoxCollider2D[] attackColliders = new BoxCollider2D[5];

    // Use this for initialization
    void Start () {
        base.Start();
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
        if (Input.GetKeyDown("f") && !isOnCoolDown)
        {
            StartCoroutine(Attack()); //Coroutine is better here, an attack doesn't need to be done every frame
        }
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

    //An IEnumerator works similar to a function in this case (Coroutine), but you can pause with a yield
    //This function enables the triggers attached to the player in dependence of which direction the player is facing
    IEnumerator Attack()
    {
        isAttacking = true;
        attackColliders[currentDir].enabled = true;
        isOnCoolDown = true;
        yield return new WaitForSeconds(0.25f); //Wait for animation
        isAttacking = false; //After animation has finished, player isn't attacking anymore
        attackColliders[currentDir].enabled = false;
        yield return new WaitForSeconds(attackCD); //Waiting for cooldown
        isOnCoolDown = false;
    }

    private void disableAttackColliders()
    {
        for (int i = 1; i < attackColliders.Length; i++)
        {
            attackColliders[i].enabled = false;
        }
    }
}
