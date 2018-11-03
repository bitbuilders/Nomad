using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerData : MonoBehaviour
{
    public Player LocalPlayer { get; set; }
    public string TempUsername { get; set; }

    public static LocalPlayerData Instance;

    private void Start()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(Player localPlayer)
    {
        LocalPlayer = localPlayer;
        LocalPlayer.UserName = TempUsername;
    }

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
