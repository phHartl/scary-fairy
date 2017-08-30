using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{

    public GameObject enemies;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Constants.PLAYER_TAG))
        {
            enemies.SetActive(true);
        }
    }
}

