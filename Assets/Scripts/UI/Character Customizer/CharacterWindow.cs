using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterWindow : MonoBehaviour
{
    [SerializeField] ModelSelector m_modelSelector = null;
    [SerializeField] GameObject m_modelView = null;
    [SerializeField] ColorValueSelector m_hairValue = null;
    [SerializeField] ColorValueSelector m_glassesValue = null;

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
            if (result.gameObject == m_hairValue.gameObject)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 offset = Input.mousePosition - m_hairValue.transform.position;
                m_hairValue.GetLocalPointFromMousePosition(offset);
            }
            else if (result.gameObject == m_glassesValue.gameObject)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 offset = Input.mousePosition - m_glassesValue.transform.position;
                m_glassesValue.GetLocalPointFromMousePosition(offset);
            }
            else if (result.gameObject == m_modelView || result.gameObject.transform.IsChildOf(m_modelView.transform))
            {
                m_modelSelector.SetCharacterRotation();
            }
        }
    }
}
