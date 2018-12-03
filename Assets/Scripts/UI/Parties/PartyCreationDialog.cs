using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PartyCreationDialog : MonoBehaviour
{
    [SerializeField] TMP_InputField m_p1Name = null;
    [SerializeField] TMP_InputField m_p2Name = null;
    [SerializeField] TMP_InputField m_p3Name = null;
    [SerializeField] TMP_InputField m_p4Name = null;

    Animator m_animator;
    UIButton m_uiButton;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_uiButton = GetComponent<UIButton>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            CreateParty();
        }
        if (Input.GetButtonDown("Cancel"))
        {
            Cancel();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (m_p1Name.isFocused)
                m_p2Name.ActivateInputField();
            else if (m_p2Name.isFocused)
                m_p3Name.ActivateInputField();
            else if (m_p3Name.isFocused)
                m_p4Name.ActivateInputField();
            else if (m_p4Name.isFocused)
                m_p1Name.ActivateInputField();
            else
                m_p1Name.ActivateInputField();
        }
    }

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
            PartyManager.Instance.SetupParty(leader);

            PlayerMovement playerMove = LocalPlayerData.Instance.LocalPlayer.GetComponent<PlayerMovement>();
            playerMove.RemoveState(PlayerMovement.PlayerState.PARTY_MESSAGE);
            m_uiButton.Click();
        }
        Cancel();
    }

    public void ShowDialog()
    {
        PartyManager.Instance.SetupPartyCreation();
        PlayerMovement playerMove = LocalPlayerData.Instance.LocalPlayer.GetComponent<PlayerMovement>();
        playerMove.AddState(PlayerMovement.PlayerState.PARTY_MESSAGE);
        m_animator.SetTrigger("Expand");

    }

    public void Cancel()
    {
        m_p1Name.text = "";
        m_p2Name.text = "";
        m_p3Name.text = "";
        m_p4Name.text = "";

        PlayerMovement playerMove = LocalPlayerData.Instance.LocalPlayer.GetComponent<PlayerMovement>();
        playerMove.RemoveState(PlayerMovement.PlayerState.PARTY_MESSAGE);
        m_animator.SetTrigger("Shrink");
    }

    public void Hide()
    {
        PartyManager.Instance.ShowPartyButton();
        gameObject.SetActive(false);
    }
}
