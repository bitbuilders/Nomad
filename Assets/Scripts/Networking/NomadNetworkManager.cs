﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NomadNetworkManager : NetworkManager
{
    public static NomadNetworkManager Instance;
    public static int m_currentModel;

    public class NetworkMessage : MessageBase
    {
        public int chosenModel;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        ConnectionManager.Instance.ShowFailMessage();

        base.OnClientDisconnect(conn);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        ConnectionManager.Instance.Hide();

        NetworkMessage test = new NetworkMessage();
        test.chosenModel = m_currentModel;
        print(test.chosenModel);
        ClientScene.AddPlayer(conn, 0, test);

        //base.OnClientConnect(conn);
    }

    public override void OnStopClient()
    {
        ConnectionManager.Instance.Hide();

        base.OnStopClient();
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        NetworkMessage message = extraMessageReader.ReadMessage<NetworkMessage>();
        int chosenModel = message.chosenModel;
        GameObject player = null;

        if (chosenModel == 0)
        {
            player = Instantiate(Resources.Load("PlayerModels/Player Model 01", typeof(GameObject))) as GameObject;
        }
        else if (chosenModel == 1)
        {
            player = Instantiate(Resources.Load("PlayerModels/Player Model 02", typeof(GameObject))) as GameObject;
        }

        player.transform.position = GetStartPosition().position;
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
}
