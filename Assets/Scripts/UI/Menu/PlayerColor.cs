using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerColor : MonoBehaviour
{
    Image m_image;

    public void Initialize(string color)
    {
        m_image = GetComponent<Image>();
        m_image.color = Colors.StringToColor(color);
    }

    public void Select()
    {
        PlayerColorPicker.Instance.UpdateColor(m_image.color);
    }
}
