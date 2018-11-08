using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUBMenu : MonoBehaviour
{
    Animator m_animator;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void Hide()
    {
        m_animator.SetTrigger("SlideOutRight");
    }

    public void Expand()
    {
        m_animator.SetTrigger("SlideInRight");
    }
}
