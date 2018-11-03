using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : Singleton<NotificationManager>
{
    [SerializeField] GameObject m_notificationTemplate = null;
    [SerializeField] Transform m_notificationLocation = null;

    public void CreateNotification(Notification.NotificationType type, string senderName, string roomName, int roomID)
    {
        GameObject go = Instantiate(m_notificationTemplate, Vector3.zero, Quaternion.identity, m_notificationLocation);
        go.transform.SetAsFirstSibling();
        Notification notification = go.GetComponent<Notification>();
        notification.Initialize(type, senderName, roomName, roomID);
    }
}
