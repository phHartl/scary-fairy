using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneOnClick : MonoBehaviour {

    public void loadCurrentLevel()
    {
        print("start clicked");
        Subject.Notify("Current Level");
    }
}
