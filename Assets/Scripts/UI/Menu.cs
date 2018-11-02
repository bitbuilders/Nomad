using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] NetworkManager m_networkManager = null;
    [SerializeField] TMP_InputField m_ipAddress = null;

    public void StartServer()
    {
        m_networkManager.StartServer();
    }

    public void StartClient()
    {
        m_networkManager.networkAddress = m_ipAddress.text.Trim();
        m_networkManager.StartClient();
    }

    public void StartHost()
    {
        m_networkManager.StartHost();
    }
}
