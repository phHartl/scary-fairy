using System.Collections;
using UnityEngine;

public class MeleePlayer : Player {

    private BoxCollider2D[] attackColliders = new BoxCollider2D[5];

    public float knockBackStrength = 2;

    // Use this for initialization
    protected void Start()
    {
        base.Start();
        attackColliders = GetComponentsInChildren<BoxCollider2D>();
        DisableAttackColliders();
        this._hitpoints = 100;
        this.attackCD = 1f;
        this.baseDamage = 20;
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown("f") && !isOnCoolDown)
        {
            StartCoroutine(Attack()); //Coroutine is better here, an attack doesn't need to be done every frame
        }
    }

 protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (isAttacking == true && other.CompareTag("CasualEnemy"))
        {

            CasualEnemy ce = other.GetComponent<CasualEnemy>();
            ce.applyDamage(_damage);
            if (iceEnchantment)
            {
                print("IceEnchanted Attack");
            }
            if (fireEnchantment)
            {
                print("FireEnchanted Attack");
            }
            if (!iceEnchantment && !fireEnchantment)
            {
                print("normal Attack");
            }
            // knockVector = direction of knockBack times strength of knockback
            Vector2 knockVector = (ce.transform.position - this.transform.position).normalized * knockBackStrength;
            ce.knockBack(knockVector);
        }
}



    //An IEnumerator works similar to a function in this case (Coroutine), but you can pause with a yield
    //This function enables the triggers attached to the player in dependence of which direction the player is facing
    IEnumerator Attack()
{
        checkForEnchantment();
        isAttacking = true;
        attackColliders[currentDir].enabled = true;
        isOnCoolDown = true;
        yield return new WaitForSeconds(0.25f); //Wait for animation
        isAttacking = false; //After animation has finished, player isn't attacking anymore
        attackColliders[currentDir].enabled = false;
        yield return new WaitForSeconds(attackCD); //Waiting for cooldown
        isOnCoolDown = false;
    }

    private void DisableAttackColliders()
    {
        for (int i = 1; i < attackColliders.Length; i++)
        {
            attackColliders[i].enabled = false;
        }
    }

}
