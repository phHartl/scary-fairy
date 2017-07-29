using System.Collections;
using UnityEngine;

public class MeleePlayer : Player, IObserver
{
    private BoxCollider2D[] attackColliders = new BoxCollider2D[5];
    private AudioSource sound;
    private float damageReduce = 0; //How much should the damage be reduced?
    private float defensiveStateDuration = 5f; //Duration of buff
    public float defensiveDebuff = 0.5f; //Factor to debuff other values
    


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
        abilityCDs[0] = 5f;
        this._hitpoints = 50;
        this.baseDamage = 20;
        Subject.AddObserver(this);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.CompareTag("CasualEnemy") && isAttacking)
        {
            Npc ce = other.GetComponent<Npc>();
            // knockVector = direction of knockBack times strength of knockback
            Vector2 knockVector = (ce.transform.position - this.transform.position).normalized * knockBackLength;
            ce.knockBack(knockVector, rb2D.mass);
        }
    }

    //An IEnumerator works similar to a function in this case (Coroutine), but you can pause with a yield
    //This function enables the triggers attached to the player in dependence of which direction the player is facing
    protected override IEnumerator Attack()
    {
        CheckForEnchantment();
        isAttacking = true;
        isOnCoolDown[0] = true;
        sound.Play();
        yield return new WaitForSeconds(attackCD); //Waiting for cooldown
        isOnCoolDown[0] = false;
    }

    protected override IEnumerator FirstAbility()
    {
        firstAbility = true;
        DefensiveState(firstAbility);
        isOnCoolDown[1] = true;
        yield return new WaitForSeconds(abilityCDs[0]);
        firstAbility = false;
        DefensiveState(firstAbility);
        yield return new WaitForSeconds(defensiveStateDuration);
        isOnCoolDown[1] = false;
    }

    private void DefensiveState(bool isDefensive)
    {
        if (isDefensive)
        {
            moveSpeed *= defensiveDebuff;
            attackCD *= 1 / defensiveDebuff;
            damageReduce = 0.8f;
            return;
        }
        else
        {
            moveSpeed *= 1 / defensiveDebuff;
            attackCD *= defensiveDebuff;
            damageReduce = 0;
        }
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

    public void OnNotify(string gameEvent)
    {
        switch (gameEvent)
        {
            case "HealthPickup":
                print("procced");
                _hitpoints += 5;
                if(_hitpoints > 100)
                {
                    _hitpoints = 100;
                }
                break;
        }
    }

    public override void applyDamage(int damage)
    {
        damage = Mathf.RoundToInt(damage * (1- damageReduce));
        base.applyDamage(damage);
    }

}
