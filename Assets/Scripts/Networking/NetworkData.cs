using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkData : NetworkBehaviour
{
    [SyncVar] public int CurrentRoomID;
    [SyncVar] public int CurrentPlayerID;
    //[SyncVar] public List<Player> OnlinePlayers;

    [Server]
    public void IncrementRoomID()
    {
        ++CurrentRoomID;
    }

    [Server]
    public void IncrementPlayerID()
    {
        ++CurrentPlayerID;
    }

    //[Command]
    //public void CmdRegisterPlayer(Player player)
    //{
    //    OnlinePlayers.Add(player);
    //}
}
