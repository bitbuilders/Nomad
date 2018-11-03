using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NameChange : MonoBehaviour
{
    [SerializeField] Button m_saveButton = null;

    TMP_InputField m_nameInput;

    private void Start()
    {
        m_nameInput = GetComponent<TMP_InputField>();
    }

    public void OnValueChange()
    {
        if (m_nameInput.text.Contains("\n"))
        {
            m_saveButton.onClick.Invoke();
        }
    }
}
