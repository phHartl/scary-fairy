using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public Rigidbody2D projectile;
    private AudioSource projectileSound;

    public virtual Rigidbody2D CreateProjectile(Vector3 position, Quaternion rotation, float travelTime)
    {
        Rigidbody2D projectileClone = Instantiate(projectile, position, rotation);
        if(projectileClone.gameObject.GetComponent<AudioSource>() != null)
        {
            projectileSound = projectileClone.gameObject.GetComponent<AudioSource>();
            projectileSound.pitch = Random.Range(0.75f, 1.25f);
        }
        Destroy(projectileClone.gameObject, travelTime);
        return projectileClone;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Golem golem = GameObject.FindObjectOfType<Golem>();
        if (other.CompareTag(Constants.PLAYER_TAG))
        {
            Player player = other.GetComponent<Player>();
            player.applyDamage(Constants.GOLEM_PROJECTILE_DAMAGE);
            Destroy(this.gameObject);
        }
    }
}