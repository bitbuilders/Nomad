using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{
    [SerializeField] float m_shotCooldown = 0.3f;
    [SerializeField] Transform m_firePosition = null;
    [SerializeField] ShotEffectsManager m_shotEffects = null;

    float m_ellapsedTime;
    bool m_canShoot;

    private void Start()
    {
        m_shotEffects.Initialize();

        if (isLocalPlayer)
            m_canShoot = true;
    }

    private void Update()
    {
        if (!m_canShoot)
            return;

        m_ellapsedTime += Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && m_ellapsedTime >= m_shotCooldown)
        {
            m_ellapsedTime = 0.0f;
            CmdFireShot(m_firePosition.position, m_firePosition.forward);
        }
    }

    // Run on server
    [Command]
    void CmdFireShot(Vector3 origin, Vector3 direction)
    {
        RaycastHit hit;

        Ray ray = new Ray(origin, direction);
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 1.0f);

        bool result = Physics.Raycast(ray, out hit, 50.0f);

        if (result)
        {
            PlayerHealth enemy = hit.transform.GetComponent<PlayerHealth>();

            if (enemy)
            {
                enemy.TakeDamage();
            }
        }

        RpcProcessShotEffects(result, hit.point);
    }

    // Run on clients from server
    [ClientRpc]
    void RpcProcessShotEffects(bool playImpact, Vector3 point)
    {
        m_shotEffects.PlayShotEffects();

        if (playImpact)
        {
            m_shotEffects.PlayImpactEffect(point);
        }
    }
}
