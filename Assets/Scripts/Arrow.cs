using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    public Rigidbody2D arrow;

	// Use this for initialization
	public Rigidbody2D createArrow(Vector3 position, Quaternion rotation)
    {
        Rigidbody2D arrowClone = Instantiate(arrow, position, rotation);
        return arrowClone;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       if(other.CompareTag("CasualEnemy"))
        {
            CasualEnemy ce = other.GetComponent<CasualEnemy>();
            ce.applyDamage(10);
            print("Arrow hit enemy");
        }
    }
}
