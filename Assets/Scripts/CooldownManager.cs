using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownManager : MonoBehaviour {

    public bool[] warriorOnCooldown = new bool[2]; //This needs to be checked by player classes
    public bool[] rangerOnCooldown = new bool[2];
    public bool[] fairyOnCooldown = new bool[4];
    private float[] warriorAbilityCooldowns = { 1f, 5f }; //Attack, defensiveState
    private float[] rangerAbilityCooldowns = { 1f, 5f }; //Attack, multiShot
    private float[] fairyAbilityCooldowns = { 2f, 5f, 5f, 8f }; //Attack, fire, ice, speed

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartCooldown(int cooldownIndex, int playerClassIndex) //Start a CD based on which class calls this
    {
        if(playerClassIndex == 0) //Warrior
        {
            warriorOnCooldown[cooldownIndex] = true;
            StartCoroutine(WaitForCooldown(warriorAbilityCooldowns[cooldownIndex]));
            warriorOnCooldown[cooldownIndex] = false;
            return; //Add event here
        }else if (playerClassIndex == 1) //Ranger
        {
            rangerOnCooldown[cooldownIndex] = true;
            StartCoroutine(WaitForCooldown(rangerAbilityCooldowns[cooldownIndex]));
            rangerOnCooldown[cooldownIndex] = false;
            return;
        }else //Fairy
        {
            fairyOnCooldown[cooldownIndex] = true;
            StartCoroutine(WaitForCooldown(fairyAbilityCooldowns[cooldownIndex]));
            return;
        }
    }

    private IEnumerator WaitForCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
    }
}
