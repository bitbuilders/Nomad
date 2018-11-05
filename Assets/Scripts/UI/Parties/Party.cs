using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Party : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_partyName = null;
    [SerializeField] TextMeshProUGUI m_partyIconName = null;
    [SerializeField] GameObject m_partyPlayer = null;
    [SerializeField] Transform m_partyMemberLocations = null;
    [SerializeField] TMP_InputField m_inviteName = null;

    List<string> m_members;
    string m_leader;

    public void ChangeParty(string leader)
    {
        m_leader = leader;
        m_members = new List<string>();

        UpdateUI();
    }

    public void LeaveParty()
    {
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        string localName = localPlayer.UserName;

        m_members.Remove(localName);

        if (m_leader == localName)
        {
            if (m_members.Count > 0)
            {
                m_leader = m_members[0];
                SendLeaderNotification(m_leader);
            }
            else
                m_leader = "";
        }

        ClearCurrentParty();

        PartyManager.Instance.DisableParty();
        
        PartyMessenger pm = localPlayer.GetComponent<PartyMessenger>();
        m_members.Remove(localName);
        foreach (string player in m_members)
        {
            pm.RemovePlayerFromParty(player, localName);
        }

        pm.RemovePlayerFromParty(m_leader, localName);
    }

    void SendLeaderNotification(string newLeader)
    {
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        PartyMessenger pm = localPlayer.GetComponent<PartyMessenger>();
        foreach (string player in m_members)
        {
            pm.NewLeader(player, m_leader);
        }
    }

    public void RemoveFromParty(string player)
    {
        m_members.Remove(player);

        PartyPlayer[] children = m_partyMemberLocations.GetComponentsInChildren<PartyPlayer>();
        foreach (PartyPlayer child in children)
        {
            if (child.PlayerName == player)
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }

    public void JoinParty()
    {
        ClearCurrentParty();

        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        string localName = localPlayer.UserName;
        PartyMessenger pm = localPlayer.GetComponent<PartyMessenger>();
        // Adds themself to party
        AddToParty(localName);

        // Adds new player to all current members' party, and adds all current members to new player's plarty
        foreach (string name in m_members)
        {
            if (name != localName)
            {
                pm.AddPlayerToParty(name, localName);
                pm.AddPlayerToParty(localName, name);
            }
        }

        // Adds leader to their party, and vice versa
        if (m_leader != localName)
        {
            pm.AddPlayerToParty(m_leader, localName);
            pm.AddPlayerToParty(localName, m_leader);
        }
    }

    public void AddToParty(string player)
    {
        GameObject go = Instantiate(m_partyPlayer, Vector3.zero, Quaternion.identity, m_partyMemberLocations);
        go.name = player;
        PartyPlayer pp = go.GetComponent<PartyPlayer>();
        pp.Initialize(player);

        m_members.Add(player);
    }

    public void InviteToParty(TMP_InputField name)
    {
        string player = name.text.Trim();
        LocalPlayerData localData = LocalPlayerData.Instance;

        bool exists = localData.PlayerExists(player);
        bool hasSameNameAsLocal = localData.LocalPlayer.UserName == player;
        bool multipleWithLocalName = localData.MultipleUsernames();
        bool invalidName = (hasSameNameAsLocal && !multipleWithLocalName);
        if (exists && !invalidName)
        {
            localData.LocalPlayer.GetComponent<PartyMessenger>().SendPlayerInvites(m_leader, player);
        }
    }

    public void ShowInvitationDialog()
    {
        m_inviteName.text = "";
        m_inviteName.ActivateInputField();
    }

    public void Hide()
    {
        PartyManager.Instance.HidePartyWindow();
    }

    public void SetAsLeader(string leader)
    {
        m_leader = leader;
        m_members.Remove(m_leader);
    }

    void UpdateUI()
    {
        m_partyName.text = m_leader + "'s";
        m_partyIconName.text = m_leader + "'s";
    }

    void ClearCurrentParty()
    {
        m_members.Clear();

        Transform[] children = m_partyMemberLocations.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child != m_partyMemberLocations)
                Destroy(child.gameObject);
        }
    }
}
