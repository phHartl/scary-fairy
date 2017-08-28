using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potionHealing : MonoBehaviour, TransformObserver {

    public Rigidbody2D potion;

    public void Start()
    {
        Subject.AddTransformObserver(this);
    }


    /*
     * Creates a new instance of Health Potion
     * 'position' is the position where the health potion spawns
     */
    public Rigidbody2D createPotion(Vector3 position)
    {
        Quaternion rotation = new Quaternion(0,0,0,0);
        Rigidbody2D potionClone = Instantiate(potion, position, rotation);
        return potionClone;
    }


    /*
     * Checks for players walking over the health potion
     * On contact the observer is notified of the pickup and the potion gets destroyed
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Constants.PLAYER_TAG))
        {
            Subject.Notify(Constants.HEALTH_PICKUP);
            Destroy(this.gameObject);
        }
    }


    /*
     * Currently only gets called from MovingObject
     * Calls createPotion() with the position of the dead enemy
     */
    public void OnNotify(string gameEvent, Vector3 position)
    {
        switch (gameEvent)
        {
            case Constants.HEALTH_POTION_DROPPED:
                createPotion(position);
                break;
        }
    }

}
