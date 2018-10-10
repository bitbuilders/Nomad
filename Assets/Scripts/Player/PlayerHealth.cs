using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] int m_maxHealth = 3;

    Player m_player;
    int m_health;

    private void Awake()
    {
        m_player = GetComponent<Player>();
    }

    [ServerCallback]
    private void OnEnable()
    {
        m_health = m_maxHealth;
    }

    [Server]
    public bool TakeDamage()
    {
        bool died = false;

        if (m_health <= 0)
            return died;

        m_health--;
        died = m_health <= 0;

        RpcTakeDamage(died);

        return died;
    }

    [ClientRpc]
    void RpcTakeDamage(bool died)
    {
        if (died)
            m_player.Die();
    }
}
