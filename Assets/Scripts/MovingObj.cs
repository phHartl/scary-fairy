using UnityEngine;

public abstract class MovingObj : MonoBehaviour
{
    public float moveSpeed = Constants.PLAYER_DEFAULT_MOVEMENTSPEED;
    public LayerMask collisionLayer;
    protected Rigidbody2D rb2D;
    protected BoxCollider2D boxCollider;
    public int _hitpoints;
    protected int _damage;
    protected bool isMoving;
    protected bool isAttacking;
    protected bool isInvincible;
    protected bool[] isOnCoolDown = new bool[4]; //Cooldowns for four abilites (0 is attack, 1 first ability, 2 second ability, 3 third ability)
    protected bool onEnchantmentCD;
    protected Vector2 newPos;
    protected Animator animator;
    protected ParticleSystem particles;
    protected ParticleSystem.MainModule particleSettings;
    private Color blue = new Color(0.1f, 0.3f, 1.0f);
    private Color red = new Color(1.0f, 0.3f, 0.1f);


    public bool iceEnchantment;
    public bool fireEnchantment;


    // Use this for initialization
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }


    protected virtual void Update()
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
        print(this.name + " took damage. HP: " + _hitpoints);
    }

    public virtual void activateFireEnchantment()
    {
        iceEnchantment = false;
        fireEnchantment = true;
        onEnchantmentCD = true;
        particleSettings.startColor = red;
        particles.Play();
    }

    public void activateIceEnchantment()
    {
        fireEnchantment = false;
        iceEnchantment = true;
        onEnchantmentCD = true;
        particleSettings.startColor = blue;
        particles.Play();
    }

    public void resetEnchantments()
    {
        iceEnchantment = false;
        fireEnchantment = false;
        if (particles != null)
        {
            particles.Stop();
        }
    }

    public void resetEnchantmentCooldown()
    {
        onEnchantmentCD = false;
    }

    public bool getOnEnchantmentCD()
    {
        return onEnchantmentCD;
    }
    protected void checkDeath()
    {
        if (_hitpoints <= 0)
        {
            gameObject.SetActive(false);
            dropHealthPotion();
        }
    }

    //Creates a random number. If the random number is within a certain range, notifys observer to drop a health potion.
    private void dropHealthPotion()
    {
        float randomFloat = Random.Range(0f, 1f);
        if(randomFloat <= Constants.HEALTH_POTION_DROP_CHANCE)
        {
            Subject.Notify(Constants.HEALTH_POTION_DROPPED, transform.position);
        }
    }

}
