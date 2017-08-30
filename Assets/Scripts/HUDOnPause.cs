using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDOnPause : MonoBehaviour, IObserver {

    private void Start()
    {
        Subject.AddObserver(this);
    }

    public void OnNotify(string gameEvent)
    {
        switch (gameEvent)
        {
            case Constants.DISABLE_HUD:
                gameObject.SetActive(false);
                break;

            case Constants.ENABLE_HUD:
                gameObject.SetActive(true);
                break;
        }
    }

}
