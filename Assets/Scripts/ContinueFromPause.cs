using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueFromPause : MonoBehaviour {

    public GameObject container;

	public void resumeGame()
    {
        for (int i = 0; i < container.transform.childCount; i++)
        {
            container.transform.GetChild(i).gameObject.SetActive(false);
            GetComponentInParent<PauseMenu>().isPaused = false;

        }
        Subject.Notify(Constants.ENABLE_HUD);
        Time.timeScale = 1;
    }

}
