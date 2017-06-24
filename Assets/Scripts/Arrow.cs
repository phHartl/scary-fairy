using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    public Rigidbody2D square;
    public float speed = 10f;

	// Use this for initialization
	void FireRocket()
    {
        Rigidbody2D arrowClone = Instantiate(square, transform.position, transform.rotation);
        arrowClone.velocity = transform.right * speed;
    }
}
