﻿using UnityEngine;

public class BhanuPrefs : MonoBehaviour
{
    const string CHIMPIONSHIPS_COUNT_KEY = "NumberOfTimesChimpion";
    const string CURRENT_CHIMPIONSHIP_STATUS_KEY = "CurrentChimpionshipStatus";
    const string FIRST_TIME_JUMP_TUTORIAL_KEY = "FirstTimeJumpTutorial";
    const string FIRST_TIME_SLIDE_TUTORIAL_KEY = "FirstTimeSlideTutorial";
    const string FIRST_TIME_UI_BUTTONS_TUTORIAL_KEY = "FirstTimeUIButtonsTutorial";
    const string HIGH_SCORE_KEY = "HighScore";
    const string SOUNDS_STATUS_KEY = "SoundsStatus";
    const string SUPERS_KEY = "Supers";

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void DeleteScore()
    {
        PlayerPrefs.DeleteKey(HIGH_SCORE_KEY);
    }

    public static int GetChimpionshipsCount()
    {
        if(PlayerPrefs.HasKey(CHIMPIONSHIPS_COUNT_KEY))
        {
            return PlayerPrefs.GetInt(CHIMPIONSHIPS_COUNT_KEY);
        }

        return 0;
    }

    public static int GetCurrentChimpionshipStatus()
    {
        if(PlayerPrefs.HasKey(CURRENT_CHIMPIONSHIP_STATUS_KEY))
        {
            return PlayerPrefs.GetInt(CURRENT_CHIMPIONSHIP_STATUS_KEY);
        }

        return 0;
    }

    public static int GetFirstTimeJumpTutorialStatus()
    {
        if(PlayerPrefs.HasKey(FIRST_TIME_JUMP_TUTORIAL_KEY))
        {
            return PlayerPrefs.GetInt(FIRST_TIME_JUMP_TUTORIAL_KEY);
        }

        return 0;
    }

    public static int GetFirstTimeSlideTutorialStatus()
    {
        if(PlayerPrefs.HasKey(FIRST_TIME_SLIDE_TUTORIAL_KEY))
        {
            return PlayerPrefs.GetInt(FIRST_TIME_SLIDE_TUTORIAL_KEY);
        }

        return 0;
    }

    public static int GetFirstTimeUIButtonsTutorialStatus()
    {
        if(PlayerPrefs.HasKey(FIRST_TIME_UI_BUTTONS_TUTORIAL_KEY))
        {
            return PlayerPrefs.GetInt(FIRST_TIME_UI_BUTTONS_TUTORIAL_KEY);
        }

        return 0;
    }
		
    public static float GetHighScore()
    {
        if(PlayerPrefs.HasKey(HIGH_SCORE_KEY))
        {
            return PlayerPrefs.GetFloat(HIGH_SCORE_KEY);
        }

        return 0;
    }

    public static int GetSoundsStatus()
    {
        if(PlayerPrefs.HasKey(SOUNDS_STATUS_KEY))
        {
            return PlayerPrefs.GetInt(SOUNDS_STATUS_KEY);
        }

        return 0;
    }

    public static int GetSupers()
    {
        if(PlayerPrefs.HasKey(SUPERS_KEY))
        {
            return PlayerPrefs.GetInt(SUPERS_KEY);
        }

        return 0;
    }

    public static void SetChimpionshipsCount(int chimpionshipsCount)
    {
        PlayerPrefs.SetInt(CHIMPIONSHIPS_COUNT_KEY , chimpionshipsCount);
    }

    public static void SetCurrentChimpionshipStatus(int status)
    {
        PlayerPrefs.SetInt(CURRENT_CHIMPIONSHIP_STATUS_KEY , status);
    }

    public static void SetFirstTimeJumpTutorialStatus(int status)
    {
        PlayerPrefs.SetInt(FIRST_TIME_JUMP_TUTORIAL_KEY , status);
    }

    public static void SetFirstTimeSlideTutorialStatus(int status)
    {
        PlayerPrefs.SetInt(FIRST_TIME_SLIDE_TUTORIAL_KEY , status);
    }

    public static void SetFirstTimeUIButtonsTutorialStatus(int status)
    {
        PlayerPrefs.SetInt(FIRST_TIME_UI_BUTTONS_TUTORIAL_KEY , status);
    }

    public static void SetHighScore(float score)
    {
        PlayerPrefs.SetFloat(HIGH_SCORE_KEY , score);
    }

    public static void SetSoundsStatus(int mute)
    {
        PlayerPrefs.SetInt(SOUNDS_STATUS_KEY , mute);
    }

    public static void SetSupers(int supers)
    {
        PlayerPrefs.SetInt(SUPERS_KEY , supers);
    }
}