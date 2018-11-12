﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomUIManager : MonoBehaviour
{
    [SerializeField] RectTransform m_roomRoot = null;
    [SerializeField] TextMeshProUGUI m_hideButtonText = null;

    Animator m_animator;
    float m_time;
    bool m_hidden;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_hidden = false;
    }

    public void ToggleVisibility()
    {
        if (m_hidden)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void Hide()
    {
        float delta = Time.time - m_time;
        if (delta >= 0.5f)
        {
            m_time = Time.time;
            HideRooms();
            m_animator.SetTrigger("SlideOutTop");
            m_hidden = true;
            m_hideButtonText.text = "v";
        }
    }

    public void Show()
    {
        float delta = Time.time - m_time;
        if (delta >= 0.5f)
        {
            m_time = Time.time;
            m_animator.SetTrigger("SlideInTop");
            m_hidden = false;
            m_hideButtonText.text = "^";
        }
    }

    void HideRooms()
    {
        ChatRoom[] rooms = m_roomRoot.GetComponentsInChildren<ChatRoom>();

        foreach (ChatRoom room in rooms)
        {
            room.Hide();
        }
    }
}
