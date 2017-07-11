using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private int _hitpoints;
    public string axisVertical;
    public string axisHorizontal;
    public string attackInput;
    public string changeClassInput;
    public string enchantFireInput;
    public string enchantIceInput;
    private Transform playerTransform;
    public int playerNumber;
    private static bool hasFairy;
    public GameObject[] classes;
    private int currentClassIndex;
    private CameraControl cameraControl;

    // Use this for initialization
    private void Start () {
        _hitpoints = 100;
        hasFairy = false;
        cameraControl = GameObject.Find("CameraRig").GetComponent<CameraControl>();
        currentClassIndex = 0;
        if (gameObject.GetComponentInChildren<Player>())
        {
            playerTransform = gameObject.transform.GetChild(0);
        } else
        {
            // No child-object
            InitPlayer();
        }
        SetAxis();
    }

    // This method initializes a new player child-object depending on the first entry of the classes array
    private void InitPlayer()
    {
        GameObject newPlayer = Instantiate(classes[currentClassIndex], gameObject.transform) as GameObject;
        playerTransform = newPlayer.transform;
    }

    // Gets called before changing class
    private void UpdateCurrentClassIndex()
    {
        currentClassIndex++;
        if (currentClassIndex==3)
        {
            currentClassIndex = 0;
        }
    }

    // This methods sets the Axis for the Player
    private void SetAxis()
    {
        Player player = playerTransform.GetComponent<Player>();
        player.axisHorizontal = axisHorizontal;
        player.axisVertical = axisVertical;
    }

    /* Update is called once per frame
     * 
     * Class Changing for Player1 has to be done in Update and
     * Class Changing for Player 2 has to done in LateUpdate in order
     * to fix both players being able to change to fairy when both buttons
     * are pressed within the same update-cycle
     */
    private void Update () {
        playerTransform = gameObject.transform.GetChild(0); //needs to be removed as soon as ChangeClass is implemented again

        // Attack
        if (Input.GetButtonDown(attackInput))
        {
            Player player = playerTransform.GetComponent<Player>();
            player.AttemptAttack(); //Coroutines don't need to be finished within the updateframe
        }

        // Change Class
        if (Input.GetButtonDown(changeClassInput))
        {
            Player player = playerTransform.GetComponent<Player>();
            ChangeClass();
            ChangePortrait();
        }
    }

    private void LateUpdate()
    {
        // Fire Enchant
        if (Input.GetButtonDown(enchantFireInput))
        {
            if (gameObject.transform.GetComponentInChildren<Fairy>())
            {
                Fairy fairy = gameObject.transform.GetComponentInChildren<Fairy>();
                fairy.EnchantAttacks(false);
            }
        }

        // Ice Enchant
        if (Input.GetButtonDown(enchantIceInput))
        {
            if (gameObject.transform.GetComponentInChildren<Fairy>())
            {
                Fairy fairy = gameObject.transform.GetComponentInChildren<Fairy>();
                fairy.EnchantAttacks(true);
            }
        }
    }

    /*
     * This method exchanges the Child of the GameObject the script is attached to (Player1 or Player2) with
     * an instance of the prefab that has been set in the classes-array in the Unity inspector.
     * The classes get changed according to their index in the array (0 - first).
     * It afterwards sets the new Player-GameObject as the new target of the camera.
     */
    private void ChangeClass()
    {
        GameObject otherPlayer;
        if (playerNumber == 1)
        {
            otherPlayer = GameObject.Find("Player2").transform.GetChild(0).gameObject;
        }
        else
        {
            otherPlayer = GameObject.Find("Player1").transform.GetChild(0).gameObject;
        }

        // The transform of the player gets saved, afterwards the old Player-Gameobject gets destroyed
        Transform newPlayerTransform = playerTransform;
        Destroy(playerTransform.gameObject);

        // Updates the new Index
        UpdateCurrentClassIndex();

        // This prohibits both players from being a fairy at the same time
        if (classes[currentClassIndex].name == "fairy" && otherPlayer.GetComponent<Fairy>())
        {
            UpdateCurrentClassIndex();
        }

        // Instanzietes the new GameObject
        GameObject newPlayer = Instantiate(classes[currentClassIndex],
            newPlayerTransform.position,
            newPlayerTransform.rotation,
            gameObject.transform) as GameObject;
        playerTransform = newPlayer.transform;
        SetAxis();

        if (otherPlayer.GetComponent<Fairy>())
        {
            // The other player is a fairy and the target needs to be set
            otherPlayer.GetComponent<Fairy>().target = newPlayer.GetComponent<MovingObj>();
        }

        if (classes[currentClassIndex].name == "fairy")
        { //Quick and dirty method - should be down better later
            if (playerNumber == 1)
            {
                newPlayer.GetComponent<Fairy>().target = GameObject.Find("Player2").transform.GetChild(0).gameObject.GetComponent<MovingObj>();
            }
            if (playerNumber == 2)
            {
                newPlayer.GetComponent<Fairy>().target = GameObject.Find("Player1").transform.GetChild(0).gameObject.GetComponent<MovingObj>();
            }
        }

        // Setting the new player as the new target of the camera
        cameraControl.SetTarget(playerNumber - 1, newPlayer);
    }

    private void ChangePortrait()
    {

    }
}
