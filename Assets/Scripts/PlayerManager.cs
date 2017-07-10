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
    public GameObject nextClassPrefab;
    public GameObject alternativePrefab;

    // Use this for initialization
    private void Start () {
        _hitpoints = 100;
        hasFairy = false;
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
        GameObject newPlayer = Instantiate(classes[0], gameObject.transform) as GameObject;
        playerTransform = newPlayer.transform;
        newPlayer.tag = "Player" + playerNumber;
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
        playerTransform = gameObject.transform.GetChild(0);
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
            //ChangeClass(playerIndex);
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
     * This method exchanges the GameObject the script is attached to (Player1 or Player2) with
     * an instance of the prefab that has been set as the nextClassPrefab(Warrior/Ranger/Fairy) in the Unity inspector.
     * It afterwards sets the new player GameObject as the new target of the camera.
     */
    public void ChangeClass(int index)
    {
        GameObject otherPlayer;
        if (index == 0)
        {
            otherPlayer = GameObject.FindGameObjectWithTag("Player2");
        }
        else
        {
            otherPlayer = GameObject.FindGameObjectWithTag("Player1");
        }

        // This prohibits both players being a fairy
        if (nextClassPrefab.name == "fairy" && otherPlayer.GetComponent<Fairy>())
        {
            nextClassPrefab = alternativePrefab;
        }

        // Setting the correct player tag
        nextClassPrefab.tag = gameObject.tag;

        // Instanzietes the new GameObject
        Destroy(this.gameObject);
        GameObject newPlayer = Instantiate(nextClassPrefab,
            gameObject.transform.position,
            gameObject.transform.rotation,
            gameObject.transform.parent) as GameObject;

        if (otherPlayer.GetComponent<Fairy>())
        {
            // The other player is a fairy and the target needs to be set
            otherPlayer.GetComponent<Fairy>().target = newPlayer.GetComponent<MovingObj>();
        }

        if (nextClassPrefab.name == "fairy")
        { //Quick and dirty method - should be down better later
            if (index == 0)
            {
                newPlayer.GetComponent<Fairy>().target = GameObject.FindGameObjectWithTag("Player2").GetComponent<MovingObj>();
            }
            if (index == 1)
            {
                newPlayer.GetComponent<Fairy>().target = GameObject.FindGameObjectWithTag("Player1").GetComponent<MovingObj>();
            }
        }

        // Setting the new player as the new target of the camera
        GameObject cameraRig = GameObject.Find("CameraRig");
        cameraRig.GetComponent<CameraControl>().SetTarget(index, newPlayer);
        ChangePortrait();
    }


    protected void ChangePortrait()
    {
    }
}
