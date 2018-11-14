using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DirectMessageInterface : MonoBehaviour
{
    [SerializeField] TMP_InputField m_conversationNameInput = null;
    [SerializeField] TMP_InputField m_messageContentInput = null;
    [SerializeField] TextMeshProUGUI m_notificationText = null;
    [SerializeField] GameObject m_conversationDialog = null;

    Animator m_animator;
    ConversationCreationDialog m_creationDialog;
    bool m_hidden;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_creationDialog = m_conversationDialog.GetComponent<ConversationCreationDialog>();
        m_hidden = true;
    }

    public void CreateNewConversation()
    {
        string playerName = m_conversationNameInput.text;
        bool targetHasLocalName = (playerName == LocalPlayerData.Instance.LocalPlayer.UserName);
        bool multipleWithLocalName = LocalPlayerData.Instance.MultipleUsernames();
        if (LocalPlayerData.Instance.PlayerExists(playerName))
        {
            if (targetHasLocalName && !multipleWithLocalName)
            {
                m_notificationText.gameObject.SetActive(true);
            }
            else
            {
                PlayerMessageRoom room = DirectMessageManager.Instance.CreateMessageRoom(playerName);
                room.Initialize();
                room.SetAsCurrentRoom();
            }
        }
        else
        {
            m_notificationText.gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        if (!m_hidden)
        {
            m_hidden = true;
            m_animator.SetTrigger("SlideOut");
            PlayerMovement playerMove = LocalPlayerData.Instance.LocalPlayer.GetComponent<PlayerMovement>();
            playerMove.RemoveState(PlayerMovement.PlayerState.DIRECT_MESSAGE);
        }
    }

    public void Expand()
    {
        if (m_hidden)
        {
            m_hidden = false;
            m_animator.SetTrigger("SlideIn");
            PlayerMovement playerMove = LocalPlayerData.Instance.LocalPlayer.GetComponent<PlayerMovement>();
            playerMove.AddState(PlayerMovement.PlayerState.DIRECT_MESSAGE);
        }
    }

    public void InitializePlayerNameField()
    {
        m_conversationDialog.SetActive(true);
        m_creationDialog.Initialize();
        m_conversationNameInput.text = "";
        m_conversationNameInput.ActivateInputField();
        m_creationDialog.Expand();
    }

    public void SendDirectMessageMessage()
    {
        PlayerMessageRoom room = DirectMessageManager.Instance.CurrentRoom;
        if (!room)
        {
            m_messageContentInput.text = "";
            InitializeInputField();
            return;
        }
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        string message = localPlayer.UserName + ": " + m_messageContentInput.text;
        LocalPlayerData.Instance.LocalPlayer.GetComponent<DirectMessenger>().SendDirectMessage(message, room.Name);
        m_messageContentInput.text = "";
        InitializeInputField();
    }

    public void InitializeInputField()
    {
        m_messageContentInput.ActivateInputField();
    }

    public void OnMessageContentChange()
    {
        if (m_messageContentInput.text.Contains("\n") && !string.IsNullOrEmpty(m_messageContentInput.text.Trim()))
        {
            m_messageContentInput.text = m_messageContentInput.text.Replace("\n", "").Trim();
            SendDirectMessageMessage();
        }
        else if (string.IsNullOrEmpty(m_messageContentInput.text.Trim()))
        {
            m_messageContentInput.text = "";
        }
    }
}
