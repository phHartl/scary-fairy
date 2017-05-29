using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleePlayer : Player {


	// Use this for initialization
	void Start () {
        base.Start();
        this._hitpoints = 100;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        base.FixedUpdate();
	}
}
