using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private int _hitpoints;
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
    public Transform otherPlayerTransform;
    private SpriteRenderer renderer;

    [HideInInspector]public const string PLAYER_FOREGROUND = "PlayerForeground";
    [HideInInspector]public const string PLAYER_BACKGROUND = "PlayerBackground";

    // Use this for initialization
    private void Awake () {
        _hitpoints = 100;
        hasFairy = false;
        cameraControl = GameObject.Find("CameraRig").GetComponent<CameraControl>();
        currentClassIndex = 0;
        InitPlayerObject();
        SetAxis();
        renderer = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // This method initializes a new player child-object depending on the first entry of the classes array
    private void InitPlayerObject()
    {
        if (gameObject.GetComponentInChildren<Player>())
        {
            playerObject = gameObject.transform.GetChild(0).gameObject;
        }
        else
        {
            // No child-object
            GameObject newPlayer = Instantiate(classes[currentClassIndex], gameObject.transform) as GameObject;
            playerObject = newPlayer;
        }
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
        // Attack
        if (Input.GetButtonDown(attackInput))
        {
            Attack();
        }

        // Change Class
        if (Input.GetButtonDown(changeClassUpInput) || Input.GetButtonDown(changeClassDownInput))
        {
            bool down = false;
            if (Input.GetButtonDown(changeClassDownInput))
            {
                down = true;
            }
            StartCoroutine(ChangeClass(down));
        }

        checkLayer();
    }


    /*
     * Checks if player is in front of or behind other player by compairing y-coordinates of both players.
     * If current player is behind other player, sets SortingLayer to PLAYER_BACKGROUND, if he is in front sets SortingLayer to PLAYER_FOREGROUND
     */
    private void checkLayer()
    {
        if(otherPlayerTransform.GetChild(0).transform.position.y > gameObject.transform.GetChild(0).transform.position.y)
        {
            renderer.sortingLayerName = PLAYER_FOREGROUND;
        } else
        {
            renderer.sortingLayerName = PLAYER_BACKGROUND;
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
        if (classes[currentClassIndex].name == "fairy" && hasFairy)
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

        // Setting the new player as target of the camera
        cameraControl.SetTarget(playerNumber - 1, playerObject);

        // Change the portrait to fit the new class
        ChangePortrait();
        yield return new WaitForSeconds(0.25f);
        Subject.Notify("Player changed class");
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
                playerObject.GetComponent<Fairy>().target = OtherPlayer().GetComponent<MovingObj>();
            }
            else
            {
                // The other player is a fairy and the target needs to be set
                OtherPlayer().GetComponent<Fairy>().target = playerObject.GetComponent<MovingObj>();
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
        // Fire Enchant
        if (Input.GetButtonDown(firstSpecialAbility))
        {
            firstAbility();
        }

        // Ice Enchant
        if (Input.GetButtonDown(secondSpecialAbility))
        {
            secondAbility();
        }

        //Speed Boost
        if (Input.GetButtonDown(speedBoostInput))
        {
            SpeedBoost();
        }
    }

    private void firstAbility()
    {
        if (gameObject.transform.GetComponentInChildren<Fairy>())
        {
            Fairy fairy = gameObject.transform.GetComponentInChildren<Fairy>();
            fairy.StartCoroutine(fairy.applyFireEnchantment());
        }
        else
        {
            Player player = gameObject.GetComponentInChildren<Player>();
            player.AttemptSpecialAbility();
        }
    }

    private void secondAbility()
    {
        if (gameObject.transform.GetComponentInChildren<Fairy>())
        {
            Fairy fairy = gameObject.transform.GetComponentInChildren<Fairy>();
            fairy.StartCoroutine(fairy.applyIceEnchantment());
        }
    }

    private void SpeedBoost()
    {
        if (gameObject.transform.GetComponentInChildren<Fairy>())
        {
            Fairy fairy = gameObject.transform.GetComponentInChildren<Fairy>();
            fairy.speedBoost();
        }
    }
}
