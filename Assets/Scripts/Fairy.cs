using System.Collections;
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

    private void Start () {
        base.Start();
        circleCollider = GetComponent<CircleCollider2D>();
        novaAnimator = GetComponentsInChildren<Animator>()[1];
        circleCollider.enabled = false;
        this.attackCD = 2f;
        this.baseDamage = 20; //Damage of Fairy AOE Attack
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

    protected override IEnumerator Attack()
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
    public void EnchantAttacks(bool isIceEnchant)
    {
        if (!target.getOnEnchantmentCD()) {
            if (isIceEnchant)
            {
                enchantmentEffectTimer = enchantmentEffectDuration;
                enchantmentTimer = enchantmentCD;
                target.activateIceEnchantment();
            } else {
                enchantmentEffectTimer = enchantmentEffectDuration;
                enchantmentTimer = enchantmentCD;
                target.activateFireEnchantment();
            }
        }
        CheckEnchantmentCD();
        CheckEnchantmentDuration();
    }

    //Checks if enchantment spell is ready again, 10 second Cooldown (default value)
    private void CheckEnchantmentCD()
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

    //Checks remaining enchantment duration, disables enchantment after 5 seconds (default value)
    private void CheckEnchantmentDuration()
    {
        if(enchantmentEffectTimer > 0)
        {
            enchantmentEffectTimer -= Time.deltaTime;
        }
        else
        {
            target.resetEnchantments();
        }
    }
}
