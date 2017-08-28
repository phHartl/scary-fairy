using UnityEngine;

public abstract class MovingObj : MonoBehaviour
{
    public float moveSpeed;
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
    [HideInInspector]public bool alive = true;

    public float HEALTH_POTION_CHANCE = 0.5f;

    public bool iceEnchantment;
    public bool fireEnchantment;

    public float BURN_DAMAGE_DURATION = 4f;
    public float BURN_TICKRATE = 0.5f;

    [HideInInspector] public string ICE_ENCHANTMENT = "ICE_ENCHANTMENT";
    [HideInInspector] public string FIRE_ENCHANTMENT = "FIRE_ENCHANTMENT";




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
            alive = false;
            dropHealthPotion();
            gameObject.SetActive(false);
        }
    }

    //Creates a random number. If the random number is within a certain range, notifys observer to drop a health potion.
    private void dropHealthPotion()
    {
        float randomFloat = Random.Range(0f, 1f);
        if(randomFloat <= HEALTH_POTION_CHANCE)
        {
            createPotion(transform.position);
        }
    }

    public virtual void createPotion(Vector3 position)
    {
       
    }

}
