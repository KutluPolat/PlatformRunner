using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationHandler : MonoBehaviour
{
    [SerializeField] private bool _isRotationSpeedRandomized = true, _canRotationDirectionBeNegative = true;
    [ShowIf("@_isRotationSpeedRandomized"), SerializeField, MinMaxSlider(30, 100)] private Vector2 _rotationSpeedRange = new Vector2(40, 60);
    [ShowIf("@_isRotationSpeedRandomized == false"), SerializeField] private float _rotationSpeed = 50;
    private float _currentRotSpeed;

    private void Start()
    {
        _currentRotSpeed = _isRotationSpeedRandomized ? Random.Range(_rotationSpeedRange.x, _rotationSpeedRange.y) : _rotationSpeed;
        _currentRotSpeed = _canRotationDirectionBeNegative ? Random.value >= 0.5f ? _currentRotSpeed * -1f : _currentRotSpeed : _currentRotSpeed;
    }

    private void FixedUpdate()
    {
        transform.RotateAround(transform.position, transform.up, _currentRotSpeed * Time.deltaTime);
    }
}
