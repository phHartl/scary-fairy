using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MovingObj : MonoBehaviour
{
	protected float moveSpeed;
	public LayerMask collisionLayer;
	protected Rigidbody2D rb2D;
	protected BoxCollider2D boxCollider;
	private int _hitpoints;
	private int _damage;
	protected Vector2 newPos;



	// Use this for initialization
	protected void Start ()
	{
		boxCollider = GetComponent<BoxCollider2D>();
		rb2D = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	protected void Update () {
		
	}

	protected virtual Vector2 Move() //Report back to Gamemanager -> Animation
	{
		return newPos;
	}

	protected void OnDie() //Report back to Gamemanager
	{

	}

	private void OnCollisionEnter2D(Collision2D other) //Report back to Gamemanager, Damage? UI changed etc.
	{
		throw new System.NotImplementedException();
	}
}
