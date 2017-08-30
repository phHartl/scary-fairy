using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public string pause;
    public bool isPaused;

	// Use this for initialization
	void Start () {
        isPaused = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown(pause))
        {
            if (!isPaused)
            {
                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    gameObject.transform.GetChild(i).gameObject.SetActive(true);
                }
                Time.timeScale = 0;
                isPaused = true;
                Subject.Notify("DisableHUD");
            } else
            {
                GetComponentInChildren<ContinueFromPause>().resumeGame();
                isPaused = false;
            }
        }
    }
}
