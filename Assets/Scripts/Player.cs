using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingObj
{
    public float maxVerticalDistance = 8.0f;
    public float maxHorizontalDistance = 12.0f;
    public float currentDistance;
    private GameObject[] players = new GameObject[2];
    private Vector2 horizontalMovement;
    private Vector2 verticalMovement;
    public string axisVertical;
    public string axisHorizontal;
    private bool playerMoving;
    private Vector2 lastMove;
    private bool playerAttack;

    // Use this for initialization
    void Start()
    {
        base.Start();
        players[0] = GameObject.FindGameObjectWithTag("Player1");
        players[1] = GameObject.FindGameObjectWithTag("Player2");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        base.FixedUpdate();
        animator.SetFloat("MoveX", Input.GetAxisRaw(axisHorizontal));
        animator.SetFloat("MoveY", Input.GetAxisRaw(axisVertical));
        animator.SetBool("PlayerMoving", playerMoving);
        animator.SetFloat("LastMoveX",lastMove.x);
        animator.SetFloat("LastMoveY", lastMove.y);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("PlayerAttack", true);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("PlayerAttack", false);
        }



    }

    protected override Vector2 Move()
    {
        playerMoving = false;
        currentDistance = 0;
        horizontalMovement = new Vector2(0, 0);
        verticalMovement = new Vector2(0, 0);
        float axisH = Input.GetAxisRaw(axisHorizontal);
        float axisV = Input.GetAxisRaw(axisVertical);
        float nextDistance;

        if (axisH > 0.5f || axisH < -0.5f)
        {
            currentDistance = rb2D.position.x - CameraControl.camPosition.x;
            nextDistance = Vector2.Distance(rb2D.position + new Vector2(axisH * moveSpeed * Time.deltaTime, 0),
                CameraControl.camPosition);

            if (Mathf.Abs(currentDistance) < maxHorizontalDistance || (currentDistance > 0 && axisH < 0) || (currentDistance < 0 && axisH > 0))
            {
                horizontalMovement = new Vector2(axisH * moveSpeed * Time.deltaTime, 0);
                playerMoving = true;
                lastMove = new Vector2(axisH, 0);
            }
        }

        if (axisV > 0.5f || axisV < -0.5f)
        {
            currentDistance = rb2D.position.y - CameraControl.camPosition.y;
            //movement possible if player below distance threshold or moving towards other player
            if (Mathf.Abs(currentDistance) < maxVerticalDistance || (currentDistance > 0 && axisV < 0) || (currentDistance < 0 && axisV > 0)) 
            {
                verticalMovement = new Vector2(0, axisV * moveSpeed * Time.deltaTime);
                playerMoving = true;
                lastMove = new Vector2(0,axisV);
            }
        }
        newPos = rb2D.position + horizontalMovement + verticalMovement;
        return newPos;
    }
}