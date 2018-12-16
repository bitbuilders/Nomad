using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterWindow : MonoBehaviour
{
    [SerializeField] GameObject m_modelView = null;
    [SerializeField] 

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

        }
    }
}
