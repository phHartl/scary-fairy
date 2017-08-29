using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class CasualEnemy : Npc
{

    UnityEngine.UI.Slider slider;


    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        slider = GetComponentInChildren<UnityEngine.UI.Slider>();
        this._hitpoints = Constants.CASUAL_ENEMY_MAX_HEALTH;
        this._damage = Constants.CASUAL_ENEMY_BASE_DAMAGE;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        slider.value = _hitpoints;
        base.Update();
    }

    protected override Vector2 Move()
    {
        return rb2D.position;
    }
    

}
