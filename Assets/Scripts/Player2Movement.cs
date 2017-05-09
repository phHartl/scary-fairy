using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Movement : MonoBehaviour {

	public float moveSpeed;
	public float maxVerticalDistance = 8;
	public float maxHorizontalDistance = 12;

	private Rigidbody2D rb;
	private float currentDistance;
	private GameObject[] players;
	private Vector3 horizontalMovement;
	private Vector3 verticalMovement;
	private Vector3 newPos;
	private float axisVertical;
	private float axisHorizontal;

	// Use this for initialization
	void Awake()
	{
		rb = GetComponent <Rigidbody2D>();
	}

	void Start()
	{
		players = GameObject.FindGameObjectsWithTag ("Player1");
	}

	// Update is called once per frame
	void Update()
	{
		horizontalMovement = new Vector3(0,0,0);
		verticalMovement = new Vector3(0,0,0);
		axisHorizontal = 0;
		axisVertical = 0;
		newPos = new Vector2(0,0);
		axisHorizontal = Input.GetAxisRaw ("Horizontalp2");
		axisVertical = Input.GetAxisRaw ("Verticalp2");

		if (axisHorizontal > 0.5f || axisHorizontal < -0.5f)
		{
			for (int i = 0; i < players.Length; i++) 
			{
				if (players [i] != this) 
				{
					currentDistance = (players [i].transform.position.x - transform.position.x);
					Debug.Log ("current X2 Distance: "+currentDistance);
					if (Mathf.Abs (currentDistance) < maxHorizontalDistance || (currentDistance > 0 && axisHorizontal > 0) || (currentDistance < 0 && axisHorizontal < 0)) 
					{ 
						horizontalMovement = new Vector3 (axisHorizontal * moveSpeed * Time.deltaTime, 0, 0);
					} 
					else
					{
						horizontalMovement = new Vector3(0,0,0);
					} 
				}
			}


		}

		if (axisVertical > 0.5f || axisVertical < -0.5f)
		{
			for (int i = 0; i < players.Length; i++) {

				if (players [i] != this) {
					currentDistance = (players [i].transform.position.y - transform.position.y);
					Debug.Log ("current Y2 Distance: "+currentDistance);
					if (Mathf.Abs (currentDistance) < maxVerticalDistance || (currentDistance > 0 && axisVertical > 0) || (currentDistance < 0 && axisVertical < 0)) 
					{
						verticalMovement = new Vector3 (0, axisVertical * moveSpeed * Time.deltaTime, 0);
					} 
					else 
					{
						verticalMovement = new Vector3 (0, 0, 0);
					} 
				}
			}
		}

		newPos = (Vector2) rb.position + (Vector2) horizontalMovement + (Vector2) verticalMovement;
		rb.MovePosition (newPos);

	}
	void FixedUpdate() 
	{

	}
}

