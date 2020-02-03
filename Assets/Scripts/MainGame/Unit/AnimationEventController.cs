using System;
using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    public event Action AnimationStartEvent;
    public event Action AnimationEndEvent;

    public void StartMoveEndEvent()
    {
        AnimationEndEvent?.Invoke();
    }

    public void StartCanMoveEvent()
    {
        AnimationStartEvent?.Invoke();
    }

    public void ClearAnimationStartEvent()
    {
        AnimationStartEvent = null;
    }

    public void ClearAnimationEndEvent()
    {
        AnimationEndEvent = null;
    }
}
