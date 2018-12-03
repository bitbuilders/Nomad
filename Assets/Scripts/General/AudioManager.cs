using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    public struct LoopingClip
    {
        public AudioSource AudioSource;
        public string ClipID;
    }

    [SerializeField] List<AudioData> m_soundClips = null;

    static AudioManager ms_instance;
    List<LoopingClip> m_loopingClips;

    private void Start()
    {
        if (ms_instance == null)
        {
            ms_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (ms_instance != this)
        {
            Destroy(gameObject);
        }

        m_loopingClips = new List<LoopingClip>();
        PlayClipsWithAwake();
    }

    void PlayClipsWithAwake()
    {
        foreach (AudioData soundClip in m_soundClips)
        {
            if (soundClip.PlayOnAwake)
            {
                PlaySoundClip(soundClip.ClipName, soundClip.ClipName);
            }
        }
    }

    public GameObject PlaySoundClip(AudioClip clip, AudioMixerGroup output, string clipID, Vector3 position, bool loop, float volume, float pitch, float range, bool destroyAfter, bool global)
    {
        return PlayClip(clip, output, clipID, position, loop, volume, pitch, range, destroyAfter, global);
    }

    public GameObject PlaySoundClip(string clipName, string clipID, Vector3 position, bool loop, float volume, float pitch, float range, bool destroyAfter, bool global)
    {
        AudioData data = GetSoundClipFromName(clipName);

        return PlayClip(data.Clip, data.Output, clipID, position, loop, volume, pitch, range, destroyAfter, global);
    }

    public GameObject PlaySoundClip(string clipName, string clipID)
    {
        AudioData data = GetSoundClipFromName(clipName);

        return PlayClip(data.Clip, data.Output, clipID, data.Position, data.Loop, data.Volume, data.Pitch, data.Range, data.DestroyAfter, data.Global);
    }

    GameObject PlayClip(AudioClip clip, AudioMixerGroup output, string clipID, Vector3 position, bool loop, float volume, float pitch, float range, bool destroyAfter, bool global)
    {
        GameObject go = new GameObject("Audio Clip: " + clip.name);
        go.transform.parent = transform;
        go.transform.position = position;
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = output;
        audioSource.clip = clip;
        audioSource.maxDistance = range;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.loop = loop;
        if (loop && !destroyAfter)
            m_loopingClips.Add(new LoopingClip() { AudioSource = audioSource, ClipID =  clipID });
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        if (global)
            audioSource.spatialBlend = 0.0f;
        else
            audioSource.spatialBlend = 1.0f;
        if (destroyAfter)
            Destroy(go, clip.length);

        audioSource.Play();

        return go;
    }

    public void StopClipFromID(string clipID, bool destroy)
    {
        foreach (LoopingClip clip in m_loopingClips)
        {
            if (clip.ClipID == clipID)
            {
                clip.AudioSource.Stop();
                if (destroy)
                    Destroy(clip.AudioSource.gameObject);
            }
        }
    }

    public void StartClipFromID(string clipID)
    {
        foreach (LoopingClip clip in m_loopingClips)
        {
            if (clip.ClipID == clipID)
            {
                clip.AudioSource.Play();
            }
        }
    }

    AudioData GetSoundClipFromName(string clipName)
    {
        AudioData clip = null;

        foreach (AudioData ad in m_soundClips)
        {
            if (ad.ClipName == clipName)
            {
                clip = ad;
                break;
            }
        }

        return clip;
    }
}
