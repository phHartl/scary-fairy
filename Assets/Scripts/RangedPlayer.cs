using System;
using System.Collections;
using UnityEngine;

public class RangedPlayer : Player, CooldownObserver
{
    public Rigidbody2D arrow;
    private float timeToTravel = Constants.RANGER_ARROW_TRAVEL_TIME;

    // Use this for initialization
    private void Awake()
    {
        this.baseDamage = Constants.RANGER_BASE_DAMAGE;
        this._hitpoints = Constants.PLAYER_MAX_HITPOINTS;
    }

    // Use this for initializing dependencies
    protected override void Start()
    {
        base.Start();
        isOnCoolDown = cdManager.GetRangerCooldowns();
        animator = GetComponent<Animator>();
        particles = GetComponentInChildren<ParticleSystem>();
        particleSettings = particles.main;
        particles.Stop();
        Subject.AddObserver(this);
        Subject.AddCDObserver(this);
    }

    //An IEnumerator works similar to a function in this case (Coroutine), but you can pause with a yield
    //This function generates an arrow and then checks which way it should fly depending on the direction the player is facing
    protected override void Attack()
    {
        CheckForEnchantment();
        isAttacking = true;
        isOnCoolDown[0] = true;
        cdManager.StartCooldown(0, Constants.RANGER_CLASS_INDEX);
    }

    protected override void FirstAbility()
    {
        CheckForEnchantment();
        firstAbility = true;
        isOnCoolDown[1] = true;
        cdManager.StartCooldown(1, Constants.RANGER_CLASS_INDEX);
    }

    // Revive
    protected override void SecondAbility()
    {
        Player otherPlayer = gameObject.GetComponentInParent<PlayerManager>().otherPlayer;
        if (_hitpoints < Constants.MINIMAL_HP_TO_REVIVE || !otherPlayer.isDead) return;
        isOnCoolDown[2] = true;
        cdManager.StartCooldown(2, Constants.RANGER_CLASS_INDEX);
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

    // This function gets called from the animator(see animations events) to check if it is a normal attack or not
    private void GenArrows(int currentDir)
    {
        if(firstAbility) {
            multiShot(currentDir, Constants.RANGER_MULTISHOT_ARROW_COUNT, Constants.RANGER_MULTISHOT_ANGLE);
        }
        else
        {
            multiShot(currentDir, 1, 0);
        }
    }

    /*This function generates a amount of arrow depending of which angle the player is facing
     * parameter currentDir = facing direction, parameter count = how much arrows should spawn, degree = how much should those arrows be rotated per spawn
     */
    private void multiShot(int currentDir, int count, float degree)
    {
        isAttacking = false;
        firstAbility = false;
        int arrowCount = count;
        Rigidbody2D [] arrows = new Rigidbody2D[arrowCount];
        float degrees = degree;
        for (int i = 0; i < arrowCount; i++)
        {
            arrows[i] = arrow.GetComponent<PlayerProjectile>().CreateProjectile(rb2D.position, transform.rotation, timeToTravel);
            Quaternion velocityAngle = Quaternion.Euler(0, 0, (i - arrowCount/2) * degrees);
            if (currentDir == 1)
            {
                Quaternion facingAngle = Quaternion.Euler(0, 0, 90 + (i - arrowCount/2) * degrees);
                arrows[i].transform.SetPositionAndRotation(transform.position, facingAngle);
                arrows[i].velocity = velocityAngle * transform.up * 10f;
            }
            if(currentDir == 2)
            {
                Quaternion facingAngle = Quaternion.Euler(0, 0, (i - arrowCount / 2) * degrees);
                arrows[i].transform.SetPositionAndRotation(transform.position, facingAngle);
                arrows[i].velocity = velocityAngle * transform.right * 10f;
            }
            if(currentDir == 3)
            {
                Quaternion facingAngle = Quaternion.Euler(0, 0, -90 + (i - arrowCount / 2) * degrees);
                arrows[i].transform.SetPositionAndRotation(transform.position, facingAngle);
                arrows[i].velocity = velocityAngle * -transform.up * 10f;
            }
            if(currentDir == 4)
            {
                Quaternion facingAngle = Quaternion.Euler(180, 0, 180 + (i - arrowCount / 2) * -degrees);
                arrows[i].transform.SetPositionAndRotation(transform.position, facingAngle);
                arrows[i].velocity = velocityAngle * -transform.right * 10f;
            }
        }
    }

   

    public override void OnNotify(string gameEvent, int cooldownIndex)
    {
        base.OnNotify(gameEvent, cooldownIndex);
        switch (gameEvent)
        {
            case Constants.RANGER_CD_OVER:
                isOnCoolDown[cooldownIndex] = false;
                break;
        }
    }
}
