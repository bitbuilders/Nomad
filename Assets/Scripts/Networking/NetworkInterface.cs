using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkInterface : Singleton<NetworkInterface>
{
    NetworkManager m_networkManager;
    public NetworkClient myNetworkClient { get; private set; }

    private void Start()
    {
        m_networkManager = GetComponent<NetworkManager>();
    }

    public void Server()
    {
        m_networkManager.StartServer();
    }

    public void Host()
    {
        myNetworkClient = m_networkManager.StartHost();
        LoadScene("Lobby");
    }

    public void Client()
    {
        myNetworkClient = m_networkManager.StartClient();
        LoadScene("Lobby");
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
