using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationImageAlert : MonoBehaviour
{
    [SerializeField] GameObject m_notificationImage = null;
    [SerializeField] bool m_isParent = false;

    public int NewNotifications { get; private set; }

    List<NotificationImageAlert> m_children;

    public void Initialize()
    {
        m_children = new List<NotificationImageAlert>();
        NewNotifications = 0;
        UpdateStatus();
    }

    public void AddChild(NotificationImageAlert child)
    {
        m_children.Add(child);
    }

    public void AddNewNotification()
    {
        NewNotifications++;
        UpdateStatus();
    }

    public void RemoveNotification()
    {
        NewNotifications--;
        UpdateStatus();
    }

    public void RemoveNotifications()
    {
        NewNotifications = 0;
        UpdateStatus();
    }

    public void UpdateStatus(int ignoredNotifications = 0)
    {
        if (NewNotifications < 0)
            NewNotifications = 0;

        if (m_isParent)
        {
            // To allow for some notifications to keep passthrough
            NewNotifications = ignoredNotifications;
            foreach (NotificationImageAlert child in m_children)
            {
                NewNotifications += child.NewNotifications;
            }
        }

        if (NewNotifications > 0)
        {
            m_notificationImage.SetActive(true);
        }
        else
        {
            m_notificationImage.SetActive(false);
        }
    }
}
