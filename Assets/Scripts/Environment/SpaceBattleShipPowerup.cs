using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBattleShipPowerup : MonoBehaviour
{
    public enum PowerupType
    {
        NONE,
        SHOTGUN,
        BOUNCE
    }
    
    [SerializeField] [Range(0.0f, 20.0f)] float[] m_durations = null;
    //[SerializeField] PowerupType m_activePowerups;

    float[] m_cooldownTimes;
    SpaceBattleShip m_ship;

    private void Awake()
    {
        m_cooldownTimes = new float[Enum.GetNames(typeof(PowerupType)).Length];
        m_ship = GetComponent<SpaceBattleShip>();
    }

    private void Update()
    {
        float delta = Time.deltaTime;
        PowerupType activeUps = PowerupType.NONE;
        for (int i = 1; i < m_cooldownTimes.Length; i++)
        {
            m_cooldownTimes[i] -= delta;
            if (m_cooldownTimes[i] > 0.0f)
            {
                activeUps |= (PowerupType)i;
            }
            else
            {
                RemovePowerup((PowerupType)i);
            }
        }
        activeUps &= ~PowerupType.NONE;
        //m_activePowerups = activeUps;
    }

    public void ObtainPowerup(PowerupType type)
    {
        int index = (int)type;
        m_cooldownTimes[index] = m_durations[index - 1];
        AddPowerup(type);
    }

    void RemovePowerup(PowerupType pt)
    {
        switch (pt)
        {
            case PowerupType.SHOTGUN:
                m_ship.Shotgun = 0;
                break;
            case PowerupType.BOUNCE:
                m_ship.Bounce = false;
                break;
        }
    }

    void AddPowerup(PowerupType pt)
    {
        switch (pt)
        {
            case PowerupType.SHOTGUN:
                m_ship.Shotgun++;
                m_ship.Shotgun = Mathf.Clamp(m_ship.Shotgun, 0, 2);
                break;
            case PowerupType.BOUNCE:
                m_ship.Bounce = true;
                break;
        }
    }
}
