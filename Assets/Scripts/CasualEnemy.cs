﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasualEnemy : Npc {

    // Use this for initialization
    void Start()
    {
        base.Start();

        // hitpoints placeholder
        this._hitpoints = 169;

        // damage placeholder
        this._damage = 69;

        //movespeed placeholder
        this.moveSpeed = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        base.FixedUpdate();
    }

    // Pathfinding has yet to be implemented (I think there is a Unity-Plugin that can be used for pathfinding)
    protected override Vector2 Move()
    {
        return rb2D.position;
    }
}
