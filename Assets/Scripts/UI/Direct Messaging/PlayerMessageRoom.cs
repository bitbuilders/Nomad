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
    public NotificationImageAlert NotificationAlert { get; private set; }
    NotificationImageAlert m_parentNotification;

    // Will sometimes need to call before start
    public void Initialize(NotificationImageAlert parentNotification)
    {
        m_parentNotification = parentNotification;
        NotificationAlert = GetComponent<NotificationImageAlert>();
        NotificationAlert.Initialize();
        string startMessage = string.IsNullOrEmpty(StartMessage) ? "" : "\n" + StartMessage;
        if (!string.IsNullOrEmpty(startMessage))
            NotificationAlert.AddNewNotification();
        Messages = Colors.ConvertToColor("This is a private room with " + Name, Colors.ColorType.WHITE) + startMessage;
        m_messageInterface = GameObject.Find("DM Menu").GetComponent<DirectMessageInterface>();
    }

    public void SetAsCurrentRoom()
    {
        DirectMessageManager.Instance.SetCurrentMessageRoom(this);
        m_messageInterface.InitializeInputField();
        NotificationAlert.RemoveNotifications();
        m_parentNotification.UpdateStatus();
    }

    public void AddMessage(string message)
    {
        StringBuilder sb = new StringBuilder(Messages);
        sb.Append("\n");
        sb.Append(message);

        Messages = sb.ToString();

        if (DirectMessageManager.Instance.CurrentRoom != this)
        {
            NotificationAlert.AddNewNotification();
        }
    }
}
