using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.Networking;

public class ChatRoom : MonoBehaviour
{
    [SerializeField] RectTransform m_containerBounds = null;
    [SerializeField] RectTransform m_roomViewTransform = null;
    [SerializeField] TextMeshProUGUI m_chatLog = null;
    [SerializeField] TextMeshProUGUI m_roomNameText = null;
    [SerializeField] GameObject m_nameChange = null;
    [SerializeField] TextMeshProUGUI m_namePlaceholder = null;
    [SerializeField] TMP_InputField m_nameInputField = null;
    [SerializeField] GameObject m_buttons = null;
    [SerializeField] GameObject m_chatRoom = null;
    [SerializeField] GameObject m_inputField = null;
    [SerializeField] GameObject m_editButton = null;
    [SerializeField] GameObject m_roomButton = null;
    [SerializeField] GameObject m_inviteDialog = null;
    [SerializeField] GameObject m_deleteDialog = null;
    [SerializeField] TMP_InputField m_field = null;

    public int ID { get; private set; }
    public string Name { get; private set; }
    public bool Hidden { get; private set; }

    Animator m_animator;
    NotificationImageAlert m_notificationAlert;
    StringBuilder m_text;
    Vector3 m_startSize;
    float m_time;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_notificationAlert = GetComponent<NotificationImageAlert>();
        m_text = new StringBuilder(m_chatLog.text);
        string welcomeMessage = Colors.ConvertToColor("Welcome to the chat room!", Colors.ColorType.WHITE);
        AddMessage(welcomeMessage);
        if (m_nameChange)
            m_nameChange.GetComponent<Animator>().SetTrigger("Expand");

        if (m_notificationAlert)
        {
            m_notificationAlert.Initialize();
        }
        Hidden = true;
    }

    private void Update()
    {
        m_time += Time.deltaTime;
        if (Input.GetButtonDown("Cancel"))
        {
            HideAllDialog();
        }
    }

    public void ResetMessages()
    {
        m_text = new StringBuilder();
        string welcomeMessage = Colors.ConvertToColor("Welcome to the chat room!", Colors.ColorType.WHITE);
        AddMessage(welcomeMessage);
    }

    public void Initialize(int roomID, string name = "Chat Room")
    {
        Name = name;
        m_buttons.gameObject.SetActive(false);
        m_chatRoom.gameObject.SetActive(false);
        m_inputField.gameObject.SetActive(false);
        ChangeRoomName();
        UpdateRoomName();

        if (roomID < 0)
            ID = 0;
        else
            ID = roomID;
    }

    public void ActivateInputField()
    {
        if (m_field)
            m_field.ActivateInputField();
    }

    public void SetRoomName(string roomName)
    {
        Name = roomName;
    }

    public void SetRoomName(TMP_InputField roomName)
    {
        string name = roomName.text;
        if (!string.IsNullOrEmpty(name.Trim()))
        {
            Name = name;
        }
    }

    public void ChangeRoomName()
    {
        ShowDialog(m_nameChange);
        m_nameInputField.ActivateInputField();
    }

    public void UpdateRoomName()
    {
        m_roomNameText.text = Name;
        m_namePlaceholder.text = Name;
        m_nameInputField.text = "";
    }

    public void ShowDialog(GameObject dialog)
    {
        Animator animator = dialog.GetComponent<Animator>();
        //if (!animator.runtimeAnimatorController)
        //    return;
        dialog.SetActive(true);
        animator.SetTrigger("Expand");
    }

    public void HideDialog(GameObject dialog)
    {
        Animator animator = dialog.GetComponent<Animator>();
        if (!animator.runtimeAnimatorController)
            return;
        animator.SetTrigger("Shrink");
    }

    public void HideAllDialog()
    {
        if (m_nameChange && m_nameChange.activeInHierarchy)
        {
            m_nameChange.GetComponent<Animator>().SetTrigger("Shrink");
        }
        if (m_inviteDialog && m_inviteDialog.activeInHierarchy)
        {
            m_inviteDialog.GetComponent<Animator>().SetTrigger("Shrink");
        }
        if (m_deleteDialog && m_deleteDialog.activeInHierarchy)
        {
            m_deleteDialog.GetComponent<Animator>().SetTrigger("Shrink");
        }
    }

    public void HideRoom()
    {
        if (m_time >= 0.2f)
        {
            Hide();
        }
    }

    public void Hide()
    {
        HideAllDialog();

        if (!m_chatRoom.activeInHierarchy)
            return;

        m_roomButton.SetActive(true);
        m_inputField.SetActive(false);
        m_chatRoom.SetActive(false);
        m_buttons.SetActive(false);
        m_editButton.SetActive(false);
        ChatRoomManager.Instance.HideChatRoom();
        m_animator.SetTrigger("ExpandUp");
        m_time = 0.0f;
        Hidden = true;
    }

    public void OpenRoom()
    {
        if (m_chatRoom.activeInHierarchy)
            return;

        m_animator.SetTrigger("Create");
        if (m_time >= 0.2f)
        {
            if (m_nameChange.activeInHierarchy)
                HideDialog(m_nameChange);
            m_roomButton.SetActive(true);
            m_inputField.SetActive(true);
            m_chatRoom.SetActive(true);
            m_buttons.SetActive(true);
            m_editButton.SetActive(true);
            ChatRoomManager.Instance.OpenChatRoom();
            m_animator.SetTrigger("ExpandDown");
            m_time = 0.0f;
            m_notificationAlert.RemoveNotifications();
            Hidden = false;
        }
    }

    public void Create()
    {
        GetComponent<Animator>().SetTrigger("Create");
        HideDialog(m_nameChange);
    }

    public void AddMessage(string text)
    {
        if (m_text.Length > 0)
        {
            m_text.Append("\n");
        }

        m_text.Append(text);
        m_chatLog.text = m_text.ToString();

        Vector2 bounds = m_roomViewTransform.sizeDelta;
        float padding = m_chatLog.margin.x * 2.0f;
        Vector2 size = m_chatLog.GetPreferredValues(m_chatLog.text, bounds.x - padding, bounds.y - padding);
        m_containerBounds.sizeDelta = new Vector2(m_containerBounds.sizeDelta.x, size.y);

        if (Hidden && m_notificationAlert)
            m_notificationAlert.AddNewNotification();
    }

    public void DestroyChatRoom()
    {
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        string joinMessage = Colors.ConvertToColor(localPlayer.UserName + " left the room", Colors.ColorType.WHITE);
        localPlayer.GetComponent<ChatRoomMessenger>().SendMessageToRoom(joinMessage, ID);
        ChatRoomManager.Instance.RemoveChatRoom(gameObject);
        ChatRoomManager.Instance.HideChatRoom();
    }
}
