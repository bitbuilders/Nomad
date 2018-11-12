using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInviter : MonoBehaviour
{
    [SerializeField] TMP_InputField m_inviteInput = null;
    [SerializeField] ChatRoom m_chatRoom = null;

    Animator m_animator;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void SendInvite()
    {
        string name = m_inviteInput.text;
        int room = m_chatRoom.ID;
        string roomName = m_chatRoom.Name;
        Player localPlayer = LocalPlayerData.Instance.LocalPlayer;
        localPlayer.GetComponent<ChatRoomMessenger>().SendInviteToRoom(localPlayer.UserName, name, roomName, room);
        ResetInputField();
        m_animator.SetTrigger("Shrink");
    }

    public void ResetInputField()
    {
        gameObject.SetActive(true);
        m_inviteInput.text = "";
        m_inviteInput.ActivateInputField();
    }

    public void CancelInvite()
    {
        ResetInputField();
        m_animator.SetTrigger("Shrink");
    }

    public void OnValueChange()
    {
        if (m_inviteInput.text.Contains("\n"))
        {
            m_inviteInput.text = m_inviteInput.text.Replace("\n", "").Trim();
            SendInvite();
        }
    }
}
