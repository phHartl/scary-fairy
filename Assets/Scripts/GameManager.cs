using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour, IObserver
{



    public static GameManager instance = null;
    //levelNum: in welchem Level befinden wir uns gerade - wird inkrementiert wenn das Levelende erreicht wird
    //und ein neues Level geladen werden soll
    private int levelNum = 1;
    private int instructions = 4;
    private int reloadDelay;

    void Awake()
    {
        // GameManager wird nicht zerstört wenn ein neues Level geladen wird
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        //delete player states so first level starts with full health ranger and warrior
        PlayerPrefs.DeleteAll();

        reloadDelay = 3;

        //lade erstes Level
        initGame();
    }

    public void OnNotify(string gameEvent)
    {
        switch (gameEvent)
        {
            case Constants.NEXT_LEVEL:
                levelNum += 1;
                SceneManager.LoadScene(levelNum);
                Time.timeScale = 1;
                break;
            case Constants.MAIN_MENU:
                SceneManager.LoadScene("MainMenu");
                Time.timeScale = 1;
                break;
            case Constants.CURRENT_LEVEL:
                SceneManager.LoadScene(levelNum);
                Time.timeScale = 1;
                break;
            case Constants.INSTRUCTIONS:
                SceneManager.LoadSceneAsync(instructions);
                break;
	        case Constants.ALL_PLAYERS_DEAD:
                StartCoroutine(restartLevel(reloadDelay));
                Time.timeScale = 1;
                break;
            case Constants.CREDITS_END:
                PlayerPrefs.DeleteAll();
                levelNum = 1;
                SceneManager.LoadScene("MainMenu");
                Time.timeScale = 1;
                break;
            default:
                break;
        }

    }

    public void initGame()
    {
        //SceneManager.LoadScene lädt Level anhand deren Index in den Build Settings (strg + shift + B in Unity)
        //auch anhand des Namens möglich
        SceneManager.LoadScene("MainMenu");
    }

    void Start()
    {
        Subject.AddObserver(this);
    }

    void Update()
    {

    }

    IEnumerator restartLevel(int numberOfSeconds)
    {
        yield return new WaitForSeconds(numberOfSeconds);
        SceneManager.LoadSceneAsync(levelNum); //Async is better here, because there is already a scene displayed
    }
}
