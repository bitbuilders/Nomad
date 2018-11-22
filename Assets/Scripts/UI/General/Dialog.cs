using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    [SerializeField] bool m_showOnStart = false;

    private void Start()
    {
        if (m_showOnStart)
            Show();
    }

    public void Show()
    {
        PlayerMovement pm = LocalPlayerData.Instance.LocalPlayer.GetComponent<PlayerMovement>();
        pm.AddState(PlayerMovement.PlayerState.DIALOG);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        PlayerMovement pm = LocalPlayerData.Instance.LocalPlayer.GetComponent<PlayerMovement>();
        pm.RemoveState(PlayerMovement.PlayerState.DIALOG);
    }
}
