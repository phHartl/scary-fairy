using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MovingObj : MonoBehaviour
{
	public float moveSpeed;
	public LayerMask collisionLayer;
	protected Rigidbody2D rb2D;
	protected BoxCollider2D boxCollider;
	protected int _hitpoints;
	protected int _damage;
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

	protected virtual Vector2 Move() //Report back to Gamemanager -> Animation
	{
		return newPos;
	}

	protected void OnDie(GameObject hit) //Report back to Gamemanager
	{
        Destroy(hit);
    }

	protected virtual void OnCollisionEnter2D(Collision2D other) //Report back to Gamemanager, Damage? UI changed etc.
	{
        print(this + " collided with other Object: " + other.gameObject.name);
        if (this.gameObject.tag == "CasualEnemy")
        {
            this._hitpoints -= 50;
            if (this._hitpoints <= 0)
            {
                OnDie(this.gameObject);
            }
        }
    }
}
