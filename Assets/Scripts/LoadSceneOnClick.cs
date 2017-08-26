using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneOnClick : MonoBehaviour {

    public void loadCurrentLevel()
    {
        Subject.Notify("Current Level");
    }

    public void loadInstructions()
    {
        Subject.Notify("Instructions");
    }

    public void loadMainMenu()
    {
        Subject.Notify("Main Menu");
    }
}
