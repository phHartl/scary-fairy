using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemExit : MonoBehaviour {

    public MovingObj golem;
	
	// Update is called once per frame
	void Update () {
        if (!golem.alive)
        {
            this.gameObject.SetActive(false);
        }
	}
}
