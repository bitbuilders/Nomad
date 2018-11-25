using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NomadNetworkDiscovery : NetworkDiscovery
{
    private void Awake()
    {
        //Initialize();
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        base.OnReceivedBroadcast(fromAddress, data);
        
        PlayerGameManager.Instance.RefreshList();
        foreach (var key in broadcastsReceived.Keys)
        {
            string hostName = System.Text.Encoding.Unicode.GetString(broadcastsReceived[key].broadcastData);
            string address = broadcastsReceived[key].serverAddress.Replace("::ffff:", "");
            print("Broadcast from Host: " + hostName + ", IP: " + address);
            PlayerGameManager.Instance.CreatePlayerGame(address, hostName);
        }
    }
}
