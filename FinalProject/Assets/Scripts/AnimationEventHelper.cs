using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHelper : MonoBehaviour
{
    public UnityEvent OnAnimationEventTrigger, OnAttackPerform;
    public void TriggerEvent()
    {
        OnAnimationEventTrigger?.Invoke();
    }

    public void TriggerAttack()
    {
        OnAttackPerform?.Invoke();
    }
}
