using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownManager : MonoBehaviour {

    private bool[] warriorOnCooldown = new bool[3];
    private bool[] rangerOnCooldown = new bool[3];
    private bool[] fairyOnCooldown = new bool[4];
    private bool buffOnCooldown = false;
    private bool changeClassOnCooldown = false;
    private float[] warriorAbilityCooldowns = { Constants.WARRIOR_ATTACK_COOLDOWN, Constants.WARRIOR_SHIELD_COOLDOWN, Constants.PLAYER_REVIVE_COOLDOWN }; //Attack, defensiveState, revive
    private float[] rangerAbilityCooldowns = { Constants.RANGER_ATTACK_COOLDOWN, Constants.RANGER_MULTISHOT_COOLDOWN, Constants.PLAYER_REVIVE_COOLDOWN }; //Attack, multiShot, revive
    private float[] fairyAbilityCooldowns = { Constants.FAIRY_ATTACK_COOLDOWN, Constants.FAIRY_FIRE_COOLDOWN, Constants.FAIRY_ICE_COOLDOWN, Constants.FAIRY_SPEED_COOLDOWN}; //Attack, fire, ice, speed (all are duration + cooldown)
    private float[] buffDurations = { Constants.FAIRY_FIRE_BUFF_DURATION, Constants.FAIRY_ICE_BUFF_DURATION, Constants.FAIRY_SPEED_BUFF_DURATION }; //Fire, ice & speedbuff
    private float classChangeCooldown = 10f;

	// Use this for initialization
	void Awake () { 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void SetWarriorCooldowns(int cooldownIndex, float multiplier)
    {
        warriorAbilityCooldowns[cooldownIndex] *= multiplier;
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

    public bool GetBuffCooldown()
    {
        return buffOnCooldown;
    }

    public bool GetClassChangeCooldown()
    {
        return changeClassOnCooldown;
    }

    //This function is needed, because IEnumerator are getting stuck when the calling object gets destroyed
    public void StartCooldown(int cooldownIndex, int playerClassIndex)
    {
        StartCoroutine(StartCD(cooldownIndex, playerClassIndex));
    }

    /* Params needed are the index of the corresponding global cooldowns (warrior, ranger, fairy or general buff)
     * the index of the corresponding class which calls this: zero - warrior, one - ranger, two - fairy &
     * any other number for a buff
     * When a cooldown is over an event gets send to the corresponding class(es)
     */
    private IEnumerator StartCD(int cooldownIndex, int playerClassIndex)
    {
        if (playerClassIndex == Constants.WARRIOR_CLASS_INDEX) //Warrior
        {
            warriorOnCooldown[cooldownIndex] = true;
            yield return new WaitForSeconds(warriorAbilityCooldowns[cooldownIndex]);
            warriorOnCooldown[cooldownIndex] = false;
            Subject.Notify(Constants.WARRIOR_CD_OVER, cooldownIndex);
        }
        else if (playerClassIndex == Constants.RANGER_CLASS_INDEX) //Ranger
        {
            rangerOnCooldown[cooldownIndex] = true;
            yield return new WaitForSeconds(rangerAbilityCooldowns[cooldownIndex]);
            rangerOnCooldown[cooldownIndex] = false;
            Subject.Notify(Constants.RANGER_CD_OVER, cooldownIndex);
        }
        else if (playerClassIndex == Constants.FAIRY_CLASS_INDEX) //Fairy
        {
            fairyOnCooldown[cooldownIndex] = true;
            yield return new WaitForSeconds(fairyAbilityCooldowns[cooldownIndex]);
            fairyOnCooldown[cooldownIndex] = false;
            Subject.Notify(Constants.FAIRY_CD_OVER, cooldownIndex);
        }
        else //Target of fairy buff
        {
            buffOnCooldown = true;
            yield return new WaitForSeconds(buffDurations[cooldownIndex-1]);
            buffOnCooldown = false;
            Subject.Notify(Constants.BUFF_OVER, cooldownIndex);
        }
    }

    public void StartChangeClassCD()
    {
        StartCoroutine(StartClassCD());
    }

    private IEnumerator StartClassCD()
    {
        changeClassOnCooldown = true;
        yield return new WaitForSeconds(classChangeCooldown);
        changeClassOnCooldown = false;
    }
}
