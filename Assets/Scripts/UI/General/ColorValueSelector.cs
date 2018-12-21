using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorValueSelector : MonoBehaviour
{
    [SerializeField] ColorPicker m_colorPicker = null;
    [SerializeField] GameObject m_pointer = null;

    public Vector2 m_point;
    RectTransform m_rectTransform;
    Vector3[] m_corners;
    Color m_topRightColor;
    Material m_material;

    private void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_corners = new Vector3[4];
        m_rectTransform.GetLocalCorners(m_corners);
        m_material = Instantiate(GetComponent<Image>().material);
        GetComponent<Image>().material = m_material;
        m_point = new Vector2(0.5f, 0.5f);
    }

    public void SetPoint(Vector2 point)
    {
        m_point = point;
        SelectHue(false);
        m_rectTransform.GetLocalCorners(m_corners);
        float x = m_point.x * 93.0f;
        float y = m_point.y * 93.0f;
        m_pointer.transform.localPosition = new Vector3(x - 46.0f, y - 46.0f, 0.0f);
    }

    public void UpdateColor(Color topRightColor)
    {
        m_topRightColor = topRightColor;
        m_material.SetColor("_ColorTopRight", topRightColor);
        SelectHue(false);
    }

    public void GetLocalPointFromMousePosition(Vector3 offset)
    {
        //print(offset);

        m_rectTransform.GetLocalCorners(m_corners);
        m_point.x = (offset.x + (m_corners[2].x / 2.0f)) / m_corners[2].x;
        m_point.y = (offset.y + (m_corners[2].y / 2.0f)) / m_corners[2].y;
        m_point.x = Mathf.Clamp01(m_point.x);
        m_point.y = Mathf.Clamp01(m_point.y);

        SelectHue(true);
    }

    public void SelectHue(bool setPosition)
    {
        if (setPosition)
            m_pointer.transform.position = Input.mousePosition;

        Color c = Color.Lerp(Color.white, m_topRightColor, m_point.x);
        c = Color.Lerp(c, Color.black, 1.0f - m_point.y);
        c.a = 1.0f;

        m_colorPicker.OnColorValueChange(c);
    }
}
