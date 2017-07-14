using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class CasualEnemy : Npc
{

    UnityEngine.UI.Slider slider;
    Vector3 currentDir;



    // Use this for initialization
    void Start()
    {
        base.Start();
        slider = GetComponentInChildren<UnityEngine.UI.Slider>();
        animator = GetComponent<Animator>();
        this._hitpoints = 100;
        this._damage = 30;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();
        //slider.value = _hitpoints;
        checkMovement();
        animator.SetBool("isMoving", true);
        animator.SetFloat("MoveX", currentDir.x);
        animator.SetFloat("MoveY", currentDir.y);
    }

    private void checkMovement()
    {
        currentDir = AI.getDirection();
        isMoving = AI.canMove;
    }
}
