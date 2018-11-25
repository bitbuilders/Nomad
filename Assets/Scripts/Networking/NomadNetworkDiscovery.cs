using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NomadNetworkDiscovery : NetworkDiscovery
{
    private void Awake()
    {
        Initialize();
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        base.OnReceivedBroadcast(fromAddress, data);

        print("Received broadcast from: " + fromAddress + " with the data: " + data);
        
        foreach (var key in broadcastsReceived.Keys)
        {
            string hostName = Convert.ToBase64String(broadcastsReceived[key].broadcastData);
            string address = broadcastsReceived[key].serverAddress;
            print("Host: " + hostName + ", IP: " + address);
            //PlayerGameManager.Instance.CreatePlayerGame(address, hostName);
        }
    }
}
