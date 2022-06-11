using Zerosum.PlatformRunner.Enums;
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

    private float _rightEdge, _leftEdge;

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

    #region Start

    protected virtual void Start()
    {
        _rightEdge = GameManager.Instance.RightEdgeOfPlatform;
        _leftEdge = GameManager.Instance.LeftEdgeOfPlatform;
    }

    #endregion // Start

    #region Movement

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

        if (modelPosition.x > _rightEdge || modelPosition.x < _leftEdge)
        {
            _modelRigidbody.velocity = new Vector3(0, 0, _modelRigidbody.velocity.z);
        }

        modelPosition.x = Mathf.Clamp(modelPosition.x, _leftEdge, _rightEdge);
        _modelTransform.position = modelPosition;
    }

    private void PushPlayerBack()
    {
        _modelRigidbody.velocity = Vector3.zero;
        _modelRigidbody.angularVelocity = Vector3.zero;

        _modelTransform.DOMove(_modelTransform.position + Vector3.back * 10f, 0.5f).OnComplete(() =>
        {
            DotweenExtensions.PunchScale(_modelTransform);
        });
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

    public float GetAverageOfLatestInputs()
    {
        _totalDeltaPosition = 0;

        foreach (float deltaPosition in _lastHorizontalDeltaPositions)
            _totalDeltaPosition += deltaPosition;

        return _lastHorizontalDeltaPositions.Count == 0 ? 0 : _totalDeltaPosition / _lastHorizontalDeltaPositions.Count;
    }

    protected void AddToQueue(float deltaPos)
    {
        _lastHorizontalDeltaPositions.Enqueue(deltaPos);

        if (_lastHorizontalDeltaPositions.Count > _numberOfDeltaPosInMemory)
            _lastHorizontalDeltaPositions.Dequeue();
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

        EventManager.Instance.PlayerTrapped += (value) => PushPlayerBack();

        EventManager.Instance.StateEndingSequence += () => { _modelRigidbody.velocity = Vector3.zero; Destroy(this); };
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.MovementBlocked -= BlockMovement;
        EventManager.Instance.MovementUnblocked -= UnblockMovement;
        EventManager.Instance.StateInGame -= UnblockMovement;

        EventManager.Instance.PlayerTrapped -= (value) => PushPlayerBack();

        EventManager.Instance.StateEndingSequence += () => { _modelRigidbody.velocity = Vector3.zero; Destroy(this); };
    }

    #endregion // Events
}
