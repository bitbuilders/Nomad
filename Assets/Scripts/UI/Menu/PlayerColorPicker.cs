using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using TMPro;

public class PlayerColorPicker : Singleton<PlayerColorPicker>
{
    [SerializeField] List<string> m_playerColors = null;
    [SerializeField] GameObject m_colorMenu = null;
    [SerializeField] RectTransform m_colorLocations = null;
    [SerializeField] GameObject m_playerColorTemplate = null;
    [SerializeField] Image m_currentColorImage = null;
    [SerializeField] TMP_InputField m_inputFieldImage = null;

    public string Color { get; private set; }
    
    TextMeshProUGUI m_text;
    bool m_colorsHidden;

    private void Start()
    {
        m_text = GetComponentInChildren<TextMeshProUGUI>();
        InitalizeColors();
        UpdateColor(m_currentColorImage.color);
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
        m_colorMenu.SetActive(true);
    }

    public void CloseColors()
    {

    }

    public void HideColors()
    {
        m_colorMenu.SetActive(false);
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
