using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Npc : MovingObj
{

    // #justKnockBackThings
    public float speed = 5;

    private Vector2 startMarker;
    private Vector2 endMarker;
    private float startTime;
    private float journeyLength;
    private bool isKnockedBack = false;

    // Use this for initialization
    void Start()
    {
        base.Start();
        startTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();
        if (isKnockedBack)
        {
            float distCovered = (Time.time - startTime) * 50;
            float fracJourney = distCovered / journeyLength;
            if ((Vector2)transform.position == endMarker)
            {
                isKnockedBack = false;
            }
            transform.position = Vector2.Lerp(startMarker, endMarker, fracJourney);
        }
    }


    public void knockBack(Vector2 force)
    {
        startTime = Time.time;
        startMarker = transform.position;
        endMarker = startMarker + force;
        journeyLength = Vector2.Distance(startMarker, endMarker);
        isKnockedBack = true;
    }
}
