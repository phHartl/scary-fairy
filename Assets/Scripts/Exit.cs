using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Exit : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player1") || collider.CompareTag("Player2"))
        {
            Subject.Notify("Next Level");
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
