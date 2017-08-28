using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : PlayerProjectile
{

    public const int PROJECTILE_DAMAGE = 10;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        Golem golem = GameObject.FindObjectOfType<Golem>();
        if (other.CompareTag(Constants.PLAYER_TAG))
        {
            Player player = other.GetComponent<Player>();
            player.applyDamage(PROJECTILE_DAMAGE);
            Destroy(this.gameObject);
        }
    }
}