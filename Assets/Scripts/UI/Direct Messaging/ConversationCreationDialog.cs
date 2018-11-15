using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ConversationCreationDialog : MonoBehaviour
{
    [SerializeField] Button m_createButton = null;

    TMP_InputField m_conversatioNameField;
    Animator m_animator;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Shrink();
        }
    }

    public void Initialize()
    {
        m_conversatioNameField = GetComponentInChildren<TMP_InputField>();
        m_animator = GetComponent<Animator>();
    }

    public void OnValueChange()
    {
        if (m_conversatioNameField.text.Contains("\n") && !string.IsNullOrEmpty(m_conversatioNameField.text.Trim()))
        {
            m_conversatioNameField.text = m_conversatioNameField.text.Replace("\n", "").Trim();
            m_createButton.onClick.Invoke();
            m_conversatioNameField.text = "";
            Shrink();
        }
        else if (string.IsNullOrEmpty(m_conversatioNameField.text.Trim()))
        {
            m_conversatioNameField.text = "";
        }
    }

    public void Expand()
    {
        m_animator.SetTrigger("Expand");
    }

    public void Shrink()
    {
        m_animator.SetTrigger("Shrink");
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
