using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/* 
 * Die angezeigten Tasten für Skills aus Inputmanager auslesen und beim initialisieren einfügen?
 */
public class UIManager : MonoBehaviour {

    // Player specific UI-components
    public Image PlayerClassPortrait;
    public Slider PlayerHealthBarSlider;
    public Text PlayerHealthBarText;
    public Image PlayerSwapCooldownImage;
    public Text PlayerSwapCooldownText;
    public Image[] PlayerSkillImages;
    public Image[] PlayerSkillImageCDs;

    // Warrior sprites
    public Sprite WarriorPortrait;
    public Sprite WarriorSkill1;
    public Sprite WarriorSkill1Fire;
    public Sprite WarriorSkill1Ice;
    public Sprite WarriorSkill2;
    public Sprite WarriorSkill2Fire;
    public Sprite WarriorSkill2Ice;

    // Ranger sprites
    public Sprite RangerPortrait;
    // There are no seperate attack icons for ranger?
    public Sprite RangerSkill1;
    public Sprite RangerSkill1Fire;
    public Sprite RangerSkill1Ice;
    public Sprite RangerSkill2;
    public Sprite RangerSkill2Fire;
    public Sprite RangerSkill2Ice;

    // Fairy sprites
    public Sprite FairyPortrait;
    public Sprite FairySkill1;
    public Sprite FairySkill2;
    public Sprite FairySkill3;
    public Sprite FairySkill4;

    // Universal sprites
    public Sprite SkillPlaceholder;

    // Constants
    private const String PLAYER_HEALTH_BAR_TEXT_ATTACHMENT = "/100";

    // Initializes the UI -> nothing is on cooldown
    public void InitUI()
    {
        InitPlayerSwapCooldown();
        InitSkillCooldowns();
    }

    // Initializes the swap-cooldown
    private void InitPlayerSwapCooldown()
    {
        PlayerSwapCooldownImage.fillAmount = 0;
        PlayerSwapCooldownText.text = 0 + "";
    }

    // Initializes the skill cooldowns
    private void InitSkillCooldowns()
    {
        PlayerSkillImageCDs[0].fillAmount = 0;
        PlayerSkillImageCDs[1].fillAmount = 0;
        PlayerSkillImageCDs[2].fillAmount = 0;
        PlayerSkillImageCDs[3].fillAmount = 0;
    }

    // This method updates all parts of the UI related to a specific class (portrait and skills)
    public void UpdateClass(int playerClassIndex)
    {
        if (playerClassIndex == 0) //Warrior
        {
            PlayerClassPortrait.sprite = WarriorPortrait;
            PlayerSkillImages[0].sprite = WarriorSkill1;
            PlayerSkillImages[1].sprite = WarriorSkill2;
            PlayerSkillImages[2].sprite = SkillPlaceholder;
            PlayerSkillImages[3].sprite = SkillPlaceholder;
        }
        else if (playerClassIndex == 1) //Ranger
        {
            PlayerClassPortrait.sprite = RangerPortrait;
            PlayerSkillImages[0].sprite = RangerSkill1;
            PlayerSkillImages[1].sprite = RangerSkill2;
            PlayerSkillImages[2].sprite = SkillPlaceholder;
            PlayerSkillImages[3].sprite = SkillPlaceholder;
        }
        else if (playerClassIndex == 2) //Fairy
        {
            PlayerClassPortrait.sprite = FairyPortrait;
            PlayerSkillImages[0].sprite = FairySkill1;
            PlayerSkillImages[1].sprite = FairySkill2;
            PlayerSkillImages[2].sprite = FairySkill3;
            PlayerSkillImages[3].sprite = FairySkill4;
        }
        //reset skill cooldown visualisation
        InitSkillCooldowns();
    }

    public void StartSwapCooldown(float classChangeCooldown)
    {
        StopAllCoroutines();
        StartCoroutine(UpdatePlayerSwapCooldown(classChangeCooldown, classChangeCooldown));
    }

    // This method updates the visualisation of the swap class cooldown (image and text)
    private IEnumerator UpdatePlayerSwapCooldown(float playerSwapCooldown, float maxPlayerSwapCooldown)
    {
        PlayerSwapCooldownImage.fillAmount = playerSwapCooldown/maxPlayerSwapCooldown;
        PlayerSwapCooldownText.text = playerSwapCooldown+"";
        if (playerSwapCooldown > 0)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(UpdatePlayerSwapCooldown(playerSwapCooldown - 1, maxPlayerSwapCooldown));
        }
    }

    // This method updates the visualisation of the skill cooldowns
   public void StartSkillCooldown(int skillIndex, float skillCooldown)
    {
        StartCoroutine(UpdateSkillCooldown(skillIndex, skillCooldown, skillCooldown));
    }

    private IEnumerator UpdateSkillCooldown(int skillIndex, float skillCooldown, float maxSkillCoolDown)
    {
        PlayerSkillImageCDs[skillIndex].fillAmount = skillCooldown / maxSkillCoolDown;
        if (skillCooldown > 0)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(UpdateSkillCooldown(skillIndex, skillCooldown - 1, maxSkillCoolDown));
        }
    }

    // This method updates the visualisation of the fairy buffs
    public void StartBuffCoolDown(int buffIndex, float buffDuration, int playerClassIndex)
    {
        if (buffIndex < 2)
        {
            StartCoroutine(ShowBuff(buffIndex, buffDuration, playerClassIndex));
        }
    }

    // buff order: Fire -> ice (-> speedbuff)
    private IEnumerator ShowBuff(int buffIndex, float buffDuration, int playerClassIndex)
    {
        if (playerClassIndex == 0) //Warrior
        {
            if (buffIndex == 0)
            {
                PlayerSkillImages[0].sprite = WarriorSkill1Fire;
                PlayerSkillImages[1].sprite = WarriorSkill2Fire;
            } else
            {
                PlayerSkillImages[0].sprite = WarriorSkill1Ice;
                PlayerSkillImages[1].sprite = WarriorSkill2Ice;
            }
            yield return new WaitForSeconds(buffDuration);
            PlayerSkillImages[0].sprite = WarriorSkill1;
            PlayerSkillImages[1].sprite = WarriorSkill2;
        }
        else if (playerClassIndex == 1) //Ranger
        {
            if (buffIndex == 0)
            {
                PlayerSkillImages[0].sprite = RangerSkill1Fire;
                PlayerSkillImages[1].sprite = RangerSkill2Fire;
            } else
            {
                PlayerSkillImages[0].sprite = RangerSkill1Ice;
                PlayerSkillImages[1].sprite = RangerSkill2Ice;
            }
            yield return new WaitForSeconds(buffDuration);
            PlayerSkillImages[0].sprite = RangerSkill1;
            PlayerSkillImages[1].sprite = RangerSkill2;
        }
    }

    // This method updates the healthbar (slider + text)
    public void UpdateHealth(int playerHealth)
    {
        // The slider should be handeled seperately, because it does not need to be called every frame
        PlayerHealthBarSlider.value = playerHealth;
        // Health can be negative (should be fixed elsewhere (limit between 0 and 100?)
        PlayerHealthBarText.text = playerHealth + PLAYER_HEALTH_BAR_TEXT_ATTACHMENT;
    }
}
