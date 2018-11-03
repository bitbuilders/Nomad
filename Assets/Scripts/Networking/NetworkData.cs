using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkData : NetworkBehaviour
{
    [SyncVar] public int CurrentRoomID;
    //[SyncVar] public List<Player> OnlinePlayers;

    [Command]
    public void CmdIncrementRoomID()
    {
        ++CurrentRoomID;
    }

    //[Command]
    //public void CmdRegisterPlayer(Player player)
    //{
    //    OnlinePlayers.Add(player);
    //}
}
