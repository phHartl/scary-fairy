using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDOnPause : MonoBehaviour, IObserver {

    private GameObject HUD;

    private void Start()
    {
        Subject.AddObserver(this);
        HUD = GameObject.Find("HUD");
        DontDestroyOnLoad(gameObject);
    }

    public void OnNotify(string gameEvent)
    {
        switch (gameEvent)
        {
            case Constants.DISABLE_HUD:
                HUD.SetActive(false);
                break;

            case Constants.ENABLE_HUD:
                HUD.SetActive(true);
                break;
        }
    }

}
