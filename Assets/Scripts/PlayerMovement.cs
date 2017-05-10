using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float moveSpeed;
	public float maxVerticalDistance = 8;
	public float maxHorizontalDistance = 12;

	private Rigidbody2D rb;
	private float currentDistance;
	private GameObject[] players;
	private Vector2 horizontalMovement;
	private Vector2 verticalMovement;
	private Vector2 newPos;
	private float axisVertical;
	private float axisHorizontal;

    // Use this for initialization
	void Awake()
	{
		rb = GetComponent <Rigidbody2D>();
	}

	void Start()
	{
		players = GameObject.FindGameObjectsWithTag ("Player2");
	}

    // Update is called once per frame
    void Update()
    {
		horizontalMovement = new Vector2(0,0);
		verticalMovement = new Vector2(0,0);
		newPos = new Vector2(0,0);

		axisHorizontal = Input.GetAxisRaw ("Horizontal");
		axisVertical = Input.GetAxisRaw ("Vertical");

		if (axisHorizontal > 0.5f || axisHorizontal < -0.5f)
        {
			for (int i = 0; i < players.Length; i++) 
			{
				
				{
					currentDistance = (players [i].transform.position.x - transform.position.x);

					//movement possible if player below distance threshold or moving towards other player
					if (Mathf.Abs (currentDistance) < maxHorizontalDistance || (currentDistance > 0 && axisHorizontal > 0) || (currentDistance < 0 && axisHorizontal < 0)) 
					{ 
						horizontalMovement = new Vector2 (axisHorizontal * moveSpeed * Time.deltaTime, 0);
					} 
					else
					{
						horizontalMovement = new Vector2(0,0);
					} 
				}
			}

			
        }

		if (axisVertical > 0.5f || axisVertical < -0.5f)
        {
			for (int i = 0; i < players.Length; i++) {

				if (players [i] != this) 
				{
					currentDistance = (players [i].transform.position.y - transform.position.y);

					//movement possible if player below distance threshold or moving towards other player
					if (Mathf.Abs (currentDistance) < maxVerticalDistance || (currentDistance > 0 && axisVertical > 0) || (currentDistance < 0 && axisVertical < 0)) 
					{
						verticalMovement = new Vector2 (0, axisVertical * moveSpeed * Time.deltaTime);
					} 
					else 
					{
						verticalMovement = new Vector2 (0, 0);
					} 
				}
			}
        }

		newPos = rb.position + horizontalMovement + verticalMovement;
		rb.MovePosition (newPos);

    }
	void FixedUpdate() 
	{
		
	}
}
