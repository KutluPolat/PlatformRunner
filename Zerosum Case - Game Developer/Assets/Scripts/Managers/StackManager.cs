using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zerosum.PlatformRunner.Enums;

public class StackManager : MonoBehaviour, IEvents
{
    #region Variables

    [SerializeField] private GameObject _moneyPrefab;
    [SerializeField] private Transform _carryPoint;
    [SerializeField, Range(0.01f, 1f)] private float _lerpSpeed;
    [SerializeField, Min(0.3f)] private float _vertDist = 1f;

    [HideInInspector] public bool IsStackFull;

    private SuperStack<StackableController> _stackables = new SuperStack<StackableController>();
    private Transform _node, _connectedNode;
    private const float INSTANT_LERP = 1;
    private int _currentNumOfStack;

    #endregion // Variables

    #region Start

    private void Start()
    {
        SaveSystem.Instance.CurrentNumOfStack = 0;
        SubscribeEvents();
        InitializeStack(ButtonType.StartingStack);
    }

    private void InitializeStack(ButtonType pressedButtonType)
    {
        if(pressedButtonType == ButtonType.StartingStack)
        {
            for (int i = _currentNumOfStack; i < SaveSystem.Instance.StartingNumOfStack; i++)
            {
                CreateNewStack();
            }
        }
    }

    private void CreateNewStack()
    {
        Vector3 spawnPos = GameManager.Instance.PlayerTransform.position + Random.insideUnitSphere * 2f;
        StackableController stackableController = Instantiate(_moneyPrefab, spawnPos, Quaternion.identity).GetComponent<StackableController>();
        EventManager.Instance.OnStackCollected(stackableController);
    }

    #endregion // Start

    #region Updates

    private void FixedUpdate()
    {
        StackUpwards();  
    }

    #endregion // Updates

    #region Methods

    #region Stacking Controls

    private void StackUpwards()
    {
        for (int i = 0; i < _currentNumOfStack; i++)
        {
            _node = _stackables.Pull(i).transform;

            switch (i)
            {
                case 0:

                    _node.transform.parent = _carryPoint;
                    _node.transform.localPosition = Vector3.zero;
                    _node.transform.localRotation = Quaternion.identity;

                    break;

                default:

                    LerpToTargetPos(_connectedNode.position + _vertDist * Vector3.up);
                    LerpToTargetRot(_carryPoint.rotation);

                    break;
            }

            _connectedNode = _stackables.Pull(i).transform;
        }
    }

    private void LerpToTargetPos(Vector3 targetPos)
    {
        _node.position = new Vector3(
            Mathf.Lerp(_node.position.x, targetPos.x, _lerpSpeed),
            Mathf.Lerp(_node.position.y, targetPos.y, _lerpSpeed),
            Mathf.Lerp(_node.position.z, targetPos.z, INSTANT_LERP));
    }

    private void LerpToTargetRot(Quaternion targetRot)
    {
        _node.rotation = Quaternion.Lerp(_node.rotation, targetRot, _lerpSpeed);
    }

    #endregion // Stacking Controls

    #region Collection Controls

    private void AddToStack(StackableController newStackable)
    {
        if (IsStackFull == false)
        {
            if (GameManager.Instance.IsGameStateEqualsTo(GameState.InGame))
            {
                GameManager.Instance.HapticHandler.MediumHaptic();
                AudioManager.Instance.PlayOneShotAudio(AudioNames.Collect, true, true);
            }

            newStackable.GetComponent<StackableAnimationHandler>().OnCollected();
            SaveSystem.Instance.AddToStackedGold(newStackable.GetCurrentStackable().Value);
            newStackable.IsCollected = true;
            _stackables.Push(newStackable);
        }
    }

    private void DropPart(StackableController trappedStackable)
    {
        int trappedIndex = _stackables.IndexOf(trappedStackable);
        int countCollection = _currentNumOfStack;

        for (int i = countCollection - 1; i >= 0; i--)
        {
            if (i >= trappedIndex) 
            {
                _stackables.Peek().transform.parent = null;
                SaveSystem.Instance.AddToStackedGold(_stackables.Peek().GetCurrentStackable().Value * -1);

                if (i > trappedIndex)
                {
                    _stackables.Peek().GetComponent<StackableAnimationHandler>().OnIdle();
                    _stackables.Peek().IsCollected = false;
                    DotweenExtensions.ThrowObjectAway(_stackables.Pop().transform, new Vector2Int(0, 2), new Vector2(1.5f, 3f), new Vector2(-3f, 3f), new Vector2(12f, 16f));
                }
                else if (i == trappedIndex)
                {
                    _stackables.Pop().DelayedDestroy();
                }
            }
        }
    }

    private void DropEverything(StackableController @null)
    {
        DropPart(null);
    }

    private void RemoveExchangedStackable(StackableController stackableController)
    {
        _stackables.Remove(stackableController);
    }

    private void OnStackUpdated()
    {
        _currentNumOfStack = SaveSystem.Instance.CurrentNumOfStack;
        IsStackFull = _currentNumOfStack >= SaveSystem.Instance.MaxNumOfStack;

        if(IsStackFull && GameManager.Instance.IsMovStateEqualsTo(MovementState.FreeToMove))
        {
            EventManager.Instance.OnFeverModeOn();
        }
        else if(IsStackFull == false && GameManager.Instance.IsMovStateEqualsTo(MovementState.Fever))
        {
            EventManager.Instance.OnFeverModeOff();
        }
    }

    #endregion // Collection Controls

    #endregion // Methods

    #region Events

    public void OnDestroy()
    {
        UnsubscribeEvents();
    }

    public void SubscribeEvents()
    {
        EventManager.Instance.StackCollected += AddToStack;
        EventManager.Instance.StackableExchanged += RemoveExchangedStackable;

        EventManager.Instance.PlayerTrapped += DropEverything;
        EventManager.Instance.StackableTrapped += DropPart;

        EventManager.Instance.StackUpdated += OnStackUpdated;
        EventManager.Instance.PressedUpgradeButton += InitializeStack;
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.StackCollected -= AddToStack;
        EventManager.Instance.StackableExchanged -= RemoveExchangedStackable;

        EventManager.Instance.PlayerTrapped -= DropEverything;
        EventManager.Instance.StackableTrapped -= DropPart;

        EventManager.Instance.StackUpdated -= OnStackUpdated;
        EventManager.Instance.PressedUpgradeButton -= InitializeStack;
    }

    #endregion // Events
}
