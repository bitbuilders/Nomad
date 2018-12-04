using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBattlePowerup : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 10.0f)] float m_spawnInTime = 1.0f;
    [SerializeField] [Range(0.0f, 20.0f)] float m_lifetime = 6.0f;

    public SpaceBattleShipPowerup.PowerupType Type { get; private set; }
    public int ID { get; private set; }

    SpaceBattle m_spaceBattle;
    float m_time;

    public void Initialize(SpaceBattle sb, int id, SpaceBattleShipPowerup.PowerupType powerup, Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
        ID = id;
        m_spaceBattle = sb;
        Type = powerup;
        StartCoroutine(SpawnIn());
        m_time = Time.time;
    }

    IEnumerator SpawnIn()
    {
        for (float i = 0.0f; i < m_spawnInTime; i += Time.deltaTime)
        {
            float a = i / m_spawnInTime;
            SetScale(a);
            yield return null;
        }
        SetScale(1.0f);
        yield return new WaitForSeconds(m_lifetime);
        for (float i = 0.0f; i < m_spawnInTime; i += Time.deltaTime)
        {
            float a = 1.0f - (i / m_spawnInTime);
            Vector3 scale = Vector3.one * a;
            transform.localScale = scale;
            yield return null;
        }
        SetScale(0.0f);
        Destroy(gameObject);
    }

    void SetScale(float s)
    {
        Vector3 scale = Vector3.one * s;
        transform.localScale = scale;
    }

    private void OnTriggerEnter(Collider other)
    {
        float delta = Time.time - m_time;
        if (delta < m_spawnInTime)
            return;
        
        SpaceBattleBullet sbb = other.GetComponent<SpaceBattleBullet>();
        if (sbb)
        {
            if (sbb.Sender != m_spaceBattle.PlayerNumber)
                return;

            AudioManager.Instance.PlaySoundClip("Powerup", "Powerup", transform.parent);
            m_spaceBattle.ObtainPowerup(sbb.Sender, Type, ID);
            // Boohoo :( I can't do this because I don't keep track of all bullets
            // on each client, meaning I can't give them the recently obtained powerup
            // Will still work normally tho

            //switch (Type)
            //{
            //    case SpaceBattleShipPowerup.PowerupType.BOUNCE:
            //        sbb.Bounce = true;
            //        break;
            //    case SpaceBattleShipPowerup.PowerupType.SHOTGUN:
            //        sbb.Shotgun++;
            //        sbb.Shotgun = Mathf.Clamp(sbb.Shotgun, 0, 2);
            //        break;
            //}
            Destroy(gameObject);
        }
    }
}
