using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBattlePlayer : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 10.0f)] float m_maxSpeed = 1.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_acceleration = 0.5f;
    [SerializeField] [Range(0.0f, 1.0f)] float m_drag = 0.999f;
    [SerializeField] [Range(0.0f, 5.0f)] public float m_fireRate = 0.5f;

    public Vector2 Velocity { get { return m_velocity; } set { m_velocity = value; } }

    GameObject m_playerSprite;
    SpaceBattle sb;
    Vector2 m_velocity;
    float m_inY;

    private void Start()
    {
        sb = GetComponent<SpaceBattle>();
    }

    private void Update()
    {
        if (!sb.Playing)
            return;

        m_inY = Input.GetAxis("Vertical");
        float speed = m_acceleration * Time.deltaTime;
        m_velocity.y += m_inY * speed;
        m_velocity.y = Mathf.Clamp(m_velocity.y, -m_maxSpeed, m_maxSpeed);
    }

    private void FixedUpdate()
    {
        if (m_inY == 0.0f)
        {
            m_velocity.y *= m_drag;
        }
    }
}
