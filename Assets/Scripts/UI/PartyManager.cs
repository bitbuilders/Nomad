using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : Singleton<PartyManager>
{
    [SerializeField] Party m_localParty = null;
    [SerializeField] GameObject m_partyButton = null;
    [SerializeField] GameObject m_partyCreate = null;
    [SerializeField] GameObject m_partyWindow = null;
    
    public void InvitePlayersToParty(string leader, List<string> invited)
    {
        foreach (string player in invited)
        {
            LocalPlayerData.Instance.LocalPlayer.GetComponent<PartyMessenger>().SendPlayerInvites(leader, player);
        }
    }

    public void AddPlayerToParty(string player)
    {
        LocalPlayerData.Instance.LocalPlayer.GetComponent<PartyMessenger>().AddPlayerToParty(player);
    }

    public void SetupParty(string leader)
    {
        m_localParty.ChangeParty(leader);
    }

    public void SetupPartyCreation()
    {
        m_partyCreate.SetActive(true);
        m_partyButton.SetActive(false);
        m_partyWindow.SetActive(false);
    }

    public void ShowPartyWindow()
    {
        m_partyCreate.SetActive(false);
        m_partyButton.SetActive(false);
        m_partyWindow.SetActive(true);
    }

    public void HidePartyWindow()
    {

    }

    public void DisableParty()
    {
        m_partyCreate.SetActive(false);
        m_partyButton.SetActive(true);
        m_partyWindow.SetActive(false);
    }
}
