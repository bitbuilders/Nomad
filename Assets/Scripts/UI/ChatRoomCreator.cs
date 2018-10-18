using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatRoomCreator : MonoBehaviour
{
    public void CreateChatRoom()
    {
        //GetComponent<NetworkData>().CmdIncrementRoomID();
        //int room = GetComponent<NetworkData>().CurrentRoomID;
        //ChatRoomManager.Instance.CreateChatRoom(room);
        LocalPlayerData.Instance.LocalPlayer.GetComponent<PlayerChatRoomManager>().CmdCreateChatRoom();
    }
}
