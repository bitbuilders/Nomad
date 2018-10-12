using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatRoomAssigner : Singleton<ChatRoomAssigner>
{
    public int TotalRoomCount { get; private set; }

    public int GetRoomID()
    {
        return ++TotalRoomCount;
    }
}
