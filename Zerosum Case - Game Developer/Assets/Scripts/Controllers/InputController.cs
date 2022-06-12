using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zerosum.PlatformRunner.Enums;

public class InputController : MonoBehaviour
{
    #region Variables

    [SerializeField] private MovementController _movementController;

    private Vector3 _firstMousePosition;
    private bool _isPressed;
    private float _horizontalDeltaPosition;

    private readonly float _minimumDeltaPositionToActivateInput = 0.025f;

    #endregion // Variables

    #region Updates

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        _movementController.HandleMovementAndRotation(_horizontalDeltaPosition);
        _horizontalDeltaPosition = 0;
    }

    #endregion // Updates

    #region Methods

    public void OnPressedTapToPlayButton()
    {
        EventManager.Instance.OnStateInGame();
        EventManager.Instance.OnStackUpdated();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) && _isPressed == false)
        {
            switch (GameManager.Instance.GetCurrentGameState())
            {
                case GameState.LevelEnd:

                    if (GameManager.Instance.IsSuccess)
                    {
                        EventManager.Instance.OnPressedNextLevel();
                    }
                    else
                    {
                        EventManager.Instance.OnPressedRestart();
                    }

                    break;
            }

            _isPressed = true;
            _firstMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0) && _isPressed)
        {
            float deltaMousePosition = Input.mousePosition.x - _firstMousePosition.x;

            if (Mathf.Abs(deltaMousePosition) > _minimumDeltaPositionToActivateInput)
            {
                _horizontalDeltaPosition = (deltaMousePosition * 1 / Screen.width);
                _firstMousePosition = Input.mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0) && _isPressed)
        {
            _movementController.StopHorizontalMovement();

            _isPressed = false;
            _horizontalDeltaPosition = 0;
            _firstMousePosition = Input.mousePosition;
        }
    }

    #endregion // Methods
}
