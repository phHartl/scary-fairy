using System;
using System.Collections;
using UnityEngine;

public class MeleePlayer : Player, IObserver, CooldownObserver
{
    private BoxCollider2D[] attackColliders = new BoxCollider2D[5];
    private AudioSource sound;
    private float damageReduce = 0; //How much should the damage be reduced?
    private float defensiveStateDuration = 5f; //Duration of buff
    public float defensiveDebuff = 0.5f; //Factor to debuff other values



    public int knockBackLength = 2;

    // Use this for initialization
    private void Awake()
    {
        this._hitpoints = 100;
        this.baseDamage = 20;
    }

    // use this for initializing dependencies
    protected override void Start()
    {
        base.Start();
        isOnCoolDown = cdManager.GetWarriorCooldowns();
        attackColliders = GetComponentsInChildren<BoxCollider2D>();
        sound = GameObject.FindObjectOfType<AudioSource>();
        particles = GetComponentInChildren<ParticleSystem>();
        particleSettings = particles.main;
        particles.Stop();
        DisableAttackColliders();
        this._hitpoints = 50;
        this.baseDamage = 20;
        Subject.AddObserver(this);
        Subject.AddCDObserver(this);
    }

    protected override void Update()
    {
        base.Update();
        animator.SetBool("ShieldUp", firstAbility);
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

    protected override void Attack()
    {
        CheckForEnchantment();
        isAttacking = true;
        isOnCoolDown[0] = true;
        sound.Play();
        //Start corresponding cooldown -> first parameter is cd index (zero is basic attack) and second parameter is classindex for warrior
        cdManager.StartCooldown(0, 0);
    }

    // Revive
    protected override void SecondAbility()
    {
        Player otherPlayer = gameObject.GetComponentInParent<PlayerManager>().otherPlayer;
        if ( _hitpoints < 25 || !otherPlayer.isDead) return;
        isOnCoolDown[2] = true;
        cdManager.StartCooldown(2, 0);
        otherPlayer.applyHealing(_hitpoints / 2);
        _hitpoints /= 2;
    }

    public override void applyHealing(int healpoints)
    {
        if (isDead)
        {
            isDead = false;
            moveSpeed = 5;
            rb2D.simulated = true;
        }
        _hitpoints += healpoints;
    }

    protected override void FirstAbility()
    {
        StartCoroutine(DefensiveState());
        isOnCoolDown[1] = true;
        cdManager.StartCooldown(1, 0);
    }

    private IEnumerator DefensiveState()
    {
        firstAbility = true;
        moveSpeed *= defensiveDebuff;
        cdManager.SetWarriorCooldowns(0, (1 / defensiveDebuff));
        damageReduce = 0.8f;
        yield return new WaitForSeconds(defensiveStateDuration);
        moveSpeed *= 1 / defensiveDebuff;
        cdManager.SetWarriorCooldowns(0, defensiveDebuff);
        damageReduce = 0;
        firstAbility = false;
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
                if (_hitpoints > 100)
                {
                    _hitpoints = 100;
                }
                break;
        }
    }

    public override void applyDamage(int damage)
    {
        damage = Mathf.RoundToInt(damage * (1 - damageReduce));
        base.applyDamage(damage);
    }

    public override void OnNotify(string gameEvent, int cooldownIndex)
    {
        base.OnNotify(gameEvent, cooldownIndex);
        switch (gameEvent)
        {
            case "WarriorCDOver":
                isOnCoolDown[cooldownIndex] = false;
                break;
        }
    }
}
