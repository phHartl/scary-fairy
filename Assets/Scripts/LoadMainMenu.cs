using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(LoadMainMenuFromCredits());
	}

    private IEnumerator LoadMainMenuFromCredits()
    {
        yield return new WaitForSeconds(5f);
        Subject.Notify(Constants.CREDITS_END);
    }
}
