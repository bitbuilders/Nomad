using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerColorPicker : Singleton<PlayerColorPicker>
{
    [SerializeField] List<string> m_playerColors = null;
    [SerializeField] GameObject m_colorMenu = null;
    [SerializeField] RectTransform m_colorLocations = null;
    [SerializeField] GameObject m_playerColorTemplate = null;
    [SerializeField] Image m_currentColorImage = null;
    [SerializeField] TMP_InputField m_inputFieldImage = null;

    public string Color { get; private set; }

    //TextMeshProUGUI m_text;
    UIButton m_uiButton;
    bool m_colorsHidden;
    

    private void Start()
    {
        m_uiButton = GetComponent<UIButton>();
        //m_text = GetComponentInChildren<TextMeshProUGUI>();
        InitalizeColors();
        UpdateColor(m_currentColorImage.color);
        m_colorsHidden = true;
    }

    void InitalizeColors()
    {
        foreach (string color in m_playerColors)
        {
            GameObject go = Instantiate(m_playerColorTemplate, Vector3.zero, Quaternion.identity, m_colorLocations);
            PlayerColor pc = go.GetComponent<PlayerColor>();
            pc.Initialize(color);
        }
    }

    public void OpenColors()
    {
        if (m_colorsHidden)
        {
            m_colorsHidden = false;
            m_uiButton.Swell();
            m_uiButton.Click();
            m_colorMenu.GetComponent<Animator>().SetTrigger("Expand");
        }
    }

    public void CloseColors()
    {
        if (!m_colorsHidden)
        {
            m_colorsHidden = true;
            m_colorMenu.GetComponent<Animator>().SetTrigger("Shrink");
        }
    }

    public void UpdateColor(Color color)
    {
        Color = "#" + ColorUtility.ToHtmlStringRGBA(color);
        //m_text.color = color;
        m_currentColorImage.color = color;
        ColorBlock cb = m_inputFieldImage.colors;
        cb.normalColor = color;
        m_inputFieldImage.colors = cb;
    }
}
