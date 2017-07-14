using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class AIMove : AIBase {

    private Transform target;

    public float updateRate = 0.5f;

    public Path path;

    public bool canMove = true;

    public float speed = 300f;

    public ForceMode2D forceMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    public float nextWaypointDistance = 0.5f;

    private int currentWaypoint = 0;

    private Transform[] targets = new Transform[2];
    

    private void Start()
    {
        StartCoroutine(SearchPlayer());
        StartCoroutine(UpdatePath());
    }
    //Searches for the nearest player -> Quick and Dirty because of new player structure in other branchs
    private IEnumerator SearchPlayer()
    {
        targets[0] = GameObject.FindGameObjectWithTag("Player1").transform;
        targets[1] = GameObject.FindGameObjectWithTag("Player2").transform;
        float distanceToPlayer1 = Math.Abs(Vector3.Distance(transform.position, targets[0].position));
        float distanceToPlayer2 = Math.Abs(Vector3.Distance(transform.position, targets[1].position));
        if (distanceToPlayer1 == Math.Min(distanceToPlayer1, distanceToPlayer2))
        {
            target = targets[0];
        }
        if (distanceToPlayer2 == Math.Min(distanceToPlayer1, distanceToPlayer2))
        {
            target = targets[1];
        }
        yield return new WaitForSeconds(3f); //Search for the newest player every three seconds
        StartCoroutine(SearchPlayer());
    }

    //Gets called if a path is available
    private void OnPathComplete(Path p)
    {
        Debug.Log("We got a path. Error?" + p.error);
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            StartCoroutine(SearchPlayer());
        }
        seeker.StartPath(transform.position, target.position, OnPathComplete); //Calcs a path to the target
        yield return new WaitForSeconds(updateRate); //Update our path every predefined time
        StartCoroutine(UpdatePath());
    }

    protected override void MovementUpdate(float deltaTime)
    {
        if(target == null)
        {
            return;
        }

        if(path == null)
        {
            return;
        }

        if (!canMove)
        {
            return;
        }

        if(currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
            {
                return;
            }
            Debug.Log("End of path reached");
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;

        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;
        rigid2D.AddForce(dir, forceMode); //This script uses addForce to move objects -> high speed & linear drag need to be set

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if(dist < nextWaypointDistance) //Have we reached our next waypoint?
        {
            currentWaypoint++;
        }
    }

    public Vector3 getDirection()
    {
        Vector3 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        return direction;
    }
}
