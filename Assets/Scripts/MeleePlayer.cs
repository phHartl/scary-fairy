using System;
using System.Collections;
using UnityEngine;

public class MeleePlayer : Player, IObserver, CooldownObserver
{
    private BoxCollider2D[] attackColliders = new BoxCollider2D[5];
    private AudioSource sound;
    private float damageReduce = Constants.WARRIOR_BASE_DAMAGE_REDUCTION; //How much should the damage be reduced?
    private float defensiveStateDuration = Constants.WARRIOR_SHIELD_DURATION; //Duration of buff


    public int knockBackLength = 2;

    // Use this for initialization
    private void Awake()
    {
        this._hitpoints = Constants.PLAYER_MAX_HITPOINTS;
        this.baseDamage = Constants.WARRIOR_BASE_DAMAGE;
    }

    // use this for initializing dependencies
    protected override void Start()
    {
        base.Start();
        isOnCoolDown = cdManager.GetWarriorCooldowns();
        attackColliders = GetComponentsInChildren<BoxCollider2D>();
        sound = GetComponent<AudioSource>();
        particles = GetComponentInChildren<ParticleSystem>();
        particleSettings = particles.main;
        particles.Stop();
        DisableAttackColliders();
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
        if (other.CompareTag(Constants.CASUAL_ENEMY) && isAttacking)
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
        cdManager.StartCooldown(0, Constants.WARRIOR_CLASS_INDEX);
    }

    // Revive
    protected override void SecondAbility()
    {
        Player otherPlayer = gameObject.GetComponentInParent<PlayerManager>().otherPlayer;
        if ( _hitpoints < Constants.MINIMAL_HP_TO_REVIVE || !otherPlayer.isDead) return;
        isOnCoolDown[2] = true;
        cdManager.StartCooldown(2, Constants.WARRIOR_CLASS_INDEX);
        otherPlayer.applyHealing(_hitpoints / 2);
        _hitpoints /= 2;
    }

    public override void applyHealing(int healpoints)
    {
        if (isDead)
        {
            isDead = false;
            rb2D.simulated = true;
        }
        _hitpoints += healpoints;
    }

    protected override void FirstAbility()
    {
        StartCoroutine(DefensiveState());
        isOnCoolDown[1] = true;
        cdManager.StartCooldown(1, Constants.WARRIOR_CLASS_INDEX);
    }

    private IEnumerator DefensiveState()
    {
        firstAbility = true;
        moveSpeed *= Constants.WARRIOR_DEFENSIVE_DEBUFF;
        cdManager.SetWarriorCooldowns(0, (1 / Constants.WARRIOR_DEFENSIVE_DEBUFF));
        damageReduce = Constants.WARRIOR_SHIELD_DAMAGE_REDUCTION;
        yield return new WaitForSeconds(defensiveStateDuration);
        moveSpeed *= 1 / Constants.WARRIOR_DEFENSIVE_DEBUFF;
        cdManager.SetWarriorCooldowns(0, Constants.WARRIOR_DEFENSIVE_DEBUFF);
        damageReduce = Constants.WARRIOR_BASE_DAMAGE_REDUCTION;
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
            case Constants.HEALTH_PICKUP:
                _hitpoints += Constants.HEALTH_POTION_RECOVERY;
                if (_hitpoints > Constants.PLAYER_MAX_HITPOINTS)
                {
                    _hitpoints = Constants.PLAYER_MAX_HITPOINTS;
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
            case Constants.WARRIOR_CD_OVER:
                isOnCoolDown[cooldownIndex] = false;
                break;
        }
    }
}
