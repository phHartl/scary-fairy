﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Movement : MonoBehaviour {
    public float moveSpeed;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontalp2") > 0.5f || Input.GetAxisRaw("Horizontalp2") < -0.5f)
        {
            transform.Translate(new Vector3(Input.GetAxisRaw("Horizontalp2") * moveSpeed * Time.deltaTime, 0f, 0f));

        }
        if (Input.GetAxisRaw("Verticalp2") > 0.5f || Input.GetAxisRaw("Verticalp2") < -0.5f)
        {

            transform.Translate(new Vector3(0f, Input.GetAxisRaw("Verticalp2") * moveSpeed * Time.deltaTime, 0f));

        }

    }
}
