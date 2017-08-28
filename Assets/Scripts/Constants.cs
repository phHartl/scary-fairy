using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Constants : MonoBehaviour {

    //EVENTS & TAGS
    public const string PLAYER_FOREGROUND = "PlayerForeground";
    public const string PLAYER_BACKGROUND = "PlayerBackground";
    public const string NEXT_LEVEL = "Next Level";
    public const string PLAYER_DIED = "Player Died";
    public const string PLAYER_CHANGED_CLASS = "Player changed class";
    public const string ALL_PLAYERS_DEAD = "All Players Dead";
    public const string MAIN_MENU = "Main Menu";
    public const string CURRENT_LEVEL = "Current Level";
    public const string INSTRUCTIONS = "Instructions";
    public const string CASUAL_ENEMY = "CasualEnemy";
    public const string HEALTH_POTION_DROPPED = "HealthPotionDropped";
    public const string ICE_ENCHANTMENT = "ICE_ENCHANTMENT";
    public const string FIRE_ENCHANTMENT = "FIRE_ENCHANTMENT";
    public const string HEALTH_PICKUP = "HealthPickup";
    public const string BUFF_OVER = "BuffOver";
    public const string PLAYER_TAG = "Player";
    public const string FEET_HITBOX = "feetHitbox";
    public const string FAIRY_CD_OVER = "FairyCDOver";
    public const string WARRIOR_CD_OVER = "WarriorCDOver";
    public const string RANGER_CD_OVER = "RangerCDOver";
    public const string MONSTER_FOREGROUND = "MonsterForeground";
    public const string MONSTER_MIDDLE = "MonsterMiddle";
    public const string MONSTER_BACKGROUND = "MonsterBackground";




    //PLAYER
    public const int PLAYER_MAX_HITPOINTS = 100;
    public const int MINIMAL_HP_TO_REVIVE = PLAYER_MAX_HITPOINTS / 4;
    public const int PLAYER_FACING_NORTH = 1;
    public const int PLAYER_FACING_EAST = 2;
    public const int PLAYER_FACING_SOUTH = 3;
    public const int PLAYER_FACING_WEST = 4;

    public const float PLAYER_REVIVE_COOLDOWN = 10f;
    public const float PLAYER_DEFAULT_MOVEMENTSPEED = 5f;
    public const float CAMERA_MAX_VERTICAL_DISTANCE = 8f;
    public const float CAMERA_MAX_HORIZONTAL_DISTANCE = 12f;
    public const float PLAYER_INVINCIBILITY_ON_HIT = 1f;

    


    //RANGER
    public const int RANGER_BASE_DAMAGE = 15;
    public const int RANGER_MULTISHOT_ARROW_COUNT = 7;
    public const int RANGER_MULTISHOT_ANGLE = 9;
    public const int RANGER_CLASS_INDEX = 1;

    public const float RANGER_ATTACK_COOLDOWN = 1f;
    public const float RANGER_MULTISHOT_COOLDOWN = 5f;
    public const float RANGER_ARROW_TRAVEL_TIME = 1f;

    

    //WARRIOR
    public const int WARRIOR_BASE_DAMAGE = 20;
    public const int WARRIOR_CLASS_INDEX = 0;

    public const float WARRIOR_ATTACK_COOLDOWN = 1f;
    public const float WARRIOR_SHIELD_DAMAGE_REDUCTION = 0.8f;
    public const float WARRIOR_DEFENSIVE_DEBUFF = 0.5f;
    public const float WARRIOR_BASE_DAMAGE_REDUCTION = 0f;  //Reduction of damage that the Warrior TAKES
    public const float WARRIOR_SHIELD_COOLDOWN = 10f;
    public const float WARRIOR_SHIELD_DURATION = 5f;

   

    //FAIRY
    public const int FAIRY_BASE_DAMAGE = 20;
    public const int ICE_ENCHANTMENT_DAMAGE_MULTIPLIER = 2;
    public const int FIRE_ENCHANTMENT_DAMAGE_MULTIPLIER = 2;
    public const int FAIRY_CLASS_INDEX = 2;
    public const int FAIRY_BUFF_TARGET_INDEX = 3;
    public const int FAIRY_BURN_TICK_DAMAGE = 5;

    public const float FAIRY_ATTACK_COOLDOWN = 2f;
    public const float FAIRY_FIRE_COOLDOWN = 15f;
    public const float FAIRY_FIRE_BUFF_DURATION = 5f;
    public const float BURN_DAMAGE_DURATION = 4f;
    public const float BURN_DAMAGE_TICKRATE = 0.5f;
    public const float FAIRY_ICE_COOLDOWN = 15f;
    public const float FAIRY_ICE_BUFF_DURATION = 5f;
    public const float FAIRY_SPEED_COOLDOWN = 18f;
    public const float FAIRY_SPEED_BUFF_DURATION = 8f;
    public const float FAIRY_SPEED_BOOST_MULTIPLIER = 1.5f;

    

    //ENEMIES
    public const int CASUAL_ENEMY_MAX_HEALTH = 100;
    public const int CASUAL_ENEMY_BASE_DAMAGE = 15;

    public const float ICE_ENCHANTMENT_SLOW_MODIFIER = 0.5f;

    //GOLEM
    public const int GOLEM_BASE_DAMAGE = 30;
    public const int GOLEM_BASE_HEALTH = 250;

    public const float GOLEM_PROJECTILE_MIN_TRAVEL_TIME = 2f;
    public const float GOLEM_PROJECTILE_MAX_TRAVEL_TIME = 4f;
    public const float GOLEM_TIME_BETWEEN_PROJECTILES = 6f;


    //HEALTH POTION
    public const int HEALTH_POTION_RECOVERY = 5;

    public const float HEALTH_POTION_DROP_CHANCE = 0.3f;
}
