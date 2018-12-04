using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBattleBullet : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 10.0f)] float m_acceleration = 4.5f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_maxSpeed = 3.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_lifetime = 3.0f;
    [SerializeField] [Range(0.0f, 5.0f)] float m_shotgunDelay = 0.35f;
    [SerializeField] [Range(0.0f, 360.0f)] float m_shotgunAngleCoverage = 90.0f;
    [SerializeField] List<Transform> m_bulletSpawns = null;

    public BillboardGame.PlayerType Sender { get; private set; }
    public bool Bounce { get; set; }
    public int Shotgun { get; set; }

    SpriteRenderer m_spriteRenderer;
    Vector3 m_direction;
    Vector3 m_velocity;
    float m_time;
    bool m_launched;

    public void Launch(Vector3 dir, Vector3 position, BillboardGame.PlayerType type, bool bounce, int shotgun, Color c)
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_spriteRenderer.color = c;
        transform.position = position;
        Sender = type;
        float angle = Mathf.Atan2(dir.y, dir.x);
        transform.localEulerAngles = Vector3.forward * (angle * Mathf.Rad2Deg);
        m_direction = dir.normalized;
        m_launched = true;

        Bounce = bounce;
        Shotgun = shotgun;
        SetBulletSpawnAngles();
    }

    void SetBulletSpawnAngles()
    {
        if (m_bulletSpawns.Count > 2)
        {
            float angleGrowth = m_shotgunAngleCoverage / m_bulletSpawns.Count;
            float angle = -angleGrowth * (m_bulletSpawns.Count / 2);
            for (int i = 0; i < m_bulletSpawns.Count; i++)
            {
                m_bulletSpawns[i].transform.localEulerAngles = new Vector3(angle, m_bulletSpawns[i].transform.localEulerAngles.y, 0.0f);
                angle += angleGrowth;
            }
        }
        else if (m_bulletSpawns.Count == 2)
        {
            float angle = m_shotgunAngleCoverage / 2.0f;
            m_bulletSpawns[0].transform.localEulerAngles = new Vector3(-angle, m_bulletSpawns[0].transform.localEulerAngles.y, 0.0f);
            m_bulletSpawns[1].transform.localEulerAngles = new Vector3(angle, m_bulletSpawns[1].transform.localEulerAngles.y, 0.0f);
        }
    }

    private void Update()
    {
        if (!m_launched)
            return;

        float speed = m_acceleration * Time.deltaTime;
        Vector3 velocity = m_direction * speed;
        m_velocity += velocity;
        if (m_velocity.magnitude > m_maxSpeed)
            m_velocity = m_velocity.normalized * m_maxSpeed;

        transform.position += (Vector3)m_velocity * Time.deltaTime;
        
        m_time += Time.deltaTime;
        if (m_time >= m_lifetime)
        {
            Destroy(gameObject);
        }

        if (Shotgun > 0)
        {
            if (m_time >= m_shotgunDelay)
            {
                Split(Shotgun - 1);
                Shotgun = 0;
            }
        }
    }

    void Split(int shotgun)
    {
        AudioManager.Instance.PlaySoundClip("Shoot", "Shoot", Vector3.zero, true, false, 0.15f, 0.8f, 10.0f, true, false, transform.parent);
        foreach (Transform t in m_bulletSpawns)
        {
            GameObject go = Instantiate(gameObject, transform.parent);
            go.GetComponent<SpaceBattleBullet>().Launch(t.forward, transform.position, Sender, Bounce, shotgun, m_spriteRenderer.color);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            return;

        SpaceBattleBullet sbb = other.GetComponent<SpaceBattleBullet>();
        if (sbb && sbb.Sender == Sender)
            return;

        SpaceBattleShip ship = other.GetComponent<SpaceBattleShip>();
        if (ship && ship.PlayerNumber == Sender)
            return;

        SpaceBattlePowerup sbp = other.GetComponent<SpaceBattlePowerup>();
        if (sbp)
            return;

        if (Bounce && ship == null)
        {
            SpaceBattleNormal sbn = other.GetComponent<SpaceBattleNormal>();
            if (sbn == null)
                return;
            
            Vector3 normal = sbn.Normal;
            m_direction =  Vector3.Reflect(m_direction, normal.normalized);
            m_velocity = m_direction * m_velocity.magnitude;
            float angle = Mathf.Atan2(m_direction.y, m_direction.x);
            transform.localEulerAngles = Vector3.forward * (angle * Mathf.Rad2Deg);

            AudioManager.Instance.PlaySoundClip("Bounce", "Bounce", transform.parent);
        }
        else
        {
            Destroy(gameObject, 0.01f);
        }
    }
}
