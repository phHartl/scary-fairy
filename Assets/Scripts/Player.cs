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

    // Use this for initialization
    void Start()
    {
        base.Start();
        players[0] = GameObject.FindGameObjectWithTag("Player1");
        players[1] = GameObject.FindGameObjectWithTag("Player2");
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override Vector2 Move()
    {
        float axisH = Input.GetAxisRaw(axisHorizontal);
        float axisV = Input.GetAxisRaw(axisVertical);
        float nextDistance;

        if (axisH > 0.5f || axisH < -0.5f)
        {
            horizontalMovement = new Vector2(axisH * moveSpeed * Time.deltaTime, 0);
            currentDistance = Vector2.Distance(rb2D.position, CameraControl.camPosition);
            nextDistance = Vector2.Distance(rb2D.position + horizontalMovement,
                CameraControl.camPosition); 

            if (!(Mathf.Abs(currentDistance) < maxHorizontalDistance) || !(Mathf.Abs(currentDistance) > Mathf.Abs(nextDistance)))
            {
                horizontalMovement = Vector2.zero;
            }
        }

        if (axisV > 0.5f || axisV < -0.5f)
        {
            verticalMovement = new Vector2(0, axisV * moveSpeed * Time.deltaTime);
            currentDistance = Vector2.Distance(rb2D.position, CameraControl.camPosition);
            nextDistance = Vector2.Distance(rb2D.position + verticalMovement,
                CameraControl.camPosition);
            //movement possible if player below distance threshold or moving towards other player
            if (!(Mathf.Abs(currentDistance) < maxVerticalDistance) || !(Mathf.Abs(currentDistance) > Mathf.Abs(nextDistance)))
            {
                verticalMovement = Vector2.zero;
            }
        }
        newPos = rb2D.position + horizontalMovement + verticalMovement;
        return newPos;
    }
}