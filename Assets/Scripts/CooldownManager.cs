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
    private float[] warriorAbilityCooldowns = { 1f, 10f, 10f }; //Attack, defensiveState, revive
    private float[] rangerAbilityCooldowns = { 1f, 5f, 10f }; //Attack, multiShot, revive
    private float[] fairyAbilityCooldowns = { 2f, 15f, 15f, 18f }; //Attack, fire, ice, speed (all are duration + cooldown)
    private float[] buffDurations = { 5f, 5f, 8f }; //Fire, ice & speedbuff
    private UIManager uiManager;


    // Use this for initialization
    void Awake () { 
	}

    private void Start()
    {
        uiManager = GetComponentInParent<UIManager>();
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
        if (playerClassIndex == 0) //Warrior
        {
            warriorOnCooldown[cooldownIndex] = true;
            uiManager.StartSkillCooldown(cooldownIndex, warriorAbilityCooldowns[cooldownIndex]);
            yield return new WaitForSeconds(warriorAbilityCooldowns[cooldownIndex]);
            warriorOnCooldown[cooldownIndex] = false;
            Subject.Notify("WarriorCDOver", cooldownIndex);
        }
        else if (playerClassIndex == 1) //Ranger
        {
            rangerOnCooldown[cooldownIndex] = true;
            uiManager.StartSkillCooldown(cooldownIndex, rangerAbilityCooldowns[cooldownIndex]);
            yield return new WaitForSeconds(rangerAbilityCooldowns[cooldownIndex]);
            rangerOnCooldown[cooldownIndex] = false;
            Subject.Notify("RangerCDOver", cooldownIndex);
        }
        else if (playerClassIndex == 2) //Fairy
        {
            fairyOnCooldown[cooldownIndex] = true;
            uiManager.StartSkillCooldown(cooldownIndex, fairyAbilityCooldowns[cooldownIndex]);
            yield return new WaitForSeconds(fairyAbilityCooldowns[cooldownIndex]);
            fairyOnCooldown[cooldownIndex] = false;
            Subject.Notify("FairyCDOver", cooldownIndex);
        }
        else //Target of fairy buff
        {
            buffOnCooldown = true;
            gameObject.GetComponent<PlayerManager>().ShowBuffIcons(cooldownIndex - 1, buffDurations[cooldownIndex - 1]);
            yield return new WaitForSeconds(buffDurations[cooldownIndex-1]);
            buffOnCooldown = false;
            Subject.Notify("BuffOver", cooldownIndex);
        }
    }

    public void StartChangeClassCD()
    {
        StartCoroutine(StartClassCD());
    }

    private IEnumerator StartClassCD()
    {
        changeClassOnCooldown = true;
        yield return new WaitForSeconds(Constants.PLAYER_CLASS_CHANGE_COOLDOWN);
        changeClassOnCooldown = false;
    }
}
