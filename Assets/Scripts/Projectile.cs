using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public const int PROJECTILE_DAMAGE = 10;
    public const float PROJECTILE_TRAVEL_TIME = 3f;

    protected void Update()
    {
        Destroy(this.gameObject, PROJECTILE_TRAVEL_TIME);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Golem golem = GameObject.FindObjectOfType<Golem>();
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.applyDamage(PROJECTILE_DAMAGE);
            Destroy(this.gameObject);
        }
    }
}