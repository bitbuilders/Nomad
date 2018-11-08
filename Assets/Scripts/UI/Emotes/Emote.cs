using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Emote", menuName = "Communication/Emote", order = 0)]
public class Emote : ScriptableObject
{
    [SerializeField] public Sprite Sprite;
    [SerializeField] public string Name;
}
