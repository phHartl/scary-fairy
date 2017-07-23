using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potionHealing : MonoBehaviour, TransformObserver {

    public Rigidbody2D potion;

    public void Start()
    {
        Subject.AddTransformObserver(this);
    }

    public Rigidbody2D createPotion(Vector3 position)
    {
        Quaternion rotation = new Quaternion(0,0,0,0);
        Rigidbody2D potionClone = Instantiate(potion, position, rotation);
        return potionClone;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Subject.Notify("HealthPickup");
            Destroy(this.gameObject);
        }
    }

    public void OnNotify(string gameEvent, Vector3 position)
    {
        switch (gameEvent)
        {
            case "HealthPotionDropped":
                createPotion(position);
                break;
        }
    }

}
