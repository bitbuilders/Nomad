﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] [Range(0.0f, 20.0f)] float m_distanceFromTarget = 3.0f;
    [SerializeField] [Range(0.0f, 20.0f)] float m_lookOffset = 3.0f;
    [SerializeField] [Range(0.0f, 180.0f)] float m_acceleration = 10.0f;
    [SerializeField] [Range(0.0f, 180.0f)] float m_maxSpeedX = 2.0f;
    [SerializeField] [Range(0.0f, 180.0f)] float m_maxSpeedY = 4.0f;
    [SerializeField] [Range(0.0f, 180.0f)] float m_idleFriction = 10.0f;
    [Header("Limits")]
    [SerializeField] [Range(0.0f, -90.0f)] float m_maxPitch = -35.0f;
    [SerializeField] [Range(0.0f, 90.0f)] float m_minPitch = 65.0f;
    [SerializeField] LayerMask m_groundMask = 0;
    [Header("Input")]
    [SerializeField] GameObject m_target = null;

    Vector3 m_targetPosition;
    Vector3 m_rotation;
    Vector3 m_speed;

    private void Start()
    {
        Vector3 dir = transform.position - m_target.transform.position;
        m_targetPosition = m_target.transform.position + Vector3.up;
        transform.position = dir.normalized * m_distanceFromTarget + m_targetPosition;
        m_rotation = transform.rotation.eulerAngles;
    }

    private void LateUpdate()
    {
        m_targetPosition = m_target.transform.position + Vector3.up;

        Vector3 straightLine = Vector3.back * m_distanceFromTarget + m_targetPosition;
        Vector3 line = straightLine - m_targetPosition;
        line = Quaternion.Euler(m_rotation) * line;
        Vector3 pos = line + m_targetPosition;
        RaycastHit raycastHit;
        if (Physics.Raycast(m_targetPosition, line, out raycastHit, m_distanceFromTarget, m_groundMask))
        {
            Vector3 newLine = line.normalized * (raycastHit.distance);
            transform.position = newLine + m_targetPosition + Vector3.up * 0.25f;
        }
        else
        {
            transform.position = pos + Vector3.up * 0.25f;
        }

        //transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * m_stiffness);

        Vector3 dir = m_targetPosition - transform.position;
        dir.y = 0.0f;
        Vector3 offset = dir.normalized * m_lookOffset + m_targetPosition;
        Vector3 lookDir = offset - transform.position;
        transform.rotation = Quaternion.LookRotation(lookDir);
    }

    private void FixedUpdate()
    {
        float inX = Input.GetAxis("Mouse X");
        float inY = -Input.GetAxis("Mouse Y");
        float speed = m_acceleration * Time.deltaTime;
        m_speed.x += inY * speed;
        m_speed.y += inX * speed;

        m_speed.x = Mathf.Clamp(m_speed.x, -m_maxSpeedX, m_maxSpeedX);
        m_speed.y = Mathf.Clamp(m_speed.y, -m_maxSpeedY, m_maxSpeedY);

        if (inX == 0.0f && Mathf.Abs(m_speed.y) > 0.35f)
        {
            float opp = (m_speed.y > 0.0f) ? -1.0f : 1.0f;
            m_speed.y += opp * m_idleFriction * Time.deltaTime;
        }
        else if (inX == 0.0f && Mathf.Abs(m_speed.y) < 0.35f)
        {
            m_speed.y = 0.0f;
        }

        if (inY == 0.0f && Mathf.Abs(m_speed.x) > 0.35f)
        {
            float opp = (m_speed.x > 0.0f) ? -1.0f : 1.0f;
            m_speed.x += opp * m_idleFriction * Time.deltaTime;
        }
        else if (inY == 0.0f && Mathf.Abs(m_speed.x) < 0.35f)
        {
            m_speed.x = 0.0f;
        }

        m_rotation += m_speed;
        m_rotation.x = Mathf.Clamp(m_rotation.x, m_maxPitch, m_minPitch);
    }
}
