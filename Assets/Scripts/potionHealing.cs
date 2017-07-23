using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potionHealing : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Subject.Notify("HealthPickup");
            Destroy(this.gameObject);
        }
    }

}
