using Coati.GameName.Enums;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Movement : MonoBehaviour, IEvents
{
    #region Variables

    [SerializeField, BoxGroup("Movement Properties")] protected float _highestVerticalSpeedLimit = 14f, _lowestVerticalSpeedLimit = 7f, _verticalMovementSpeed = 0.1f;
    [SerializeField, BoxGroup("Rotation Properties")] protected float _rotationUpAngle = 50f, _rotationSpeed = 0.02f;

    protected Rigidbody _modelRigidbody;
    protected Transform _modelTransform;
    protected MovementState _currentMovementState;
    protected float _minimumLocalRotationY;
    protected bool _isRotationBlocked, _isHorizontalMovementBlocked;

    private Queue<float> _lastHorizontalDeltaPositions = new Queue<float>();
    private readonly int _numberOfDeltaPosInMemory = 15;
    protected float _maxDeltaPosition = 0.015f;

    protected float _currentMaxVerticalMovementSpeed;
    private float _totalDeltaPosition;

    private Vector3 _newPosition;

    #endregion // Variables

    #region Awake

    protected virtual void Awake()
    {
        _modelTransform = transform;
        _modelRigidbody = GetComponent<Rigidbody>();
        _minimumLocalRotationY = _rotationUpAngle / 120f;

        _currentMaxVerticalMovementSpeed = _highestVerticalSpeedLimit;
    }

    #endregion // Awake

    #region Movement

    private void BlockHorizontalMovementFor(float seconds)
    {
        StopAllCoroutines();
        StartCoroutine(BlockingCoroutine(seconds));
    }

    private IEnumerator BlockingCoroutine(float seconds)
    {
        _isHorizontalMovementBlocked = true;
        yield return new WaitForSeconds(seconds);
        _isHorizontalMovementBlocked = false;
    }

    protected void MoveForward()
    {
        if (GameManager.Instance.IsGameStateEqualsTo(GameState.InGame) && _currentMovementState == MovementState.FreeToMove)
        {
            _modelRigidbody.velocity = Vector3.Lerp(_modelRigidbody.velocity, Vector3.forward * _currentMaxVerticalMovementSpeed, _verticalMovementSpeed);
        }
    }

    protected void ClampMovement()
    {
        Vector3 modelPosition = _modelTransform.position;

        if (modelPosition.x > 4.15f || modelPosition.x < -4.15f)
        {
            _modelRigidbody.velocity = new Vector3(0, 0, _modelRigidbody.velocity.z);
        }

        modelPosition.x = Mathf.Clamp(modelPosition.x, -4.15f, +4.15f);
        _modelTransform.position = modelPosition;
    }

    private void BlockMovement()
    {
        _currentMovementState = MovementState.Blocked;
    }
    private void UnblockMovement()
    {
        _currentMovementState = MovementState.FreeToMove;
    }

    #endregion // Movement

    #region Rotation

    protected virtual void HandleRotation(float horizontalDeltaPosition)
    {
        AddToQueue(horizontalDeltaPosition);

        if (_isRotationBlocked == false)
        {
            _modelTransform.rotation = Quaternion.Lerp(_modelTransform.rotation, GetTargetRotation(), _rotationSpeed);
        }
    }

    private float GetAverageOfLatestInputs()
    {
        _totalDeltaPosition = 0;

        foreach (float deltaPosition in _lastHorizontalDeltaPositions)
            _totalDeltaPosition += deltaPosition;

        return _totalDeltaPosition / _lastHorizontalDeltaPositions.Count;
    }

    private void AddToQueue(float deltaPos)
    {
        _lastHorizontalDeltaPositions.Enqueue(deltaPos);

        if (_lastHorizontalDeltaPositions.Count > _numberOfDeltaPosInMemory)
            _lastHorizontalDeltaPositions.Dequeue();
    }
    private Quaternion GetTargetRotation()
    {
        float averageOfLatestInputs = GetAverageOfLatestInputs();
        float targetRotationMultiplier = averageOfLatestInputs / _maxDeltaPosition;

        if (averageOfLatestInputs == 0)
        {
            return new Quaternion(0, 0, 0, 1);
        }
        else
        {
            return new Quaternion(0, _minimumLocalRotationY * targetRotationMultiplier, 0, 1);
        }
    }

    #endregion // Rotation

    #region Events

    public void OnDestroy()
    {
        UnsubscribeEvents();
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.MovementBlocked += BlockMovement;
        EventManager.Instance.MovementUnblocked += UnblockMovement;
        EventManager.Instance.StateInGame += UnblockMovement;

        EventManager.Instance.StateEndingSequence += () => { _modelRigidbody.velocity = Vector3.zero; Destroy(this); };
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.MovementBlocked -= BlockMovement;
        EventManager.Instance.MovementUnblocked -= UnblockMovement;
        EventManager.Instance.StateInGame -= UnblockMovement;

        EventManager.Instance.StateEndingSequence += () => { _modelRigidbody.velocity = Vector3.zero; Destroy(this); };
    }

    #endregion // Events
}
