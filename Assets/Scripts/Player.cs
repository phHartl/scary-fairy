using System;
using System.Collections;
using UnityEngine;

public class Player : MovingObj, CooldownObserver
{
    public float maxVerticalDistance = 8.0f;
    public float maxHorizontalDistance = 12.0f;
    public float currentDistance;
    private Vector2 horizontalMovement;
    private Vector2 verticalMovement;
    public string axisVertical;
    public string axisHorizontal;
    [HideInInspector]
    public Vector2 lastMove;
    protected int baseDamage;
    protected bool firstAbility;
    protected int currentDir; // Current facing direction north(1), east(2), south(3), west(4)
    public CooldownManager cdManager;
    public bool isDead;

    // Use this for initialization
    private void Awake()
    {
        isDead = false;
    }

    protected override void Start()
    {
        base.Start();
        cdManager = GetComponentInParent<CooldownManager>();
        lastMove.x = 0;
        lastMove.y = -1;
        animator = GetComponent<Animator>();
        onEnchantmentCD = cdManager.GetBuffCooldown();
    }

    protected override void Update()
    {
        animator.SetFloat("MoveX", Input.GetAxisRaw(axisHorizontal));
        animator.SetFloat("MoveY", Input.GetAxisRaw(axisVertical));
        animator.SetBool("PlayerMoving", isMoving);
        animator.SetFloat("LastMoveX", lastMove.x);
        animator.SetFloat("LastMoveY", lastMove.y);
        animator.SetBool("PlayerAttack", isAttacking);
        animator.SetInteger("Hitpoints", _hitpoints);
    }

    protected virtual void FixedUpdate()
    {
        rb2D.MovePosition(Move());
    }

    protected override Vector2 Move()
    {
        isMoving = false;
        horizontalMovement = new Vector2(0, 0);
        verticalMovement = new Vector2(0, 0);
        float axisH = Input.GetAxisRaw(axisHorizontal);
        float axisV = Input.GetAxisRaw(axisVertical);
        float nextDistance;

        if (axisH > 0.5f || axisH < -0.5f)
        {
            currentDistance = rb2D.position.x - CameraControl.camPosition.x;
            nextDistance = Vector2.Distance(rb2D.position + new Vector2(axisH * moveSpeed * Time.deltaTime, 0),
                CameraControl.camPosition);

            if (Mathf.Abs(currentDistance) < maxHorizontalDistance || (currentDistance > 0 && axisH < 0) || (currentDistance < 0 && axisH > 0))
            {
                horizontalMovement = new Vector2(axisH * moveSpeed * Time.deltaTime, 0);
                isMoving = true;
                lastMove = new Vector2(axisH, 0);
                if (axisH > 0)
                {
                    currentDir = 2;
                }
                else
                {
                    currentDir = 4;
                }
            }
        }

        if (axisV > 0.5f || axisV < -0.5f)
        {
            currentDistance = rb2D.position.y - CameraControl.camPosition.y;
            //movement possible if player below distance threshold or moving towards other player
            if (Mathf.Abs(currentDistance) < maxVerticalDistance || (currentDistance > 0 && axisV < 0) || (currentDistance < 0 && axisV > 0))
            {
                verticalMovement = new Vector2(0, axisV * moveSpeed * Time.deltaTime);
                isMoving = true;
                lastMove = new Vector2(0, axisV);
                if (axisV > 0)
                {
                    currentDir = 1;
                }
                else
                {
                    currentDir = 3;
                }
            }
        }
        newPos = rb2D.position + horizontalMovement + verticalMovement;
        Debug.DrawLine(rb2D.position, rb2D.position + lastMove);
        return newPos;
    }

    public void AttemptAttack()
    {
        if (!isOnCoolDown[0])
        {
            Attack();
        }
    }

    public void AttemptSpecialAbility()
    {
        if(!isOnCoolDown[1])
            FirstAbility();
    }

    public void AttemptSecondSpecialAbility()
    {
        if (!isOnCoolDown[2])
            SecondAbility();
    }

    public void AttemptThirdSpecialAbility()
    {
        if (!isOnCoolDown[3])
            ThirdAbility();
    }

    // This method is needed in order to call Attack from the PlayerManager in a secure way
    protected virtual void Attack()
    {
        return;
    }

    protected virtual void FirstAbility()
    {
        return;
    }

    protected virtual void SecondAbility()
    {
        return;
    }

    protected virtual void ThirdAbility()
    {
        return;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (isAttacking == true && other.CompareTag(Constants.CASUAL_ENEMY))
        {
            CalcEnemyDamage(other);
        }
    }

    public void CalcEnemyDamage(Collider2D other) //This methode can be called by projectiles, removed duplicate code in arrow and meleeplayer
    {
        CasualEnemy ce = other.GetComponent<CasualEnemy>();
        AIMove ai = other.GetComponent<AIMove>();
        CheckForEnchantment();
        if (iceEnchantment)
        {
            ce.applyDamage(_damage);
            ai.hitByIceEnchantment();
        }
        if (fireEnchantment)
        {
            ce.applyDamage(_damage, Constants.FIRE_ENCHANTMENT);
        }
        if (!iceEnchantment && !fireEnchantment)
        {
            ce.applyDamage(_damage);
        }
    }

    //Overrides applyDamage in MovingObj, player gets invincible for 0.5 seconds if hit by an enemy
    public override void applyDamage(int damage)
    {
        if (!isInvincible && !isDead)
        {
            _hitpoints -= damage;
            if(_hitpoints <= 0)
            {
                _hitpoints = 0;
                isDead = true;
                rb2D.simulated = false;
                Subject.Notify(Constants.PLAYER_DIED);
            }
            StartCoroutine(PlayerInvincible());
        }
    }


       IEnumerator PlayerInvincible()
    {
        isInvincible = true;
        SetPlayerTransparency(0.5f); // 50% transparent
        yield return new WaitForSeconds(0.5f);
        SetPlayerTransparency(1.0f);
        isInvincible = false;
    }


    //This methodes makes the player transparent, input variable is transparency in percent
    private void SetPlayerTransparency(float alpha)
    {
        gameObject.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, alpha);
    }

    public virtual void applyHealing(int healpoints)
    {

    }


    protected void CheckForEnchantment()
    {
        _damage = baseDamage;
        if (iceEnchantment)
        {
            _damage = baseDamage * Constants.ICE_ENCHANTMENT_DAMAGE_MULTIPLIER;
        }
        else if (fireEnchantment)
        {
            _damage = baseDamage * Constants.FIRE_ENCHANTMENT_DAMAGE_MULTIPLIER;
        }
    }

    public virtual void OnNotify(string gameEvent, int cooldownIndex)
    {
        switch (gameEvent)
        {
            case Constants.BUFF_OVER:
                if (this != null)
                {
                    resetEnchantments();
                    moveSpeed = Constants.PLAYER_DEFAULT_MOVEMENTSPEED;
                    onEnchantmentCD = cdManager.GetBuffCooldown();
                }
                break;
        }
    }
}
