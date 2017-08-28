using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{

    public Rigidbody2D projectile;

    // To create an arrow you need its position, rotation and the time it should travel (if we get an general object spawner this class can be a sub class)
    public Rigidbody2D CreateProjectile(Vector3 position, Quaternion rotation, float travelTime)
    {
        Rigidbody2D projectileClone = Instantiate(projectile, position, rotation);
        Destroy(projectileClone.gameObject, travelTime);
        return projectileClone;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Player player = GameObject.FindObjectOfType<RangedPlayer>();
        if (other.CompareTag("CasualEnemy"))
        {
            player.CalcEnemyDamage(other);
        }
    }
}
