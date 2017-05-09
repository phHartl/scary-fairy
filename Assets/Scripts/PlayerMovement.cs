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
	private Vector3 horizontalMovement;
	private Vector3 verticalMovement;
	private Vector3 newPos;
	private float axisVertical;
	private float axisHorizontal;

    // Use this for initialization
    void Start()
    {
		rb = GetComponent <Rigidbody2D>();
		players = GameObject.FindGameObjectsWithTag ("Player");
    }

    // Update is called once per frame
    void Update()
    {
		horizontalMovement = new Vector3(0,0,0);
		verticalMovement = new Vector3(0,0,0);
		axisHorizontal = 0;
		axisVertical = 0;
		newPos = new Vector2(0,0);
		axisHorizontal = Input.GetAxisRaw ("Horizontal");
		axisVertical = Input.GetAxisRaw ("Vertical");

		if (axisHorizontal > 0.5f || axisHorizontal < -0.5f)
        {
			for (int i = 0; i < players.Length; i++) 
			{
				if (players [i] != this) 
				{
					currentDistance = (players [i].transform.position.x - transform.position.x);

					if (Mathf.Abs (currentDistance) < maxHorizontalDistance) 
					{
						horizontalMovement = new Vector3 (axisHorizontal * moveSpeed * Time.deltaTime, 0, 0);
					} 
					else if ((currentDistance < 0 && axisHorizontal > -0.5f) || (currentDistance > 0 && axisHorizontal < 0.5f) ) 
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

					if (Mathf.Abs (currentDistance) < maxVerticalDistance) 
					{
						verticalMovement = new Vector3 (0, axisVertical * moveSpeed * Time.deltaTime, 0);
					} 
					else if ((currentDistance < 0 && axisVertical > -0.5f) || (currentDistance > 0 && axisVertical < 0.5f) ) {
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
