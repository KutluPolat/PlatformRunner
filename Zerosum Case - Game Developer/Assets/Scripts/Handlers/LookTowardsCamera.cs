using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTowardsCamera : MonoBehaviour
{
    private Transform _cameraTransform;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _cameraTransform.position);
    }
}
