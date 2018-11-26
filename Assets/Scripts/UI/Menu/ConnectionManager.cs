using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class ConnectionManager : Singleton<ConnectionManager>
{
    [SerializeField] TextMeshProUGUI m_message = null;
    [SerializeField] TextMeshProUGUI m_dotText = null;
    [SerializeField] [Range(1, 50)] int m_dotCount = 5;
    [SerializeField] [Range(0.0f, 5.0f)] float m_dotSpeed = 0.5f;

    public bool Hidden { get; private set; }

    Animator m_animator;
    float m_dotTime;
    int m_dots;
    bool m_expanding;

    private void Start()
    {
        m_dots = 0;
        m_animator = GetComponent<Animator>();
        Hidden = true;
        m_dotText.text = "";
        m_expanding = true;
    }

    private void Update()
    {
        m_dotTime += Time.deltaTime;
        if (m_dotTime >= m_dotSpeed)
        {
            m_dotTime = 0.0f;

            if (m_dots >= m_dotCount)
                m_expanding = false;
            else if (m_dots <= 0)
                m_expanding = true;

            if (m_expanding)
                m_dots++;
            else
                m_dots--;
            
            if (m_dots > 0)
            {
                m_dotText.text = ".";
                for (int i = 1; i < m_dots; i++)
                {
                    m_dotText.text += ".";
                }
            }
            else
            {
                m_dotText.text = "";
            }
        }
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

    public void Show()
    {
        if (!Hidden)
            return;

        Hidden = false;
        m_animator.SetTrigger("Show");
    }

    public void Hide()
    {
        if (Hidden)
            return;

        Hidden = true;
        m_animator.SetTrigger("Hide");
    }

    public void ShowConnectionMessage(string message, string hostName)
    {
        m_message.text = "Connecting To " + hostName + "'s Game";
    }
}
