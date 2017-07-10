using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    private int _hitpoints;
    public string axisVertical;
    public string axisHorizontal;
    public string attackInput;
    public string changeClassInput;
    public string enchantFireInput;
    public string enchantIceInput;
    private Transform playerTransform;
    public int playerIndex;
    private static bool hasFairy;

    // Use this for initialization
    void Start () {
        _hitpoints = 100;
        hasFairy = false;
        if (gameObject.transform.GetChild(0))
        {
            playerTransform = gameObject.transform.GetChild(0);
        } else
        {
            // No child-object
            InitPlayer();
        }
        SetAxis();
    }

    // This method initializes a new warrior child-object
    private void InitPlayer()
    {
        
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
            player.ChangeClass(playerIndex);
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
            }
        }

        // Ice Enchant
        if (Input.GetButtonDown(enchantIceInput))
        {
            if (gameObject.transform.GetComponentInChildren<Fairy>())
            {
                Fairy fairy = gameObject.transform.GetComponentInChildren<Fairy>();
            }
        }
    }
}
