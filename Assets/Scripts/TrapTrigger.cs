using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour {

    public GameObject entrance;
    public GameObject trapEnemies;
    public BoxCollider2D trapCollider;
    public GameObject exit;
    private bool enemiesSpawned;

	// Use this for initialization
	protected void Start () {
        enemiesSpawned = false;
        trapEnemies.SetActive(false);
	}
	
	// Update is called once per frame
	protected void Update () {
        if (enemiesSpawned)
        {
            entrance.SetActive(true);
            enemiesSpawned = checkIfEnemiesAlive();     //Entrance closed as long as at least one enemy spawned by trap is alive
        } else
        {
            entrance.SetActive(false);
        }
	}

    //Spawn enemies and close entrance when stepped on trap
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Constants.FEET_HITBOX))
        {
            enemiesSpawned = true;
            trapEnemies.SetActive(true);
        }
    }


    /*
     * Checks if enemies spawned by trap are still alive.
     * If there is at least 1 enemy alive returns true.
     * If all enemies are dead returns false.
    */
    private bool checkIfEnemiesAlive()
    {
        for (int i = 0; i < trapEnemies.transform.childCount; i++)
        {
            if (trapEnemies.transform.GetChild(i).gameObject.activeSelf)
            {
                exit.SetActive(false);
                return true;
            }
        }
        return false;
    }
}
