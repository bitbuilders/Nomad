using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : Singleton<PartyManager>
{
    [SerializeField] Party m_localParty = null;
    
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
}
