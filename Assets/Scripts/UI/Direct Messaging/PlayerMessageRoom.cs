using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerMessageRoom : MonoBehaviour
{
    public string Messages { get; set; }
    public string StartMessage { get; set; }
    public string Name { get; set; }

    DirectMessageInterface m_messageInterface;
    DirectMessageNotification m_dmNotification;

    private void Start()
    {
        Initialize();
    }

    // Will sometimes need to call before start
    public void Initialize()
    {
        string startMessage = string.IsNullOrEmpty(StartMessage) ? "" : "\n" + StartMessage;
        Messages = Colors.ConvertToColor("This is a private room with " + Name, Colors.ColorType.WHITE) + startMessage;
        m_messageInterface = GameObject.Find("Menu").GetComponent<DirectMessageInterface>();
        m_dmNotification = GetComponent<DirectMessageNotification>();
    }

    public void SetAsCurrentRoom()
    {
        DirectMessageManager.Instance.SetCurrentMessageRoom(this);
        m_messageInterface.InitializeInputField();
        m_dmNotification.RemoveNotifications();
    }

    public void AddMessage(string message)
    {
        StringBuilder sb = new StringBuilder(Messages);
        sb.Append("\n");
        sb.Append(message);

        Messages = sb.ToString();

        if (DirectMessageManager.Instance.CurrentRoom != this)
        {
            m_dmNotification.AddNewNotification();
        }
    }
}
