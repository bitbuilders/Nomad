using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PartyCreationDialog : MonoBehaviour
{
    [SerializeField] TMP_InputField m_p1Name = null;
    [SerializeField] TMP_InputField m_p2Name = null;
    [SerializeField] TMP_InputField m_p3Name = null;
    [SerializeField] TMP_InputField m_p4Name = null;

    public void CreateParty()
    {
        List<string> invited = new List<string>() { m_p1Name.text, m_p2Name.text, m_p3Name.text, m_p4Name.text };
        string leader = LocalPlayerData.Instance.LocalPlayer.UserName;

        for (int i = 0; i < invited.Count; i++)
        {
            if (invited.Count == 0)
                break;

            if (string.IsNullOrEmpty(invited[i].Trim()))
            {
                invited.RemoveAt(i);
                i--;
            }
        }

        if (invited.Count > 0)
        {
            PartyManager.Instance.InvitePlayersToParty(leader, invited);
            PartyManager.Instance.ShowPartyWindow();
        }
    }

    public void ShowDialog()
    {
        PartyManager.Instance.SetupPartyCreation();
    }

    public void Cancel()
    {
        m_p1Name.text = "";
        m_p2Name.text = "";
        m_p3Name.text = "";
        m_p4Name.text = "";

        PartyManager.Instance.DisableParty();
    }
}
