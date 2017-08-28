using System;
using System.Collections;
using UnityEngine;

public class Fairy : Player, CooldownObserver {

    public Player target;
    public Vector3 FAIRY_DISTANCE;                  //Distance between fairy and other player

    private float speedBoostPower = 1.5f;
    protected CircleCollider2D circleCollider;
    private Animator novaAnimator;
 

    protected override void Start () {
        base.Start();
        isOnCoolDown = cdManager.GetFairyCooldown();
        circleCollider = GetComponent<CircleCollider2D>();
        novaAnimator = GetComponentsInChildren<Animator>()[1];
        circleCollider.enabled = false;
        this.baseDamage = 20; //Damage of Fairy AOE Attack
    }

    private void OnDestroy()
    {
        target.resetEnchantments();
    }

    protected override void FixedUpdate()
    {
        
    }

    protected override void Update()
    {
        //animator.SetFloat("MoveX", Input.GetAxisRaw(target.GetComponent<Player>().axisHorizontal));
        //animator.SetFloat("MoveY", Input.GetAxisRaw(target.GetComponent<Player>().axisVertical));
        animator.SetFloat("LastMoveX", target.GetComponent<Player>().lastMove.x);
        animator.SetFloat("LastMoveY", target.GetComponent<Player>().lastMove.y);
        novaAnimator.SetBool("NovaAttack", isAttacking);
    }

    private void LateUpdate () {
        transform.position = target.transform.position + FAIRY_DISTANCE;
    }

    protected override void Attack()
    {
        _damage = baseDamage;
        isAttacking = true;
        circleCollider.enabled = true;
        isOnCoolDown[0] = true;
        cdManager.StartCooldown(0, 2);
    }

    //Called trough child object because of how an animation event works
    public void AttackOver()   
    {
        isAttacking = false;
        circleCollider.enabled = false;
    }

    protected override void FirstAbility()
    {
        if (!target.getOnEnchantmentCD())
        {
            isOnCoolDown[1] = true;
            cdManager.StartCooldown(1, 3);
            cdManager.StartCooldown(1, 2);
            target.activateFireEnchantment();
        }
    }


    protected override void SecondAbility()
    {
        if (!target.getOnEnchantmentCD())
        {
            isOnCoolDown[2] = true;
            cdManager.StartCooldown(2, 3);
            cdManager.StartCooldown(2, 2);
            target.activateIceEnchantment();
        }
    }

    protected override void ThirdAbility()
    {
        isOnCoolDown[3] = true;
        cdManager.StartCooldown(3, 3);
        cdManager.StartCooldown(3, 2);
        target.moveSpeed = target.moveSpeed * speedBoostPower;
    }

    public void OnNotify(string gameEvent, int cooldownIndex)
    {
        switch (gameEvent)
        {
            case "FairyCDOver":
                isOnCoolDown[cooldownIndex] = false;
                break;
        }
    }
}
