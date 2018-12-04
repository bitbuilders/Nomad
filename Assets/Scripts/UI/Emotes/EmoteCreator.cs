using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmoteCreator : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 5.0f)] float m_fadeInTime = 0.1f;
    [SerializeField] [Range(0.0f, 5.0f)] float m_fadeOutTime = 0.1f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_linger = 3.0f;
    [SerializeField] Camera m_camera;

    Animator m_animator;
    Image m_image;
    RectTransform m_rectTransform;
    float m_time;
    bool m_active;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_image = GetComponent<Image>();
        m_rectTransform = GetComponent<RectTransform>();
        m_active = false;

        m_image.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    //private void Update()
    //{
    //    if (m_active)
    //    {
    //        m_time += Time.deltaTime;

    //        if (m_time >= m_linger)
    //        {
    //            RemoveEmote();
    //        }
    //    }
    //}

    private void Update()
    {
        if (m_active)
        {
            m_time += Time.deltaTime;

            if (m_time >= m_linger)
            {
                RemoveEmote();
            }
        }

        if (!m_camera.gameObject.activeInHierarchy)
            m_camera = Camera.main;

        Vector3 dir = transform.position - m_camera.gameObject.transform.position;
        Quaternion look = Quaternion.LookRotation(dir);
        m_rectTransform.rotation = look;
    }

    public void CreateEmote(Sprite image)
    {
        if (!m_active)
        {
            StartCoroutine(DelayedSound(0.15f));
            m_image.sprite = image;
            m_time = 0.0f;
            m_active = true;
            Fade(true, m_fadeInTime);
            // Do animation
            m_animator.SetTrigger("Spawn");
        }
    }

    IEnumerator DelayedSound(float time)
    {
        yield return new WaitForSeconds(time);
        AudioManager.Instance.StartClipFromID("Emote", transform.position, false);
    }

    void RemoveEmote()
    {
        m_time = 0.0f;
        m_active = false;
        Fade(false, m_fadeOutTime);
        m_animator.SetTrigger("Stop");
    }

    void Fade(bool fadeIn, float time)
    {
        float a = (fadeIn) ? 1.0f : 0.0f;
        UIJuice.Instance.FadeAlpha(m_image, a, fadeIn, time, false);
    }
}
