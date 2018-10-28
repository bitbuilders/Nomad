using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvents : MonoBehaviour
{
    PlayerMovement m_playerMovement;

    private void Start()
    {
        m_playerMovement = GetComponentInParent<PlayerMovement>();
    }

    public void Jump()
    {
        m_playerMovement.Jump();
    }

    public void EnableMovement()
    {
        m_playerMovement.EnableMovement();
    }

    public void DisableMovement()
    {
        m_playerMovement.DisableMovement();
    }
}
