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
        GameObject go = Instantiate(m_partyPlayer, Vector3.zero, Quaternion.identity, m_partyMemberLocations);
        PartyPlayer pp = go.GetComponent<PartyPlayer>();
        pp.Initialize(player);

        m_members.Add(player);
    }

    void UpdateUI()
    {
        m_partyName.text = m_leader + "'s";
    }
}
