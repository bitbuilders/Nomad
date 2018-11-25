using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUBMenu : MonoBehaviour
{
    [SerializeField] GameObject m_hideButton = null;
    [SerializeField] GameObject m_expandButton = null;

    Animator m_animator;
    float m_time;

    public bool Hidden { get; private set; }

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        Hidden = true;
        m_time = 0.0f;
    }

    public void Hide()
    {
        float delta = Time.time - m_time;
        bool longEnough = (delta >= 0.5f);
        if (!Hidden && longEnough)
        {
            m_animator.SetTrigger("SlideOutRight");
            Hidden = true;
            m_hideButton.SetActive(false);
            m_expandButton.SetActive(true);
            m_time = Time.time;
        }
    }

    public void Expand()
    {
        float delta = Time.time - m_time;
        bool longEnough = (delta >= 0.5f);
        if (Hidden && longEnough)
        {
            m_animator.SetTrigger("SlideInRight");
            Hidden = false;
            NotificationManager.Instance.RemoveHUBNotifications();
            m_hideButton.SetActive(true);
            m_expandButton.SetActive(false);
            m_time = Time.time;
        }
    }

    public void ToggleVisibility()
    {
        if (Hidden)
        {
            Expand();
        }
        else
        {
            Hide();
        }
    }

    public void ReturnToMenu()
    {
        GameLobby.Instance.ReturnToMainMenu();
    }
}
