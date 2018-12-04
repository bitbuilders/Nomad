using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    public struct LingeringClip
    {
        public AudioSource AudioSource;
        public string ClipID;
    }

    [SerializeField] List<AudioData> m_soundClips = null;

    static AudioManager ms_instance;
    List<LingeringClip> m_lingeringClips;

    private void Awake()
    {
        m_lingeringClips = new List<LingeringClip>();
        PlayClipsWithAwake();
    }

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
    }

    void PlayClipsWithAwake()
    {
        foreach (AudioData soundClip in m_soundClips)
        {
            if (soundClip.PlayOnAwake)
            {
                PlaySoundClip(soundClip.ClipName, soundClip.ClipName);
            }
            if (soundClip.AddOnAwake)
            {
                AddSoundClip(soundClip.ClipName, soundClip.ClipName, null);
            }
        }
    }

    public GameObject PlaySoundClip(AudioClip clip, AudioMixerGroup output, string clipID, Vector3 position, bool setLocalPosition, bool loop, float volume, float pitch, float range, bool destroyAfter, bool global, Transform parent = null)
    {
        return PlayClip(clip, output, clipID, position, setLocalPosition, loop, volume, pitch, range, destroyAfter, global, parent);
    }

    public GameObject PlaySoundClip(string clipName, string clipID, Vector3 position, bool setLocalPosition, bool loop, float volume, float pitch, float range, bool destroyAfter, bool global, Transform parent = null)
    {
        AudioData data = GetSoundClipFromName(clipName);
        if (data == null)
            return null;

        return PlayClip(data.Clip, data.Output, clipID, position, setLocalPosition, loop, volume, pitch, range, destroyAfter, global, parent);
    }

    public GameObject PlaySoundClip(string clipName, string clipID, Transform parent = null)
    {
        AudioData data = GetSoundClipFromName(clipName);
        if (data == null)
            return null;

        return PlayClip(data.Clip, data.Output, clipID, data.Position, data.SetLocalPosition, data.Loop, data.Volume, data.Pitch, data.Range, data.DestroyAfter, data.Global, parent);
    }

    public GameObject AddSoundClip(string clipName, string clipID, Transform parent = null)
    {
        AudioData data = GetSoundClipFromName(clipName);
        if (data == null)
            return null;

        return PlayClip(data.Clip, data.Output, clipID, data.Position, data.SetLocalPosition, data.Loop, data.Volume, data.Pitch, data.Range, data.DestroyAfter, data.Global, parent, false);
    }

    GameObject PlayClip(AudioClip clip, AudioMixerGroup output, string clipID, Vector3 position, bool setLocalPosition, bool loop, float volume, float pitch, float range, bool destroyAfter, bool global, Transform parent, bool play = true)
    {
        GameObject go = new GameObject("Audio Clip: " + clip.name);
        if (parent == null)
            go.transform.parent = transform;
        else
            go.transform.parent = parent;
        if (setLocalPosition)
            go.transform.localPosition = position;
        else
            go.transform.position = position;
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = output;
        audioSource.clip = clip;
        audioSource.maxDistance = range;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.loop = loop;
        if (!destroyAfter)
            m_lingeringClips.Add(new LingeringClip() { AudioSource = audioSource, ClipID =  clipID });
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        if (global)
            audioSource.spatialBlend = 0.0f;
        else
            audioSource.spatialBlend = 1.0f;
        if (destroyAfter)
            Destroy(go, clip.length);

        if (play)
            audioSource.Play();

        return go;
    }

    public void StopClipFromID(string clipID, bool destroy)
    {
        foreach (LingeringClip clip in m_lingeringClips)
        {
            if (clip.ClipID == clipID)
            {
                clip.AudioSource.Stop();
                if (destroy)
                    Destroy(clip.AudioSource.gameObject);
            }
        }
    }

    public void StartClipFromID(string clipID, Vector3 position, bool setLocalPosition)
    {
        foreach (LingeringClip clip in m_lingeringClips)
        {
            if (clip.ClipID == clipID)
            {
                if (setLocalPosition)
                    clip.AudioSource.gameObject.transform.localPosition = position;
                else
                    clip.AudioSource.gameObject.transform.position = position;
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
