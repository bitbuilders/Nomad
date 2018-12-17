using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterWindow : MonoBehaviour
{
    [SerializeField] ModelSelector m_modelSelector = null;
    [SerializeField] GameObject m_modelView = null;
    [SerializeField] ColorHueSelector m_hairHue = null;
    [SerializeField] ColorHueSelector m_glassesHue = null;

    GraphicRaycaster m_raycaster;
    PointerEventData m_pointerEventData;
    EventSystem m_eventSystem;

    private void Start()
    {
        m_raycaster = GetComponent<GraphicRaycaster>();
        m_eventSystem = FindObjectOfType<EventSystem>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastUIHit();
        }
    }

    void RaycastUIHit()
    {
        m_pointerEventData = new PointerEventData(m_eventSystem);
        m_pointerEventData.position = Input.mousePosition;

        List<RaycastResult> rayResults = new List<RaycastResult>();
        m_raycaster.Raycast(m_pointerEventData, rayResults);
        foreach (RaycastResult result in rayResults)
        {
            if (result.gameObject == m_hairHue.gameObject)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 offset = Input.mousePosition - m_hairHue.transform.position;
                m_hairHue.GetLocalPointFromMousePosition(offset);
            }
            else if (result.gameObject == m_glassesHue.gameObject)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 offset = Input.mousePosition - m_glassesHue.transform.position;
                m_glassesHue.GetLocalPointFromMousePosition(offset);
            }
            else if (result.gameObject == m_modelView || result.gameObject.transform.IsChildOf(m_modelView.transform))
            {
                m_modelSelector.SetCharacterRotation();
            }
        }
    }
}
