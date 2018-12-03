using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpaceBattlePowerupSpawner : NetworkBehaviour
{
    [SerializeField] [Range(0.0f, 20.0f)] float m_powerupSpawnMin = 6.5f;
    [SerializeField] [Range(0.0f, 20.0f)] float m_powerupSpawnMax = 10.0f;
    [SerializeField] [Range(0.0f, 20.0f)] float m_powerupLocX = 2.0f;
    [SerializeField] [Range(0.0f, 20.0f)] float m_powerupLocY = 1.5f;

    [SyncVar] public int m_powerupID;
    BillboardMessenger m_bbm;
    float m_nextPowerupSpawn;
    float m_powerupTime;

    private void Start()
    {
        m_nextPowerupSpawn = UnityEngine.Random.Range(m_powerupSpawnMin, m_powerupSpawnMax);
    }

    private void Update()
    {
        m_powerupTime += Time.deltaTime;
        if (m_powerupTime >= m_nextPowerupSpawn)
        {
            m_powerupTime = 0.0f;
            Spawn();
        }
    }

    public void Spawn()
    {
        m_nextPowerupSpawn = UnityEngine.Random.Range(m_powerupSpawnMin, m_powerupSpawnMax);
        float minY = -m_powerupLocY / 2.0f;
        float maxY = m_powerupLocY / 2.0f;
        float minX = -m_powerupLocX / 2.0f;
        float maxX = m_powerupLocX / 2.0f;
        float x = UnityEngine.Random.Range(minX, maxX);
        float y = UnityEngine.Random.Range(minY, maxY);
        Vector3 position = new Vector3(x, y);
        var powerup = GetRandomPowerup();
        if (m_bbm == null)
            m_bbm = LocalPlayerData.Instance.LocalPlayer.GetComponent<BillboardMessenger>();
        m_bbm.SpawnPowerup(BillboardGame.GameName.SPACE_BATTLE, powerup, position, m_powerupID);
    }

    public SpaceBattleShipPowerup.PowerupType GetRandomPowerup()
    {
        int numOfPowerups = Enum.GetNames(typeof(SpaceBattleShipPowerup.PowerupType)).Length;
        int r = UnityEngine.Random.Range(1, numOfPowerups);
        return (SpaceBattleShipPowerup.PowerupType)r;
    }
}
