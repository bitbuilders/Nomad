using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Player : NetworkBehaviour
{
    static int id = 0;

    private void Start()
    {
        id++;

        if (isLocalPlayer)
        {
            RpcRecieveMessage("Worked");
        }
    }

    [ClientRpc]
    public void RpcRecieveMessage(string msg)
    {
        Debug.Log(id + " recieved message: " + msg);
    }
}
