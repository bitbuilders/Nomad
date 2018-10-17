using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerData : Singleton<LocalPlayerData>
{
    public Player LocalPlayer { get; set; }
}
