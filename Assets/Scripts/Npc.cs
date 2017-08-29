using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Npc : MovingObj
{
    private Vector2 startMarker;
    private Vector2 endMarker;
    private float journeyLength;
    private bool isKnockedBack = false;
    protected AIMove AI;
    private Vector3 currentDir;

    public Rigidbody2D potion;
    protected bool isBurning = false;
    protected bool durationRefreshed = false;

    private float player1Pos;
    private float player2Pos;
    private float thisPos;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        AI = GetComponent<AIMove>(); //Simple caching of component -> better performance
    }

    protected virtual void FixedUpdate()
    {
        if (isKnockedBack && Math.Abs(Vector2.Distance(startMarker, rb2D.position)) > journeyLength) //IsKnockedBack used for performance improvement, if first argument is false, no more calc is need (distance needs cpu power)
        {
            rb2D.velocity = Vector2.zero; //Stop rigidbody from moving
            isKnockedBack = false;
            AI.canMove = true; //AI can now move again
            
        }
    }

    protected override void Update()
    {
        base.Update();
        //slider.value = _hitpoints;
        checkMovement();
        animator.SetBool("isMoving", true);
        if (Mathf.Abs(currentDir.x) > 0.5f)
        {
            animator.SetFloat("MoveX", currentDir.x);
        }
        if (Mathf.Abs(currentDir.y) > 0.5f)
        {
            animator.SetFloat("MoveY", currentDir.y);
        }
        player1Pos = GameObject.Find("Player1").transform.GetChild(0).transform.position.y;
        player2Pos = GameObject.Find("Player2").transform.GetChild(0).transform.position.y;
        thisPos = transform.position.y;
        checkLayer();
    }

    public void checkMovement()
    {
        currentDir = AI.getDirection();
        isMoving = AI.canMove;
    }



    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Constants.PLAYER_TAG) && !isKnockedBack)
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.applyDamage(_damage);
            if (!player.isDead)
            {
                rb2D.bodyType = RigidbodyType2D.Kinematic; //Set rigidbody to kinematic to prevent player from pushing enemy
            }
            rb2D.velocity = Vector2.zero;
        }
    }

 
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        rb2D.bodyType = RigidbodyType2D.Dynamic; //Set rigidbody dynamic again;
    }



    public void knockBack(Vector2 force, float playerMass)
    {
        startMarker = rb2D.position;
        endMarker = startMarker + force;
        journeyLength = Math.Abs(Vector2.Distance(startMarker, endMarker));
        rb2D.velocity = Vector2.zero;
        isKnockedBack = true;
        AI.canMove = false;
        rb2D.bodyType = RigidbodyType2D.Dynamic;
        rb2D.AddForce(force,ForceMode2D.Impulse); //Add force in direction using an impulse
        rb2D.velocity = rb2D.velocity * playerMass; //If a player is heavier, knockBack further
    }


    public override void createPotion(Vector3 position)
    {
        Quaternion rotation = new Quaternion(0, 0, 0, 0);
        Rigidbody2D potionClone = Instantiate(potion, position, rotation);
    }

    public override void applyDamage(int damage)
    {
        _hitpoints -= damage;
        checkDeath();
        print("Enemy took damage, health: " + _hitpoints);
    }

    public virtual void applyDamage(int damage, string enchantment)
    {
        _hitpoints -= damage;
        checkDeath();
        print("Enemy took damage, health: " + _hitpoints);

        if (enchantment == Constants.FIRE_ENCHANTMENT)
        {
            if (!isBurning)
            {
                StartCoroutine(applyBurnDamage());
            }
            if (isBurning)
            {
                durationRefreshed = true;
            }
        }
        if (enchantment == Constants.ICE_ENCHANTMENT)
        {
            _hitpoints -= damage;
            checkDeath();
        }
    }

    protected IEnumerator applyBurnDamage()
    {
        isBurning = true;
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        for (int i = 0; i < Constants.BURN_DAMAGE_DURATION; i++)
        {
            if (durationRefreshed)
            {
                i = 0;
                durationRefreshed = false;
            }
            _hitpoints -= Constants.FAIRY_BURN_TICK_DAMAGE;
            checkDeath();
            yield return new WaitForSeconds(Constants.BURN_DAMAGE_TICKRATE);
        }
        isBurning = false;
        GetComponent<Renderer>().material.color = Color.white;
    }

    private void checkLayer()
    {
        if (thisPos > player1Pos && thisPos > player2Pos)
        {
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = Constants.MONSTER_BACKGROUND;
        }
        else if ((thisPos > player1Pos && thisPos < player2Pos) || (thisPos < player1Pos && thisPos > player2Pos))
        {
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = Constants.MONSTER_MIDDLE;
        }
        else if (thisPos < player1Pos && thisPos < player2Pos)
        {
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = Constants.MONSTER_FOREGROUND;
        }
    }

}
