using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] TMP_InputField m_username = null;
    [SerializeField] TMP_InputField m_ipAddress = null;
    [SerializeField] LANToggle m_lanToggle = null;
    [SerializeField] PlayerColorPicker m_colorPicker = null;

    private void Start()
    {
        m_ipAddress.text = NomadNetworkManager.Instance.networkAddress;
    }

    public void StartServer()
    {
        NomadNetworkManager.Instance.StartServer();
    }

    public void StartClient()
    {
        SetPlayerData();

        if (m_lanToggle.LAN)
        {
            NomadNetworkManager.Instance.networkAddress = m_ipAddress.text.Trim();
        }
        else
        {

        }

        NomadNetworkManager.Instance.StartClient();
    }

    public void StartHost()
    {
        SetPlayerData();
        NomadNetworkManager.Instance.StartHost();
    }

    void SetPlayerData()
    {
        string username = m_username.text.Trim();
        if (!string.IsNullOrEmpty(username))
            LocalPlayerData.Instance.TempUsername = username;
        else
            LocalPlayerData.Instance.TempUsername = "Lost Nomad";

        LocalPlayerData.Instance.TempColor = m_colorPicker.Color;
    }
}
