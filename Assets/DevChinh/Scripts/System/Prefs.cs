using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefs
{
   public static int LEVEL
    {
        get => PlayerPrefs.GetInt(SystemVariable.LEVEL);
        set
        {
            PlayerPrefs.SetInt(SystemVariable.LEVEL,value);
        }
    }

    public static int COINT
    {
        get => PlayerPrefs.GetInt(SystemVariable.COINT);
        set
        {
            PlayerPrefs.SetInt(SystemVariable.COINT, value);
        }
    }


    public static int LIFE
    {
        get => PlayerPrefs.GetInt(SystemVariable.LIFE);
        set
        {
            PlayerPrefs.SetInt(SystemVariable.LIFE, value);
        }
    }

    public static int SKIN
    {
        get => PlayerPrefs.GetInt(SystemVariable.SKIN);
        set
        {
            PlayerPrefs.SetInt(SystemVariable.SKIN, value);
        }
    }

    public static int DAME
    {
        get => PlayerPrefs.GetInt(SystemVariable.DAME);
        set
        {
            PlayerPrefs.SetInt(SystemVariable.DAME, value);
        }
    }
    public static int SPEED
    {
        get => PlayerPrefs.GetInt(SystemVariable.SPEED);
        set
        {
            PlayerPrefs.SetInt(SystemVariable.SPEED, value);
        }
    }

    public static float TIMEATTACK
    {
        get => PlayerPrefs.GetFloat(SystemVariable.TIMEATTACK);
        set
        {
            PlayerPrefs.SetFloat(SystemVariable.TIMEATTACK, value);
        }
    }

    public static float TIMETELE
    {
        get => PlayerPrefs.GetFloat(SystemVariable.TIMETELEPORT);
        set
        {
            PlayerPrefs.SetFloat(SystemVariable.TIMETELEPORT, value);
        }
    }
}
