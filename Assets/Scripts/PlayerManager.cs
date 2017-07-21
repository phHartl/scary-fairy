using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public string axisVertical;
    public string axisHorizontal;
    public string attackInput;
    public string changeClassInput;
    public string enchantFireInput;
    public string enchantIceInput;
    public string speedBoostInput;
    private GameObject playerObject;
    public int playerNumber;
    // The hasFairy-attribute is used to check and set if any player is a fairy or not
    private static bool hasFairy;
    public GameObject[] classes;
    private int currentClassIndex;
    private CameraControl cameraControl;

    // Use this for initialization
    private void Awake () {
        Subject.AddObserver(this);
        hasFairy = false;
        cameraControl = GameObject.Find("CameraRig").GetComponent<CameraControl>();
        currentClassIndex = 0;
        InitPlayerObject();
        SetAxis();
    }

    // This method initializes a new player child-object depending on the first entry of the classes array
    private void InitPlayerObject()
    {   
        // returns player state to the last saved state, if there is one
        if (PlayerPrefs.HasKey("HP" + gameObject.name))
        {
            currentClassIndex = PlayerPrefs.GetInt("classIndex" + gameObject.name);
            playerObject = Instantiate(classes[currentClassIndex], gameObject.transform) as GameObject;
            if (PlayerPrefs.GetInt("hasFairy") == 1)
            {
                hasFairy = true;
                // broken
                SetFairyTarget();
            }
            gameObject.GetComponentInChildren<MovingObj>()._hitpoints = PlayerPrefs.GetInt("HP" + gameObject.name);
            ChangePortrait();

        }
        else if (gameObject.GetComponentInChildren<Player>())
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
        if (Input.GetButtonDown(changeClassInput))
        {
            StartCoroutine(ChangeClass());
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
    private IEnumerator ChangeClass()
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
        UpdateCurrentClassIndex();

        // This prohibits both players from being a fairy at the same time
        if (classes[currentClassIndex].name == "fairy" && hasFairy)
        {
            UpdateCurrentClassIndex();
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

    public void SavePlayerState()
    {
        PlayerPrefs.SetInt("classIndex" + gameObject.name, currentClassIndex);
        PlayerPrefs.SetInt("HP" + gameObject.name, gameObject.GetComponentInChildren<MovingObj>()._hitpoints);
        int fairy = 0;
        if (hasFairy) fairy = 1;
        PlayerPrefs.SetInt("hasFairy", fairy);
    }

    // Gets called before changing class
    private void UpdateCurrentClassIndex()
    {
        currentClassIndex++;
        if (currentClassIndex == 3)
        {
            currentClassIndex = 0;
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
        if (Input.GetButtonDown(enchantFireInput))
        {
            EnchantFire();
        }

        // Ice Enchant
        if (Input.GetButtonDown(enchantIceInput))
        {
            EnchantIce();
        }

        //Speed Boost
        if (Input.GetButtonDown(speedBoostInput))
        {
            SpeedBoost();
        }
    }

    private void EnchantFire()
    {
        if (gameObject.transform.GetComponentInChildren<Fairy>())
        {
            Fairy fairy = gameObject.transform.GetComponentInChildren<Fairy>();
            fairy.StartCoroutine(fairy.applyFireEnchantment());
        }
    }

    private void EnchantIce()
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

    public void OnNotify(string gameEvent)
    {
        switch (gameEvent)
        {
            case "Next Level":
                SavePlayerState();
                break;
        }
    }


    private void OnDestroy()
    {
        Subject.RemoveObserver(this);
    }
}
