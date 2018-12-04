using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundClip", menuName = "Audio/SoundClip", order = 0)]
public class AudioData : ScriptableObject
{
    public AudioClip Clip;
    public AudioMixerGroup Output;
    public string ClipName;
    public Vector3 Position;
    public bool SetLocalPosition;
    [Range(0.0f, 1000.0f)] public float Range;
    [Range(0.0f, 1.0f)] public float Volume;
    [Range(-3.0f, 3.0f)] public float Pitch;
    public bool Loop;
    public bool PlayOnAwake;
    public bool AddOnAwake;
    public bool Global;
    public bool DestroyAfter;
}
