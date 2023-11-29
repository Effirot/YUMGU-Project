using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UntwistedGipsumDryState : MonoBehaviour
{
    [SerializeField]
    private Transform waterCollisionTransform;

    [SerializeField]
    private bool isDrying = false;

    [SerializeField]
    private UnityEvent OnGipsumDryied = new();

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.transform == waterCollisionTransform && !isDrying)
        {
            isDrying = true;
            OnGipsumDryied.Invoke();
        }
    }
}