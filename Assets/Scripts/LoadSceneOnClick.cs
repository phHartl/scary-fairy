using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneOnClick : MonoBehaviour {

    public void loadCurrentLevel()
    {
        Subject.Notify(Constants.CURRENT_LEVEL);
    }

    public void loadInstructions()
    {
        Subject.Notify(Constants.INSTRUCTIONS);
    }

    public void loadMainMenu()
    {
        Subject.Notify(Constants.MAIN_MENU);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
