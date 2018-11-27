using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class ConnectionManager : Singleton<ConnectionManager>
{
    [SerializeField] TextMeshProUGUI m_message = null;
    [SerializeField] [Range(0.0f, 10.0f)] float m_dotRate = 1.0f;
    [SerializeField] List<Dot> m_dots = null;

    public bool Hidden { get; private set; }

    Animator m_animator;
    float m_dotTime;
    int m_currentDot;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        Hidden = true;
        ResetDots();
    }

    private void Update()
    {
        if (!Hidden)
        {
            m_dotTime += Time.deltaTime;
            if (m_dotTime >= m_dotRate)
            {
                m_dotTime = 0.0f;
                m_dots[m_currentDot].FlyIn();
                m_currentDot++;
                m_currentDot %= m_dots.Count;
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
        ResetDots();
    }

    public void Hide()
    {
        if (Hidden)
            return;

        Hidden = true;
        m_animator.SetTrigger("Hide");
    }

    public void ShowConnectionMessage(string hostName)
    {
        m_message.text = "Connecting To " + hostName + "'s Game";
        Show();
    }

    public void ShowHostMessage()
    {
        m_message.text = "Creating Game";
        Show();
    }

    public void ShowServerMessage()
    {
        m_message.text = "Creating Server";
        Show();
    }

    public void ShowFailMessage()
    {
        m_message.text = "Connection Failed";
        StartCoroutine(HideDelay(1.0f));
    }

    IEnumerator HideDelay(float time)
    {
        yield return new WaitForSeconds(time);
        Hide();
    }

    void ResetDots()
    {
        m_dotTime = m_dotRate;
        m_currentDot = 0;
    }

    public void Cancel()
    {
        NomadNetworkManager.Instance.StopClient();
    }
}
