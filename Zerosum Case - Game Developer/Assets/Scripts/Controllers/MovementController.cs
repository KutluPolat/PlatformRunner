using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zerosum.PlatformRunner.Enums;
using Sirenix.OdinInspector;
using DG.Tweening;

public class MovementController : Movement
{
    #region Variables

    [SerializeField, BoxGroup("Movement Properties")] private float _horizontalMovementSpeed = 500f;

    #endregion // Variables

    #region Start

    protected override void Start()
    {
        base.Start();
        SubscribeEvents();
    }

    #endregion // Start

    #region Updates

    private void LateUpdate()
    {
        ClampMovement();
    }

    #endregion // Updates

    #region Methods

    public void StopHorizontalMovement()
    {
        if (GameManager.Instance.IsGameStateEqualsTo(GameState.InGame))
        {
            _modelRigidbody.velocity = new Vector3(0, _modelRigidbody.velocity.y, _modelRigidbody.velocity.z);
        }
    }

    public void HandleMovementAndRotation(float horizontalDeltaPosition)
    {
        MoveForward();

        if (_isHorizontalMovementBlocked)
            return;

        if (GameManager.Instance.IsGameStateEqualsTo(GameState.InGame))
        {
            AddToQueue(horizontalDeltaPosition);
            _modelRigidbody.velocity = new Vector3(horizontalDeltaPosition * _horizontalMovementSpeed * (int)GameManager.Instance.GetMovState(), 0, _modelRigidbody.velocity.z);
        }
    }

    #endregion // Methods
}
