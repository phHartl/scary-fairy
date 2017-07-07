using UnityEngine;

public abstract class MovingObj : MonoBehaviour
{
	public float moveSpeed;
	public LayerMask collisionLayer;
	protected Rigidbody2D rb2D;
	protected BoxCollider2D boxCollider;
	protected int _hitpoints;
	protected int _damage;
    protected bool isMoving;
    protected bool isAttacking;
    protected bool isInvincible;
    protected bool isOnCoolDown;
    protected float attackCD;
    protected bool iceEnchantment;
    protected bool fireEnchantment;
    protected bool onEnchantmentCD;
	protected Vector2 newPos;
    protected Animator animator;
    



	// Use this for initialization
	protected void Start ()
	{
		boxCollider = GetComponent<BoxCollider2D>();
		rb2D = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	protected void FixedUpdate ()
	{
		rb2D.MovePosition(Move());
	}

    protected virtual void Update ()
    {

    }

    protected virtual Vector2 Move() //Report back to Gamemanager -> Animation
	{
		return newPos;
	}

    public int getDamage()
    {
        return _damage;
    }

    public virtual void applyDamage(int damage)
    {
        _hitpoints -= damage;
        if(_hitpoints <= 0)
        {
            gameObject.SetActive(false);
        }
        print(this.name + " took damage. HP: " + _hitpoints);
    }

    public virtual void activateFireEnchantment()
    {
        iceEnchantment = false;
        fireEnchantment = true;
        onEnchantmentCD = true;
    }

    public void activateIceEnchantment()
    {
        fireEnchantment = false;
        iceEnchantment = true;
        onEnchantmentCD = true;
    }

    public void resetEnchantments()
    {
        iceEnchantment = false;
        fireEnchantment = false;
    }

    public void resetEnchantmentCooldown()
    {
        onEnchantmentCD = false;
    }

    public bool getOnEnchantmentCD()
    {
        return onEnchantmentCD;
    }
}
