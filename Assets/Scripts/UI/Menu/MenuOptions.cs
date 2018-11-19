using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptions : MonoBehaviour
{
    Animator m_animator;
    bool m_expanded;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_expanded = false;
    }

    public void Expand()
    {
        m_expanded = true;
        m_animator.SetTrigger("Expand");
    }

    public void Shrink()
    {
        m_expanded = false;
        m_animator.SetTrigger("Shrink");
    }

    public void ToggleSize()
    {
        if (m_expanded)
        {
            Shrink();
        }
        else
        {
            Expand();
        }
    }
}
