using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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

	protected void OnDie(GameObject hit) //Report back to Gamemanager
	{
        Rigidbody2D hit1 = hit.GetComponent<Rigidbody2D>();
        hit1.AddForce(Vector2.one *10, ForceMode2D.Impulse);
    }
}
