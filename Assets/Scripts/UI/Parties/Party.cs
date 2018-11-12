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
    [SerializeField] ChatRoom m_chatRoom = null;
    [SerializeField] GameObject m_chatRoomAndField = null;

    Animator m_animator;
    List<string> m_members;
    string m_leader;
    float m_lastTime;
    bool m_chatOpen;

    private void Start()
    {
        Initialize();
        gameObject.SetActive(false);
    }

    public void Initialize()
    {
        m_animator = GetComponent<Animator>();
    }

    public void ChangeParty(string leader)
    {
        m_leader = leader;
        m_members = new List<string>();

        UpdateUI();
        m_chatOpen = false;
        m_animator.SetTrigger("ExpandDownSmall");
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
        
        PartyManager.Instance.DisableParty();
        
        PartyMessenger pm = localPlayer.GetComponent<PartyMessenger>();
        foreach (string player in m_members)
        {
            pm.RemovePlayerFromParty(player, localName);
        }

        ClearCurrentParty();
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

        // Adds leader to their party, and vice versa
        if (m_leader != localName)
        {
            pm.AddPlayerToParty(m_leader, localName);
            AddToParty(m_leader);
        }
    }

    public void SendJoinNotification(string player)
    {
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        PartyMessenger pm = localPlayer.GetComponent<PartyMessenger>();
        foreach (string name in m_members)
        {
            if (name != player && name != m_leader)
            {
                pm.AddPlayerToParty(name, player);
                pm.AddPlayerToParty(player, name);
            }
        }
    }

    public void AddToParty(string player)
    {
        GameObject go = Instantiate(m_partyPlayer, Vector3.zero, Quaternion.identity, m_partyMemberLocations);
        go.name = player;
        PartyPlayer pp = go.GetComponent<PartyPlayer>();
        pp.Initialize(player);

        m_members.Add(player);

        if (m_leader == LocalPlayerData.Instance.LocalPlayer.UserName && m_leader != player)
        {
            SendJoinNotification(player);
        }
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
        else
        {
            // ERROR
        }
    }

    public void ShowInvitationDialog()
    {
        m_inviteName.text = "";
        m_inviteName.ActivateInputField();
    }

    public void Hide()
    {
        float time = Time.time - m_lastTime;
        if (time >= 0.51f)
        {
            PartyManager.Instance.HidePartyWindow();
            m_animator.SetTrigger("ExpandUpSmall");
            HideChatRoom();
            m_chatOpen = false;
            m_lastTime = Time.time;
        }
    }

    public void Show(GameObject caller = null)
    {
        float time = Time.time - m_lastTime;
        if (time >= 0.51f)
        {
            gameObject.SetActive(true);
            if (caller)
                caller.SetActive(false);
            m_animator.SetTrigger("ExpandDownSmall");
            m_lastTime = Time.time;
        }
    }

    public void HideForReal()
    {
        gameObject.SetActive(false);
    }

    public void SetAsLeader(string leader)
    {
        m_leader = leader;
        UpdateUI();
    }

    public void SendChatMessage(string message)
    {
        foreach (string player in m_members)
        {
            LocalPlayerData.Instance.LocalPlayer.GetComponent<PartyMessenger>().SendChatMessage(player, message);
        }
    }

    public void ReceiveMessage(string message)
    {
        m_chatRoom.AddMessage(message);
    }

    public void ShowChatRoom()
    {
        PlayerMovement playerMove = LocalPlayerData.Instance.LocalPlayer.GetComponent<PlayerMovement>();
        playerMove.AddState(PlayerMovement.PlayerState.PARTY_MESSAGE);
        m_chatRoomAndField.GetComponent<PartyChatRoom>().FlipLeft();
    }

    public void HideChatRoom()
    {
        PlayerMovement playerMove = LocalPlayerData.Instance.LocalPlayer.GetComponent<PlayerMovement>();
        playerMove.RemoveState(PlayerMovement.PlayerState.PARTY_MESSAGE);
        m_chatRoomAndField.GetComponent<PartyChatRoom>().FlipRight();
    }

    public void ToggleChatRoom()
    {
        if (m_chatOpen)
        {
            HideChatRoom();
            m_chatOpen = false;
        }
        else
        {
            ShowChatRoom();
            m_chatOpen = true;
        }
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
            {
                Destroy(child.gameObject);
            }
        }
    }
}
