using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : Singleton<PartyManager>
{
    [SerializeField] Party m_localParty = null;
    [SerializeField] GameObject m_partyButton = null;
    [SerializeField] GameObject m_partyCreate = null;
    [SerializeField] GameObject m_partyWindow = null;
    [SerializeField] GameObject m_inviteButton = null;
    [SerializeField] GameObject m_partyIcon = null;
    
    public void InvitePlayersToParty(string leader, List<string> invited)
    {
        foreach (string player in invited)
        {
            LocalPlayerData.Instance.LocalPlayer.GetComponent<PartyMessenger>().SendPlayerInvites(leader, player);
        }
    }

    public void AddPlayerToParty(string player)
    {
        m_localParty.AddToParty(player);
    }

    public void SetupParty(string leader)
    {
        m_localParty.ChangeParty(leader);
        ShowPartyWindow();
        m_localParty.JoinParty(LocalPlayerData.Instance.LocalPlayer.UserName);

        if (leader == LocalPlayerData.Instance.LocalPlayer.UserName)
            m_inviteButton.SetActive(true);
        else
            m_inviteButton.SetActive(false);
    }

    public void SetupPartyCreation()
    {
        m_partyCreate.SetActive(true);
        m_partyButton.SetActive(false);
        m_partyWindow.SetActive(false);
        m_partyIcon.SetActive(false);
    }

    public void ShowPartyWindow()
    {
        m_partyCreate.SetActive(false);
        m_partyButton.SetActive(false);
        m_partyWindow.SetActive(true);
        m_partyIcon.SetActive(false);
    }

    public void HidePartyWindow()
    {
        m_partyIcon.SetActive(true);
    }

    public void DisableParty()
    {
        m_partyCreate.SetActive(false);
        m_partyButton.SetActive(true);
        m_partyWindow.SetActive(false);
        m_partyIcon.SetActive(false);
    }
}
