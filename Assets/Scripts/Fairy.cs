using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fairy : Player {

    public MovingObj target;
    public Vector3 FAIRY_DISTANCE;                  //Distance between fairy and other player
    private float enchantmentCD = 10f;              //Duration between enchantments
    private float enchantmentEffectTimer = 0;
    private float enchantmentEffectDuration = 5f;   //Duration of a single enchantment spell
    private float enchantmentTimer = 0;
    protected CircleCollider2D circleCollider;
    private Animator novaAnimator;
    public ParticleSystem buff;
 

 
    void Start () {
        circleCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        novaAnimator = GetComponentsInChildren<Animator>()[1];
        circleCollider.enabled = false;
        this.attackCD = 2f;
        this.baseDamage = 20; //Damage of Fairy AOE Attack
    }

    protected override void Update()
    {
        animator.SetFloat("MoveX", Input.GetAxisRaw(target.GetComponent<Player>().axisHorizontal));
        animator.SetFloat("MoveY", Input.GetAxisRaw(target.GetComponent<Player>().axisVertical));
        animator.SetFloat("LastMoveX", target.GetComponent<Player>().lastMove.x);
        animator.SetFloat("LastMoveY", target.GetComponent<Player>().lastMove.y);
        novaAnimator.SetBool("NovaAttack", isAttacking);
        if (Input.GetKeyDown("q") && !isOnCoolDown)
        {
            StartCoroutine(Attack());
        }
        ChangeClassInput();
    }


    void LateUpdate () {
        transform.position = target.transform.position + FAIRY_DISTANCE;
        enchantAttacks();
    }
    //copy pasta MeleePlayer
    IEnumerator Attack()
    {
        _damage = baseDamage;
        isAttacking = true;
        circleCollider.enabled = true;
        isOnCoolDown = true;
        yield return new WaitForSeconds(0.25f);
        isAttacking = false;
        circleCollider.enabled = false;
        yield return new WaitForSeconds(attackCD);
        isOnCoolDown = false;
     }

    /*
     * Press Button "1" to enchant the other player with ice
     * Press Button "2" to enchant the other player with fire
     * Fire deals more damage than ice, ice does currently not have any extra effects. Maybe add slow.
    */
    private void enchantAttacks()
    {
        if (Input.GetKeyDown("1") && !target.getOnEnchantmentCD())
        {
            buff.Play();
            enchantmentEffectTimer = enchantmentEffectDuration;
            enchantmentTimer = enchantmentCD;
            target.activateIceEnchantment();
        }
        if (Input.GetKeyDown("2") && !target.getOnEnchantmentCD()){
            enchantmentEffectTimer = enchantmentEffectDuration;
            enchantmentTimer = enchantmentCD;
            target.activateFireEnchantment();
        }
        checkEnchantmentCD();
        checkEnchantmentDuration();
    }

    //Checks remaining enchantment duration, disables enchantment after 5 seconds (default value)
    private void checkEnchantmentDuration()
    {
        if(enchantmentEffectTimer > 0)
        {
            enchantmentEffectTimer -= Time.deltaTime;
        }
        else
        {
            target.resetEnchantments();
            buff.Stop();

        }
    }

    private void FixedUpdate() { }


    //Checks if enchantment spell is ready again, 10 second Cooldown (default value)
    private void checkEnchantmentCD()
    {
        if (target.getOnEnchantmentCD())
        {
            if (enchantmentTimer > 0)
            {
                enchantmentTimer -= Time.deltaTime;
            }
            else
            {
                target.resetEnchantmentCooldown();
            }
        }
    }
}
