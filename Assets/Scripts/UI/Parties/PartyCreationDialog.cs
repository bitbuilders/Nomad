using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class PartyCreationDialog : MonoBehaviour
{
    [SerializeField] TMP_InputField m_p1Name = null;
    [SerializeField] TMP_InputField m_p2Name = null;
    [SerializeField] TMP_InputField m_p3Name = null;
    [SerializeField] TMP_InputField m_p4Name = null;

    public void CreateParty()
    {
        List<string> invited = new List<string>() { m_p1Name.text, m_p2Name.text, m_p3Name.text, m_p4Name.text };
        LocalPlayerData localData = LocalPlayerData.Instance;
        string leader = localData.LocalPlayer.UserName;

        for (int i = 0; i < invited.Count; i++)
        {
            if (invited.Count == 0)
                break;

            string player = invited[i].Trim();
            bool hasNameAsLeader = (leader == player);
            bool multipleLeaderUsername = (localData.MultipleUsernames());
            bool invalidName = (hasNameAsLeader && !multipleLeaderUsername);
            if (string.IsNullOrEmpty(player) || !localData.PlayerExists(player) || invalidName)
            {
                invited.RemoveAt(i);
                i--;
            }
        }

        if (invited.Count > 0)
        {
            PartyManager.Instance.InvitePlayersToParty(leader, invited);
            PartyManager.Instance.ShowPartyWindow();
            PartyManager.Instance.SetupParty(leader);
        }
    }

    public void ShowDialog()
    {
        PartyManager.Instance.SetupPartyCreation();
        PlayerMovement playerMove = LocalPlayerData.Instance.LocalPlayer.GetComponent<PlayerMovement>();
        playerMove.AddState(PlayerMovement.PlayerState.PARTY_MESSAGE);
    }

    public void Cancel()
    {
        m_p1Name.text = "";
        m_p2Name.text = "";
        m_p3Name.text = "";
        m_p4Name.text = "";

        PartyManager.Instance.DisableParty();
        PlayerMovement playerMove = LocalPlayerData.Instance.LocalPlayer.GetComponent<PlayerMovement>();
        playerMove.RemoveState(PlayerMovement.PlayerState.PARTY_MESSAGE);
    }
}
