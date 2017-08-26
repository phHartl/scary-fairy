using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{

    // Application.Quit only works in a built version of the game
    public void Quit()
    {
        print("Game is quit!");
        Application.Quit();
    }
}
