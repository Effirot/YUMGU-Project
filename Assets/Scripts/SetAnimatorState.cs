using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SetAnimatorState : MonoBehaviour
{
    [field : SerializeField]
    public Animator animator { get; set; }

    [field : SerializeField]
    public string state { get; set; }

    public void SetFloatState(float value)
    {
        animator.SetFloat(state, value);
    }
    public void SetIntState(int value)
    {
        animator.SetInteger(state, value);
    }   
    public void SetBoolState(bool value)
    {
        animator.SetBool(state, value);
    }
    public void InvokeTriggerState()
    {
        animator.SetTrigger(state);
    }
}
