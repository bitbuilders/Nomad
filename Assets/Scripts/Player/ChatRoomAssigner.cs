using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChatRoomAssigner : NetworkBehaviour
{
    [SyncVar] public int RoomID;

    [Command]
    public void CmdIncrementRoomID()
    {
        ++RoomID;
    }
}
