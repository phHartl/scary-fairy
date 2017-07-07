using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Npc : MovingObj
{

    // #justKnockBackThings
    public int speed = 50;

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
            travelKnockBackPath();
        }
    }

    protected void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.GetComponent<MovingObj>().applyDamage(_damage);
    }


    public void knockBack(Vector2 force)
    {
        // npc needs to travel from start to endmarker
        startTime = Time.time;
        startMarker = transform.position;
        endMarker = startMarker + force;
        journeyLength = Vector2.Distance(startMarker, endMarker);
        isKnockedBack = true;
    }

    private void travelKnockBackPath()
    {
        // calculate a fraction of the path to travel in this frame, based on passed time and speed
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        if ((Vector2)transform.position == endMarker)
        {
            isKnockedBack = false;
        }
        transform.position = Vector2.Lerp(startMarker, endMarker, fracJourney);
    }
}
