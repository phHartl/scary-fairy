using System.Collections;
using UnityEngine;

public class MeleePlayer : Player
{
    private BoxCollider2D[] attackColliders = new BoxCollider2D[5];
    private AudioSource sound;

    public int knockBackLength = 2;

    // Use this for initialization
    private void Start()
    {
        base.Start();
        attackColliders = GetComponentsInChildren<BoxCollider2D>();
        sound = GameObject.FindObjectOfType<AudioSource>();
        particles = GetComponentInChildren<ParticleSystem>();
        particleSettings = particles.main;
        particles.Stop();
        DisableAttackColliders();
        this.attackCD = 1f;
        this._hitpoints = 100;
        this.baseDamage = 20;
    }

 protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (isAttacking && other.CompareTag("CasualEnemy"))
        {

            CasualEnemy ce = other.GetComponent<CasualEnemy>();
            AIMove ai = other.GetComponent<AIMove>();
            if (iceEnchantment)
            {
                ce.applyDamage(_damage);
                ai.hitByIceEnchantment();
                print("IceEnchanted Attack");
            }
            if (fireEnchantment)
            {
                ce.applyDamage(_damage, FIRE_ENCHANTMENT);
                print("FireEnchanted Attack");
            }
            if (!iceEnchantment && !fireEnchantment)
            {
                ce.applyDamage(_damage);
                print("normal Attack");
            }
            // knockVector = direction of knockBack times strength of knockback
            Vector2 knockVector = (ce.transform.position - this.transform.position).normalized * knockBackLength;
            ce.knockBack(knockVector,rb2D.mass);
        }
    }

    //An IEnumerator works similar to a function in this case (Coroutine), but you can pause with a yield
    //This function enables the triggers attached to the player in dependence of which direction the player is facing
    protected override IEnumerator Attack()
    {
        CheckForEnchantment();
        isAttacking = true;
        isOnCoolDown = true;
        yield return new WaitForSeconds(attackCD); //Waiting for cooldown
        isOnCoolDown = false;
    }

    //This function gets called when the attack animation starts (see animations events)
    private void EnableAttackCollider(int currentDir)
    {
        attackColliders[currentDir].enabled = true;
    }

    private void DisableAttackColliders()
    {
        for (int i = 1; i < attackColliders.Length; i++)
        {
            attackColliders[i].enabled = false;
        }
    }
    //This function gets called when the attack animations ends
    private void DisableAttackCollider(int currentDir)
    {
        attackColliders[currentDir].enabled = false;
        isAttacking = false;
    }

}
