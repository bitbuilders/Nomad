using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSwipe : MonoBehaviour
{
    [SerializeField] ModelSelector.SwipeDirection m_direction = ModelSelector.SwipeDirection.LEFT;

    public void OnClick()
    {
        ModelSelector.Instance.SwipeModel(m_direction);
    }
}
