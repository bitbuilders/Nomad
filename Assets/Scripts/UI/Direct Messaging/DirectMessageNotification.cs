using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectMessageNotification : MonoBehaviour
{
    [SerializeField] GameObject m_notificationImage = null;

    public int NewNotifications { get; private set; }

    public void Initialize()
    {
        NewNotifications = 0;
    }

    public void AddNewNotification()
    {
        NewNotifications++;
        UpdateStatus();
    }

    public void RemoveNotifications()
    {
        NewNotifications = 0;
        UpdateStatus();
    }

    public void UpdateStatus()
    {
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
