using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : EnemyProjectile
{

    // To create an arrow you need its position, rotation and the time it should travel (if we get an general object spawner this class can be a sub class)
    public Rigidbody2D CreateArrow(Vector3 position, Quaternion rotation, float travelTime)
    {
        Rigidbody2D projectileClone = CreateProjectile(position, rotation, travelTime);
        Destroy(projectileClone.gameObject, travelTime);
        return projectileClone;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        Player player = GameObject.FindObjectOfType<RangedPlayer>();
        if (other.CompareTag(Constants.CASUAL_ENEMY))
        {
            player.CalcEnemyDamage(other);
        }
    }
}
