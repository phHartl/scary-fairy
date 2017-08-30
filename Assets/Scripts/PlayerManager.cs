using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IObserver
{
    public string axisVertical;
    public string axisHorizontal;
    public string attackInput;
    public string changeClassUpInput;
    public string changeClassDownInput;
    public string firstSpecialAbility;
    public string secondSpecialAbility;
    public string speedBoostInput;
    private GameObject playerObject;
    public int playerNumber;
    // The hasFairy-attribute is used to check and set if any player is a fairy or not
    private static bool hasFairy;
    public GameObject[] classes;
    private int currentClassIndex;
    private CameraControl cameraControl;
    private CooldownManager cdmanager;
    public Player thisPlayer;
    public Player otherPlayer;
    private UIManager uiManager;


    // Use this for initialization
    private void Awake () {
        Subject.AddObserver(this);
        hasFairy = false;
        cameraControl = GameObject.Find("CameraRig").GetComponent<CameraControl>();
        cdmanager = GetComponent<CooldownManager>();
        uiManager = GetComponent<UIManager>();
        currentClassIndex = 0;
        InitPlayerObject();
        SetAxis();
    }

    private void Start()
    {
        SetFairyTarget();
        thisPlayer = gameObject.GetComponentInChildren<Player>();
        otherPlayer = OtherPlayer().GetComponentInChildren<Player>();
        uiManager.InitUI();
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        uiManager.UpdateHealth(playerObject.GetComponent<Player>()._hitpoints);
    }

    // This method is used to change the UI components that are related to a class
    private void UpdateClassUI()
    {
        uiManager.UpdateClass(currentClassIndex);
        uiManager.StartSwapCooldown(Constants.PLAYER_CLASS_CHANGE_COOLDOWN);
    }

    // This method initializes a new player child-object depending on the first entry of the classes array
    private void InitPlayerObject()
    {   
        // returns player state to the last saved state, if there is one
        if (PlayerPrefs.HasKey("HP" + gameObject.name))
        {
            currentClassIndex = PlayerPrefs.GetInt("classIndex" + gameObject.name);
            playerObject = SpawnPlayer();
            if (PlayerPrefs.GetInt("hasFairy") == 1)
            {
                hasFairy = true;           
            }
            gameObject.GetComponentInChildren<MovingObj>()._hitpoints = PlayerPrefs.GetInt("HP" + gameObject.name);
            uiManager.InitPlayerUIComponents();
            uiManager.UpdateClass(currentClassIndex);
        }
        else if (gameObject.GetComponentInChildren<Player>())
        {
            playerObject = gameObject.transform.GetChild(0).gameObject;
        } 
        else
        {
            // No child-object
            GameObject newPlayer = SpawnPlayer();
            playerObject = newPlayer;
            playerObject.GetComponentInChildren<MovingObj>()._hitpoints = 100;
        }
    }

    private GameObject SpawnPlayer()
    {
        return Instantiate(classes[currentClassIndex], gameObject.transform.position, gameObject.transform.rotation, gameObject.transform) as GameObject;
    }

    // This methods sets the Axis for the Player
    private void SetAxis()
    {
        Player player = playerObject.GetComponent<Player>();
        player.axisHorizontal = axisHorizontal;
        player.axisVertical = axisVertical;
    }

    // Update is called once per frame
    private void Update () {
        //Prevent player from doing anything while being dead
        if (!thisPlayer.isDead)
        {
            // Attack
            if (Input.GetButtonDown(attackInput))
            {
                Attack();
            }

            // Change Class
            if ((Input.GetButtonDown(changeClassUpInput) || Input.GetButtonDown(changeClassDownInput)) && !cdmanager.GetClassChangeCooldown())
            {
                SavePlayerState();
                bool down = false;
                if (Input.GetButtonDown(changeClassDownInput))
                {
                    down = true;
                }
                StartCoroutine(ChangeClass(down));
            }
        }
        checkFairyAutoswitch();
        checkLayer();
        UpdateHealthBar();
    }


    /*
     * Switches to another class if the player is a fairy and its host dies
     */
    private void checkFairyAutoswitch()
    {
        if(hasFairy && otherPlayer.isDead)
        {
            StartCoroutine(ChangeClass(true));
        }
    }


    /*
     * Checks if player is in front of or behind other player by compairing y-coordinates of both players.
     * If current player is behind other player, sets SortingLayer to PLAYER_BACKGROUND, if he is in front sets SortingLayer to PLAYER_FOREGROUND
     */
    private void checkLayer()
    {
        if(OtherPlayer().transform.GetChild(0).transform.position.y > gameObject.transform.GetChild(0).transform.position.y)
        {
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = Constants.PLAYER_FOREGROUND;
        } else
        {
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = Constants.PLAYER_BACKGROUND;
        }
    }

    private void Attack()
    {
        Player player = playerObject.GetComponent<Player>();
        player.AttemptAttack();
    }

    /*
     * This method exchanges the Child of the GameObject the script is attached to (Player1 or Player2) with
     * an instance of the prefab that has been set in the classes-array in the Unity inspector.
     * The classes get changed according to their index in the array (0 - first).
     * It afterwards sets the new Player-GameObject as the new target of the camera.
     */
    private IEnumerator ChangeClass(bool changedDownwards)
    {
        // The transform of the player gets saved, afterwards the old Player-Gameobject gets destroyed
        Transform newPlayerTransform = playerObject.transform;
        Destroy(playerObject.gameObject);

        // The player switches his class from fairy to the next class
        if (classes[currentClassIndex].name == "fairy")
        {
            hasFairy = false;
        }

        // Increments the Index
        UpdateCurrentClassIndex(changedDownwards);

        // This prohibits both players from being a fairy at the same time
        if (classes[currentClassIndex].name == "fairy" && (hasFairy|| otherPlayer.isDead))
        {
            UpdateCurrentClassIndex(changedDownwards);
        }

        // Instantiates the new GameObject
        playerObject = Instantiate(classes[currentClassIndex],
            newPlayerTransform.position,
            newPlayerTransform.rotation,
            gameObject.transform) as GameObject;
        SetAxis();

        // The player now is a fairy
        if (classes[currentClassIndex].name == "fairy")
        {
            hasFairy = true;
        }

        // Sets the target for the fairy
        SetFairyTarget();

        playerObject.GetComponentInChildren<MovingObj>()._hitpoints = PlayerPrefs.GetInt("HP" + gameObject.name);

        // Setting the new player as target of the camera
        cameraControl.SetTarget(playerNumber - 1, playerObject);

        // Change the portrait to fit the new class
        UpdateClassUI();
        cdmanager.StartChangeClassCD();
        yield return new WaitForSeconds(0.25f);
        Subject.Notify("Player changed class");
        Subject.Notify("Player changed class", currentClassIndex);
    }

    public void SavePlayerState()
    {
        PlayerPrefs.SetInt("classIndex" + gameObject.name, currentClassIndex);
        PlayerPrefs.SetInt("HP" + gameObject.name, gameObject.GetComponentInChildren<MovingObj>()._hitpoints);
        int fairy = 0;
        if (hasFairy) fairy = 1;
        PlayerPrefs.SetInt("hasFairy", fairy);
    }

    // Gets called before changing class
    private void UpdateCurrentClassIndex(bool changedDown)
    {
        if (changedDown)
        {
            currentClassIndex--;
            if (currentClassIndex == -1)
            {
                currentClassIndex = 2;
            }
            return;
        }
        else
        {
            currentClassIndex++;
            if (currentClassIndex == 3)
            {
                currentClassIndex = 0;
            }
        }
    }

    /* 
     * This method first determines if anyone is a fairy.
     * If that is the case it determines which player is a fairy and 
     * then sets the target of the fairy accordingly.
     */
    private void SetFairyTarget()
    {
        if (hasFairy)
        {
            if (classes[currentClassIndex].name == "fairy")
            {
                // The player is a fairy and the target needs to be set
                playerObject.GetComponent<Fairy>().target = OtherPlayer().GetComponent<Player>();
            }
            else
            {
                // The other player is a fairy and the target needs to be set
                OtherPlayer().GetComponent<Fairy>().target = playerObject.GetComponent<Player>();
            }


        }
    }

    // This method can be used to get the GameObject of the other Player
    private GameObject OtherPlayer()
    {
        if (playerNumber == 1)
        {
            return GameObject.Find("Player2").transform.GetChild(0).gameObject;
        }
        else
        {
            return GameObject.Find("Player1").transform.GetChild(0).gameObject;
        }
    }

    // This method is used to change the portrais of the player to fit the current class
    private void ChangePortrait()
    {

    }

    private void LateUpdate()
    {
        if (!thisPlayer.isDead)
        {
            if (Input.GetButtonDown(firstSpecialAbility))
            {
                firstAbility();
            }
            if (Input.GetButtonDown(secondSpecialAbility))
            {
                secondAbility();
            }
            if (Input.GetButtonDown(speedBoostInput))
            {
                thirdAbility();
            }
        }
    }

    private void firstAbility()
    {
            Player player = gameObject.GetComponentInChildren<Player>();
            player.AttemptSpecialAbility();
    }

    private void secondAbility()
    {
        Player player = gameObject.GetComponentInChildren<Player>();
        player.AttemptSecondSpecialAbility();
    }

    private void thirdAbility()
    {
        Player player = gameObject.GetComponentInChildren<Player>();
        player.AttemptThirdSpecialAbility();
    }

    public void OnNotify(string gameEvent)
    {
        switch (gameEvent)
        {
            case Constants.NEXT_LEVEL:
                SavePlayerState();
                break;
            case Constants.PLAYER_DIED:
                checkForPlayersDeath();
                break;
            case Constants.PLAYER_CHANGED_CLASS:
                thisPlayer = gameObject.GetComponentInChildren<Player>();
                otherPlayer = OtherPlayer().GetComponentInChildren<Player>();
                break;
        }
    }

    private void checkForPlayersDeath()
    {
        if(thisPlayer.isDead && otherPlayer.isDead)
        {
            Subject.Notify(Constants.ALL_PLAYERS_DEAD);
        }
    }

    public void ShowBuffIcons(int buffIndex, float buffDuration)
    {
        OtherPlayer().GetComponentInParent<UIManager>().StartBuffCoolDown(buffIndex, buffDuration, OtherPlayer().GetComponentInParent<PlayerManager>().currentClassIndex);
    }

    private void OnDestroy()
    {
        Subject.RemoveObserver(this);
    }

    public bool getHasFairy()
    {
        return hasFairy;
    }
}
