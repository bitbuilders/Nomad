using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBattleShip : MonoBehaviour
{
    public enum FirePosition
    {
        LEFT,
        RIGHT,
        NONE
    }

    [SerializeField] GameObject m_bullet = null;
    [SerializeField] Color m_p1Color = Color.blue;
    [SerializeField] Color m_p2Color = Color.green;
    [SerializeField] List<Transform> m_bulletSpawns = null;

    public SpaceBattle SpaceBattle { get; set; }
    public BillboardGame.PlayerType PlayerNumber { get; set; }
    public int Shotgun { get; set; }
    public bool Bounce { get; set; }

    Color m_bulletColor;
    int m_spawnLocation;

    public void Initialize(SpaceBattle sb, BillboardGame.PlayerType pt)
    {
        SpaceBattle = sb;
        PlayerNumber = pt;
        m_spawnLocation = 0;

        switch (pt)
        {
            case BillboardGame.PlayerType.P1:
                m_bulletColor = m_p1Color;
                break;
            case BillboardGame.PlayerType.P2:
                m_bulletColor = m_p2Color;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PlayerNumber == SpaceBattle.PlayerNumber)
            return;

        SpaceBattleBullet sbb = other.GetComponent<SpaceBattleBullet>();
        if (sbb && sbb.Sender != PlayerNumber && SpaceBattle.Playing)
        {
            SpaceBattle.TakeDamage(PlayerNumber, other.transform.position);
        }
    }

    public FirePosition Fire(FirePosition fp = FirePosition.NONE)
    {
        GameObject go = Instantiate(m_bullet, Vector3.zero, Quaternion.identity, transform.parent);
        Transform t = m_bulletSpawns[m_spawnLocation++];
        if (fp != FirePosition.NONE)
        {
            switch (fp)
            {
                case FirePosition.LEFT:
                    t = m_bulletSpawns[0];
                    m_spawnLocation = 1;
                    break;
                case FirePosition.RIGHT:
                    t = m_bulletSpawns[1];
                    m_spawnLocation = 0;
                    break;
            }
        }
        m_spawnLocation %= m_bulletSpawns.Count;
        go.GetComponent<SpaceBattleBullet>().Launch(t.forward, t.position, PlayerNumber, Bounce, Shotgun, m_bulletColor);

        int prevFire = m_spawnLocation - 1;
        if (prevFire < 0)
            prevFire = m_bulletSpawns.Count - 1;

        AudioManager.Instance.PlaySoundClip("Shoot", "Shoot", transform);
        return (FirePosition)prevFire;
    }
}
