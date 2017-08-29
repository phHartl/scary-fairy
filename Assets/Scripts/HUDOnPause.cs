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
            case "DisableHUD":
                gameObject.SetActive(false);
                break;

            case "EnableHUD":
                gameObject.SetActive(true);
                break;
        }
    }

}
