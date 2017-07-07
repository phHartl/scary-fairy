using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    public Rigidbody2D arrow;

	// To create an arrow you need its position, rotation and the time it should travel (if we get an general object spawner this class can be a sub class)
	public Rigidbody2D createArrow(Vector3 position, Quaternion rotation, float travelTime)
    {
        Rigidbody2D arrowClone = Instantiate(arrow, position, rotation);
        Destroy(arrowClone.gameObject, travelTime);
        return arrowClone;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CasualEnemy"))
        {
            CasualEnemy ce = other.GetComponent<CasualEnemy>();
            ce.applyDamage(GameObject.FindObjectOfType<RangedPlayer>().getDamage()); //If we want enemies to be able to shoot arrows we need some changes here
            print("Arrow hit enemy");
        }
    }
}
