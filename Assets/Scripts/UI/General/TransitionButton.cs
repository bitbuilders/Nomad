using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionButton : MonoBehaviour
{
    [SerializeField] CanvasTransitioner.TransitionDirection m_transitionDirection = CanvasTransitioner.TransitionDirection.LEFT;

    public void Transition()
    {
        CanvasTransitioner.Instance.Transition(m_transitionDirection);
    }
}
