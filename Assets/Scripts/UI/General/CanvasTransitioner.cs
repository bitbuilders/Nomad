using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasTransitioner : Singleton<CanvasTransitioner>
{
    public struct TransitionData
    {
        public RectTransform New;
        public RectTransform Old;
        public Vector3 NewTarget;
        public Vector3 OldTarget;
        public Vector3 NewStart;
        public Vector3 OldStart;
        public float Time;
    }

    public enum InterpolationType
    {
        LINEAR,
        ELASTIC_IN,
        ELASTIC_OUT,
        ELASTIC_IN_OUT,
        BACK_IN,
        BACK_OUT,
        BACK_IN_OUT,
        BOUNCE_IN,
        BOUNCE_OUT,
        BOUNCE_IN_OUT,
        SMOOTH_STEP,
        SMOOTHER_STEP,
        EXPO_IN,
        EXPO_OUT,
        EXPO_IN_OUT
    }

    public enum TransitionDirection
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    [Header("Anchors")]
    [SerializeField] RectTransform m_centerAnchor = null;
    [SerializeField] RectTransform m_leftAnchor = null;
    [SerializeField] RectTransform m_rightAnchor = null;
    [SerializeField] RectTransform m_topAnchor = null;
    [SerializeField] RectTransform m_bottomAnchor = null;
    [Space(15)]
    [Header("Roots")]
    [SerializeField] RectTransform m_centerRoot = null;
    [SerializeField] RectTransform m_leftRoot = null;
    [SerializeField] RectTransform m_rightRoot = null;
    [SerializeField] RectTransform m_topRoot = null;
    [SerializeField] RectTransform m_bottomRoot = null;
    [Space(15)]
    [Header("Values")]
    [SerializeField] [Range(0.0f, 50.0f)] float m_transitionSpeed = 5.0f;
    [SerializeField] InterpolationType m_enterInterpolation = InterpolationType.ELASTIC_IN_OUT;
    [SerializeField] InterpolationType m_leaveInterpolation = InterpolationType.BACK_IN;
    [Space(15)]
    [Header("Misc.")]
    [SerializeField] LayerMask m_playerMask = 0;

    Camera m_camera;
    TransitionData m_transition;
    RectTransform m_center;
    RectTransform m_left;
    RectTransform m_right;
    RectTransform m_top;
    RectTransform m_bottom;
    bool m_transitioning = false;

    private void Start()
    {
        m_center = m_centerRoot;
        m_left = m_leftRoot;
        m_right = m_rightRoot;
        m_top = m_topRoot;
        m_bottom = m_bottomRoot;
        m_camera = Camera.main;
        SetPlayerVisibility();
    }

    public void Transition(TransitionDirection direction, InterpolationType enterType, InterpolationType leaveType)
    {
        StartTransition(direction, enterType, leaveType);
    }

    public void Transition(TransitionDirection direction)
    {
        Transition(direction, m_enterInterpolation, m_leaveInterpolation);
    }

    void StartTransition(TransitionDirection direction, InterpolationType enterType, InterpolationType leaveType)
    {
        switch (direction)
        {
            case TransitionDirection.RIGHT:
                if (m_right == null)
                    return;
                m_left = m_center;
                m_center = m_right;
                m_transition.New = m_right;
                m_transition.NewStart = m_rightAnchor.position;
                m_transition.Old = m_left;
                m_transition.OldTarget = m_leftAnchor.position;
                m_right = null;
                break;
            case TransitionDirection.LEFT:
                if (m_left == null)
                    return;
                m_right = m_center;
                m_center = m_left;
                m_transition.New = m_left;
                m_transition.NewStart = m_leftAnchor.position;
                m_transition.Old = m_right;
                m_transition.OldTarget = m_rightAnchor.position;
                m_left = null;
                break;
            case TransitionDirection.DOWN:
                if (m_bottom == null)
                    return;
                m_top = m_center;
                m_center = m_bottom;
                m_transition.New = m_bottom;
                m_transition.NewStart = m_bottomAnchor.position;
                m_transition.Old = m_top;
                m_transition.OldTarget = m_topAnchor.position;
                m_bottom = null;
                break;
            case TransitionDirection.UP:
                if (m_top == null)
                    return;
                m_bottom = m_center;
                m_center = m_top;
                m_transition.New = m_top;
                m_transition.NewStart = m_topAnchor.position;
                m_transition.Old = m_bottom;
                m_transition.OldTarget = m_bottomAnchor.position;
                m_top = null;
                break;
        }

        m_transition.OldStart = m_centerAnchor.position;
        m_transition.NewTarget = m_centerAnchor.position;

        m_enterInterpolation = enterType;
        m_leaveInterpolation = leaveType;
        m_transitioning = true;

        if (m_transition.Time >= 1.0f)
            m_transition.Time = 0.0f;
        else if (m_transition.Time != 0.0f)
            m_transition.Time = 1.0f - m_transition.Time;

        HandlePlayerVisibility(0.09f);
    }

    void HandlePlayerVisibility(float delay)
    {
        StartCoroutine(DelayVisibility(delay));
    }

    IEnumerator DelayVisibility(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetPlayerVisibility();
    }

    void SetPlayerVisibility()
    {
        if (m_center == m_centerRoot)
        {
            m_camera.cullingMask &= ~m_playerMask;
        }
        else
        {
            m_camera.cullingMask |= m_playerMask;
        }
    }

    private void Update()
    {
        if (m_transitioning)
        {
            m_transition.Time += Time.deltaTime * m_transitionSpeed;
            Vector3 newPosition = GetInterpolatedPosition(m_transition.NewStart, m_transition.NewTarget, m_transition.Time, m_enterInterpolation);
            Vector3 oldPosition = GetInterpolatedPosition(m_transition.OldStart, m_transition.OldTarget, m_transition.Time, m_leaveInterpolation);
            m_transition.New.position = newPosition;
            m_transition.Old.position = oldPosition;
            if (m_transition.Time > 1.0f)
            {
                m_transitioning = false;
            }
        }
    }

    public Vector3 GetInterpolatedPosition(Vector3 start, Vector3 end, float time, InterpolationType type)
    {
        float t = 0.0f;

        switch (type)
        {
            case InterpolationType.LINEAR:
                t = Interpolation.Linear(time);
                break;
            case InterpolationType.ELASTIC_IN:
                t = Interpolation.ElasticIn(time);
                break;
            case InterpolationType.ELASTIC_OUT:
                t = Interpolation.ElasticOut(time);
                break;
            case InterpolationType.ELASTIC_IN_OUT:
                t = Interpolation.ElasticInOut(time);
                break;
            case InterpolationType.BACK_IN:
                t = Interpolation.BackIn(time);
                break;
            case InterpolationType.BACK_OUT:
                t = Interpolation.BackOut(time);
                break;
            case InterpolationType.BACK_IN_OUT:
                t = Interpolation.BackInOut(time);
                break;
            case InterpolationType.BOUNCE_IN:
                t = Interpolation.BounceIn(time);
                break;
            case InterpolationType.BOUNCE_OUT:
                t = Interpolation.BounceOut(time);
                break;
            case InterpolationType.BOUNCE_IN_OUT:
                t = Interpolation.BounceInOut(time);
                break;
            case InterpolationType.SMOOTH_STEP:
                t = Interpolation.SmoothStep(time);
                break;
            case InterpolationType.SMOOTHER_STEP:
                t = Interpolation.SmootherStep(time);
                break;
            case InterpolationType.EXPO_IN:
                t = Interpolation.ExpoIn(time);
                break;
            case InterpolationType.EXPO_OUT:
                t = Interpolation.ExpoOut(time);
                break;
            case InterpolationType.EXPO_IN_OUT:
                t = Interpolation.ExpoInOut(time);
                break;
        }

        Vector3 position = Vector3.Lerp(start, end, t);

        return position;
    }
}
