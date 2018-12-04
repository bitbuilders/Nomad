using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    Animator m_animator;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void Click()
    {
        AudioManager.Instance.StartClipFromID("Button Click", Vector3.zero, false);
    }

    public void Swell()
    {
        m_animator.SetTrigger("Swell");
    }
}
