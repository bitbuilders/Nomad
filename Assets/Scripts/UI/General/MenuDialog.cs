using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDialog : MonoBehaviour
{
    public bool Hidden { get; private set; }

    Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        Hidden = true;
        gameObject.SetActive(false);
    }

    public void ToggleVisibility()
    {
        if (Hidden)
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
        if (Hidden)
            return;

        Hidden = true;
        m_animator.SetTrigger("QuitShrink");
    }

    public void Show()
    {
        if (!Hidden && gameObject.activeInHierarchy)
            return;

        Hidden = false;
        gameObject.SetActive(true);
        m_animator.SetTrigger("Swell");
    }

    public void DeActivate()
    {
        gameObject.SetActive(false);
    }

    public void Cancel()
    {
        Hide();
    }

    public void Return()
    {
        Hide();
        GameLobby.Instance.ReturnToMainMenu();
    }
}
