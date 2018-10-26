using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerData : Singleton<LocalPlayerData>
{
    public Player LocalPlayer { get; set; }

    public bool PlayerExists(string playerName)
    {
        bool exists = false;

        Player[] players = FindObjectsOfType<Player>();
        foreach (Player p in players)
        {
            if (p.UserName == playerName)
            {
                exists = true;
                break;
            }
        }


        return exists;
    }

    public bool MultipleUsernames()
    {
        bool multiple = false;
        string localUser = LocalPlayer.UserName;

        Player[] players = FindObjectsOfType<Player>();
        foreach (Player p in players)
        {
            if (p.UserName == localUser && p != LocalPlayer)
            {
                multiple = true;
                break;
            }
        }

        return multiple;
    }
}
