using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DirectMessageManager : Singleton<DirectMessageManager>
{
    [SerializeField] GameObject m_messageRoom = null;

    List<PlayerMessageRoom> m_messageRooms;
    Transform m_roomLocation;

    private void Start()
    {
        m_roomLocation = GameObject.Find("Player Rooms").GetComponent<Transform>();
        m_messageRooms = new List<PlayerMessageRoom>();
    }

    public void CreateMessageRoom(string playerName)
    {
        if (!IsLocalPlayer(playerName))
            return;
        GameObject go = Instantiate(m_messageRoom, Vector3.zero, Quaternion.identity, m_roomLocation);
        TextMeshProUGUI pName = go.GetComponentInChildren<TextMeshProUGUI>();
        pName.text = playerName;
        m_messageRooms.Add(go.GetComponent<PlayerMessageRoom>());
    }

    bool IsLocalPlayer(string playerName)
    {
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        return localPlayer.UserName == playerName;
    }
}
