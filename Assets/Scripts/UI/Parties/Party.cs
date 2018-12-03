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
    [SerializeField] GameObject m_inviteDialog = null;
    [SerializeField] GameObject m_leaveDialog = null;

    public bool Hidden { get; private set; }
    public bool InParty { get { return m_members.Count > 0; } }

    Animator m_animator;
    NotificationImageAlert m_notificationAlert;
    List<string> m_members;
    string m_leader;
    float m_lastTime;
    float m_lastChatClick;
    bool m_chatOpen;

    private void Start()
    {
        Initialize();
        Hidden = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (m_inviteDialog.activeInHierarchy)
                HideDialog(m_inviteDialog);
            if (m_leaveDialog.activeInHierarchy)
                HideDialog(m_leaveDialog);
        }
    }

    public void Initialize()
    {
        m_members = new List<string>();
        m_animator = GetComponent<Animator>();
        m_lastChatClick = 0.0f;
        m_lastTime = 0.0f;
        m_notificationAlert = GetComponent<NotificationImageAlert>();
        m_notificationAlert.Initialize();
        Hidden = false;
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
        SendChatMessage(Colors.ConvertToColor(localName + " left the party", Colors.ColorType.WHITE));

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

        Hide(true);
        PartyManager.Instance.ShowPartyButton();
        m_chatRoom.ResetMessages();
        
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
        
        SendChatMessage(Colors.ConvertToColor(localName + " joined the party!", Colors.ColorType.WHITE));
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

    public void Hide(bool permanently = false)
    {
        float time = Time.time - m_lastTime;
        if (time >= 0.51f && !Hidden)
        {
            HideAllDialog();
            if (!permanently)
                PartyManager.Instance.HidePartyWindow();
            if (m_chatOpen)
                StartCoroutine(MinimizeWithChat());
            else
                m_animator.SetTrigger("ExpandUpSmall");
            m_chatOpen = false;
            m_lastTime = Time.time;
            Hidden = true;
        }
    }

    public void HideAllDialog()
    {
        if (m_inviteDialog.activeInHierarchy)
            HideDialog(m_inviteDialog);
        if (m_leaveDialog.activeInHierarchy)
            HideDialog(m_leaveDialog);
    }

    IEnumerator MinimizeWithChat()
    {
        HideChatRoom();
        yield return new WaitForSeconds(0.4f);
        m_animator.SetTrigger("ExpandUpSmall");
    }

    public void Show(GameObject caller = null)
    {
        float time = Time.time - m_lastTime;
        if (time >= 0.51f && Hidden)
        {
            gameObject.SetActive(true);
            if (caller)
                caller.SetActive(false);
            m_animator.SetTrigger("ExpandDownSmall");
            m_lastTime = Time.time;
            Hidden = false;
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
        if (InParty)
            m_chatRoom.AddMessage(message);

        if (!m_chatOpen)
        {
            m_notificationAlert.AddNewNotification();
        }
    }

    public void ShowChatRoom()
    {
        float delta = Time.time - m_lastChatClick;
        if (delta >= 0.5f)
        {
            PlayerMovement playerMove = LocalPlayerData.Instance.LocalPlayer.GetComponent<PlayerMovement>();
            playerMove.AddState(PlayerMovement.PlayerState.PARTY_MESSAGE);
            m_chatRoomAndField.GetComponent<PartyChatRoom>().FlipLeft();
            m_lastChatClick = Time.time;
            m_chatOpen = true;
            m_notificationAlert.RemoveNotifications();
            m_chatRoom.ActivateInputField();
        }
    }

    public void HideChatRoom()
    {
        float delta = Time.time - m_lastChatClick;
        if (delta >= 0.5f)
        {
            PlayerMovement playerMove = LocalPlayerData.Instance.LocalPlayer.GetComponent<PlayerMovement>();
            playerMove.RemoveState(PlayerMovement.PlayerState.PARTY_MESSAGE);
            m_chatRoomAndField.GetComponent<PartyChatRoom>().FlipRight();
            m_lastChatClick = Time.time;
            m_chatOpen = false;
        }
    }

    public void ToggleChatRoom()
    {
        if (m_chatOpen)
        {
            HideChatRoom();
        }
        else
        {
            ShowChatRoom();
        }
    }

    public void ShowInviteDialog()
    {
        if (m_leaveDialog.activeInHierarchy)
            Minimize(m_leaveDialog);
        Maximize(m_inviteDialog);

        m_inviteName.text = "";
        m_inviteName.ActivateInputField();
    }

    public void ShowLeaveDialog()
    {
        if (m_inviteDialog.activeInHierarchy)
            Minimize(m_inviteDialog);
        Maximize(m_leaveDialog);
    }

    public void HideDialog(GameObject dialog)
    {
        Minimize(dialog);
    }

    void Minimize(GameObject dialog)
    {
        dialog.GetComponent<Animator>().SetTrigger("Shrink");
    }

    void Maximize(GameObject dialog)
    {
        dialog.GetComponent<Animator>().SetTrigger("Expand");
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
