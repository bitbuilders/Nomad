using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ConversationCreationDialog : MonoBehaviour
{
    [SerializeField] Button m_createButton = null;

    TMP_InputField m_conversatioNameField;

    private void Start()
    {
        m_conversatioNameField = GetComponentInChildren<TMP_InputField>();
    }

    public void OnValueChange()
    {
        if (m_conversatioNameField.text.Contains("\n") && !string.IsNullOrEmpty(m_conversatioNameField.text.Trim()))
        {
            m_conversatioNameField.text = m_conversatioNameField.text.Replace("\n", "").Trim();
            m_createButton.onClick.Invoke();
            gameObject.SetActive(false);
        }
        else if (string.IsNullOrEmpty(m_conversatioNameField.text.Trim()))
        {
            m_conversatioNameField.text = "";
        }
    }
}
