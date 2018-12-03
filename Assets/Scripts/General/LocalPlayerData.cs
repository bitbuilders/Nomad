﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerData : MonoBehaviour
{
    public Player LocalPlayer { get; set; }
    public string TempUsername { get; set; }
    public string TempColor { get; set; }

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
            if (p.UserName == playerName && p != LocalPlayer)
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
            if (p.UserName == localUser)
            {
                multiple = true;
                break;
            }
        }

        return multiple;
    }

    public Player FindPlayerWithID(int id)
    {
        Player player = null;

        Player[] players = FindObjectsOfType<Player>();
        foreach (Player p in players)
        {
            if (p.ID == id)
            {
                player = p;
                break;
            }
        }

        return player;
    }
}
