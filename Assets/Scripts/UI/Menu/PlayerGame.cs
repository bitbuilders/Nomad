using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerGame : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_hostNameText = null;

    public string IP { get; set; }
    public string HostName { get; set; }

    public void Initialize(string ip, string hostName)
    {
        IP = ip;
        HostName = hostName;

        UpdateText();
    }

    public void Join()
    {
        Menu.Instance.StartClient(IP);
    }

    void UpdateText()
    {
        m_hostNameText.text = HostName;
    }
}
