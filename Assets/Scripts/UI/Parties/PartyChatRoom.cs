using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyChatRoom : MonoBehaviour
{
    Animator m_animator;
    bool m_left;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_left = false;
    }

    public void FlipLeft()
    {
        m_animator.SetTrigger("FlipLeft");
        m_left = true;
    }

    public void FlipRight()
    {
        if (m_left)
        {
            m_left = false;
            m_animator.SetTrigger("FlipRight");
        }
    }
}
