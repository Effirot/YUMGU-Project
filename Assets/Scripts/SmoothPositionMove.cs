using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothPositionMove : MonoBehaviour
{
    [field : SerializeField]
    public Transform target { get; set; } = null;
    [field : SerializeField]
    public float translatePositionSpeed { get; set; } = 0.2f;
    [field : SerializeField]
    public float translateRotationSpeed { get; set; } = 0.2f;

    void OnEnable()
    {
        if(target != null)
        {
            transform.position = transform.position;
            transform.rotation = transform.rotation;
        }
    }

    void LateUpdate()
    {
        if(target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, translatePositionSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, translateRotationSpeed);
        }
    }
}
