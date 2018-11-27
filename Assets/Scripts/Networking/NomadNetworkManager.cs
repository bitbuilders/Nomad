using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NomadNetworkManager : NetworkManager
{
    public static NomadNetworkManager Instance;

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

        base.OnClientConnect(conn);
    }

    public override void OnStopClient()
    {
        ConnectionManager.Instance.Hide();

        base.OnStopClient();
    }
}
