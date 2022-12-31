using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemVariable
{
    //Static
    public static GameController gameController;
    public static BossController bossController;
    public static PlayerController playerController;
    public static AudioController audioController;

    public static int quantityCurrentHeart;

    //Const
    public const string LEVEL = "Level";
    public const string COINT = "Coint";
    public const string LIFE = "Life";
    public const string SKIN = "Skin";
    public const string DAME = "Dame";
    public const string SPEED = "Speed";
    public const string TIMEATTACK = "TimeAttack";
    public const string TIMETELEPORT = "TimeTeleport";
}

//Enum
public enum stateOfPlayer
{
    Appear,
    Run,
    IsJumpingUp,
    IsJumpingDown,
    Teleport,
    Attack,
    Attack2,
    Win,
    Death
}

public enum stateOfSkin
{
    MoveLeft,
    MoveRight
}

enum stateOfBoss
{
    Walk,
    Attack,
    UseSkill1,
    UseSkill2,
    BeAttacked,
    Death
}
public enum Tag
{
    Player,
    Enemy,
    Rubi,
    Dart
}

public enum stateOfGame
{
    Play,
    Pause,
    Win,
    GameOver
}

public enum DialogAnim
{
    Open
}

public enum Cards
{
    Open
}


