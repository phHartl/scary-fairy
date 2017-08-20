using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{

    public GameObject enemies;
    public GameObject exit;
    private bool enemiesSpawned = true;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemies.SetActive(true);
        }
    }

    private void Update()
    {
        if (enemies.activeSelf)
        {
            if (enemiesSpawned)
            {
                enemiesSpawned = checkIfEnemiesAlive();
            }
            else
            {
                exit.SetActive(false);
            }
        }
    }

    private bool checkIfEnemiesAlive()
    {
        for (int i = 0; i < enemies.transform.childCount; i++)
        {
            if (enemies.transform.GetChild(i).gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }
}

