using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Nametag : MonoBehaviour
{
    [SerializeField] Camera m_camera;
    
    RectTransform m_rectTransform;
    TextMeshProUGUI m_nameText;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_nameText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (!m_camera.gameObject.activeInHierarchy)
            m_camera = Camera.main;

        Vector3 dir = transform.position - m_camera.gameObject.transform.position;
        Quaternion look = Quaternion.LookRotation(dir);
        m_rectTransform.rotation = look;
    }

    public void UpdateName(string name)
    {
        m_nameText.text = name;
    }

    public void UpdateColor(Color color)
    {
        if (m_nameText)
            m_nameText.color = color;
    }
}
