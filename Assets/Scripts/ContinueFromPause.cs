using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueFromPause : MonoBehaviour {

    public GameObject menu = GameObject.Find("Container");

	public void resumeGame()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
        Time.timeScale = 1;
    }

}
