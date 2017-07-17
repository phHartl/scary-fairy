using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealth : MonoBehaviour {

    UnityEngine.UI.Slider slider;

    // Use this for initialization
    void Start () {
        slider = GetComponent<UnityEngine.UI.Slider>();
	}
	
	// Update is called once per frame
	void Update () {
		if(this.name == "HealthP1") //Should be done with an event later on
        {
            slider.value = GameObject.Find("Player1").GetComponentInChildren<MovingObj>()._hitpoints;
        }
        else
        {
            slider.value = GameObject.Find("Player2").GetComponentInChildren<MovingObj>()._hitpoints;
        }
	}
}
