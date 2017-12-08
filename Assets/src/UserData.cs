using System;
using UnityEngine;

static public class UserData
{
    static UserData ()
    {
    }
        

    static public int bestScore
    {
        get
        {
            string key = "bestScore";
            if (PlayerPrefs.HasKey(key) == false)
            {
                PlayerPrefs.SetInt(key, 0);
            }
            return PlayerPrefs.GetInt(key); 
        }
        set
        {
            if (bestScore < value)
            {
                PlayerPrefs.SetInt("bestScore", value);
            } 
        }
    }

    static public void save()
    {
        PlayerPrefs.Save();
    }

    static public bool soundIsOn
    {
        get
        {
            string key = "sound";
            if (PlayerPrefs.HasKey(key) == false)
            {
                PlayerPrefs.SetInt(key, 1);
            }
            return PlayerPrefs.GetInt(key) == 1; 
        }
        set
        {
            PlayerPrefs.SetInt("sound", value ? 1 : 0); 
        }
    }

    static public bool adRemoved
    {
        get
        {
            string key = "adRemoved";
            if (PlayerPrefs.HasKey(key) == false)
            {
                PlayerPrefs.SetInt(key, 0);
            }
            return PlayerPrefs.GetInt(key) == 1; 
        }
        set
        {
            PlayerPrefs.SetInt("adRemoved", value ? 1 : 0); 
        }
    }

    static public bool gamecenterLogged
    {
        get
        {
            string key = "gamecenterLogged";
            if (PlayerPrefs.HasKey(key) == false)
            {
                PlayerPrefs.SetInt(key, 0);
            }
            return PlayerPrefs.GetInt(key) == 1; 
        }
        set
        {
            PlayerPrefs.SetInt("gamecenterLogged", value ? 1 : 0);
        }
    }

    static public int undoCnt
    {
        get
        {
            string key = "undoCnt";
            if (PlayerPrefs.HasKey(key) == false)
            {
                PlayerPrefs.SetInt(key, 5);
            }
            return PlayerPrefs.GetInt(key); 
        }
        set
        {
            PlayerPrefs.SetInt("undoCnt", value);
        }
    }

    static public bool isFirstPlay
    {
        get
        {
            string key = "isFirstPlay";
            bool result = PlayerPrefs.HasKey(key) == false;
            if (result)
                PlayerPrefs.SetInt("isFirstPlay", 1);

            return result;
        }
    }

}

