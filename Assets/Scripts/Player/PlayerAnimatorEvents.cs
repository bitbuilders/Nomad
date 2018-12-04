using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvents : MonoBehaviour
{
    [SerializeField] AudioClip m_footstepSound = null;
    [SerializeField] [Range(0.0f, 1.0f)] float m_footstepPitchVariation = 0.1f;
    [SerializeField] [Range(0.0f, 1.0f)] float m_footstepVolumeVariation = 0.1f;

    PlayerMovement m_playerMovement;
    AudioSource m_audioSource;
    float m_originalFootstepPitch;
    float m_originalFootstepVolume;

    private void Start()
    {
        m_playerMovement = GetComponentInParent<PlayerMovement>();
        m_audioSource = GetComponent<AudioSource>();
        m_audioSource.clip = m_footstepSound;
        m_originalFootstepPitch = m_audioSource.pitch;
        m_originalFootstepVolume = m_audioSource.volume;
        //GameObject go = AudioManager.Instance.AddSoundClip("Jump", "Jump", transform.parent);
        //go.transform.localPosition = Vector3.zero;
    }

    public void Jump()
    {
        m_playerMovement.Jump();
        AudioManager.Instance.StartClipFromID("Jump", transform.position, false);
    }

    public void EnableMovement()
    {
        m_playerMovement.EnableMovement();
    }

    public void DisableMovement()
    {
        m_playerMovement.DisableMovement();
    }

    public void PlayFootstep()
    {
        float pitchVariation = Random.Range(-m_footstepPitchVariation, m_footstepPitchVariation);
        float volumeVariation = Random.Range(-m_footstepVolumeVariation, m_footstepVolumeVariation);
        m_audioSource.pitch = m_originalFootstepPitch + pitchVariation;
        m_audioSource.volume = m_originalFootstepVolume + volumeVariation;
        m_audioSource.Play();
    }
}
