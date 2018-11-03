using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Party : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_partyName = null;
    [SerializeField] GameObject m_partyPlayer = null;
    [SerializeField] Transform m_partyMemberLocations = null;

    List<string> m_members;
    string m_leader;

    public void ChangeParty(string leader)
    {
        m_leader = leader;
        m_members = new List<string>();

        UpdateUI();
    }

    public void LeaveParty(string player)
    {
        if (!m_members.Contains(player))
            return;

        m_members.Remove(player);

        if (m_leader == player)
        {
            if (m_members.Count > 0)
                m_leader = m_members[0];
            else
                m_leader = "";
        }
    }

    public void JoinParty(string player)
    {
        ClearCurrentParty();

        PartyMessenger pm = LocalPlayerData.Instance.LocalPlayer.GetComponent<PartyMessenger>();
        // Adds themself to party
        AddToParty(player);

        // Adds new player to all current members' party, and adds all current members to new player's plarty
        foreach (string name in m_members)
        {
            if (name != player)
            {
                pm.AddPlayerToParty(name, player);
                pm.AddPlayerToParty(player, name);
            }
        }

        // Adds leader to their party, and vice versa
        if (m_leader != player)
        {
            pm.AddPlayerToParty(m_leader, player);
            pm.AddPlayerToParty(player, m_leader);
        }
    }

    public void AddToParty(string player)
    {
        GameObject go = Instantiate(m_partyPlayer, Vector3.zero, Quaternion.identity, m_partyMemberLocations);
        PartyPlayer pp = go.GetComponent<PartyPlayer>();
        pp.Initialize(player);

        m_members.Add(player);
    }

    void UpdateUI()
    {
        m_partyName.text = m_leader + "'s";
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
