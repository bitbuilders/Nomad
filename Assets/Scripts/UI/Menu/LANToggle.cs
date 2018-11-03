using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MenuToggle : UnityEvent<bool> { }

public class LANToggle : MonoBehaviour
{
    [SerializeField] MenuToggle m_lanModeToggle = null;

    public bool LAN { get { return m_enabled; } }

    bool m_enabled;

    private void Start()
    {
        m_enabled = false;
    }

    public void Toggle()
    {
        m_enabled = !m_enabled;

        if (m_enabled)
        {
            EnableLAN();
        }
        else
        {
            DisableLAN();
        }
    }

    void EnableLAN()
    {
        m_lanModeToggle.Invoke(true);
    }

    void DisableLAN()
    {
        m_lanModeToggle.Invoke(false);
    }
}
