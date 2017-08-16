using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownManager : MonoBehaviour {

    private bool[] warriorOnCooldown = new bool[2]; //This needs to be checked by player classes
    private bool[] rangerOnCooldown = new bool[2];
    private bool[] fairyOnCooldown = new bool[4];
    private float[] warriorAbilityCooldowns = { 1f, 5f }; //Attack, defensiveState
    private float[] rangerAbilityCooldowns = { 1f, 5f }; //Attack, multiShot
    private float[] fairyAbilityCooldowns = { 2f, 5f, 5f, 8f }; //Attack, fire, ice, speed

	// Use this for initialization
	void Awake () { 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool[] GetWarriorCooldowns()
    {
        return warriorOnCooldown;
    }

    public bool[] GetRangerCooldowns()
    {
        return rangerOnCooldown;
    }

    public bool[] GetFairyCooldown()
    {
        return fairyOnCooldown;
    }

    public void StartCooldown(int cooldownIndex, int playerClassIndex)
    {
        StartCoroutine(StartCD(cooldownIndex, playerClassIndex));
    }

    private IEnumerator StartCD(int cooldownIndex, int playerClassIndex) //Start a CD based on which class calls this
    {
        if (playerClassIndex == 0) //Warrior
        {
            warriorOnCooldown[cooldownIndex] = true;
            yield return new WaitForSeconds(warriorAbilityCooldowns[cooldownIndex]);
            warriorOnCooldown[cooldownIndex] = false;
            Subject.Notify("WarriorCDOver", cooldownIndex);
        }
        else if (playerClassIndex == 1) //Ranger
        {
            rangerOnCooldown[cooldownIndex] = true;
            yield return new WaitForSeconds(rangerAbilityCooldowns[cooldownIndex]);
            rangerOnCooldown[cooldownIndex] = false;
            Subject.Notify("RangerCDOver", cooldownIndex);
        }
        else //Fairy
        {
            fairyOnCooldown[cooldownIndex] = true;
            yield return new WaitForSeconds(warriorAbilityCooldowns[cooldownIndex]);
            fairyOnCooldown[cooldownIndex] = false;
            Subject.Notify("FairyCDOver", cooldownIndex);
        }
    }
}
