using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : Singleton<NotificationManager>
{
    [SerializeField] GameObject m_notificationTemplate = null;
    [SerializeField] Transform m_notificationLocation = null;
    [SerializeField] NotificationImageAlert m_notificationAlert = null;

    private void Start()
    {
        m_notificationAlert.Initialize();
    }

    public void CreateNotification(Notification.NotificationType type, string senderName, string roomName, int roomID)
    {
        GameObject go = Instantiate(m_notificationTemplate, Vector3.zero, Quaternion.identity, m_notificationLocation);
        go.transform.SetAsFirstSibling();
        Notification notification = go.GetComponent<Notification>();
        notification.Initialize(type, senderName, roomName, roomID);
        if (!GameLobby.Instance.HUBOpen)
            m_notificationAlert.AddNewNotification();
    }

    public void RemoveHUBNotifications()
    {
        m_notificationAlert.RemoveNotifications();
    }
}
