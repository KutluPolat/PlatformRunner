using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour, IEvents
{
    #region Variables

    [SerializeField] private Transform _carryPoint;
    [SerializeField, Range(0.01f, 1f)] private float _lerpSpeed;
    [SerializeField, Min(0.3f)] private float _vertDist = 1f;

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
    }

    #endregion // Start

    #region Updates

    private void FixedUpdate()
    {
        StackUpwards();  
    }

    #endregion // Updates

    #region Methods

    private void AddToStack(StackableController newStackable)
    {
        newStackable.IsCollected = true;
        _stackables.Push(newStackable);
    }

    private void StackUpwards()
    {
        for (int i = 0; i < _currentNumOfStack; i++)
        {
            _node = _stackables.Pull(i).transform;

            switch (i)
            {
                case 0:

                    LerpToTargetPos(_carryPoint.position);

                    break;

                default:

                    LerpToTargetPos(_connectedNode.position + _vertDist * Vector3.up);

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

    private void DropPart(StackableController trappedStackable)
    {
        int trappedIndex = _stackables.IndexOf(trappedStackable);
        int countCollection = _currentNumOfStack;

        for (int i = countCollection - 1; i >= 0; i--)
        {
            if(i > trappedIndex)
            {
                _stackables.Peek().IsCollected = false;
                DotweenExtensions.ThrowObjectAway(_stackables.Pop().transform, new Vector2Int(0, 2), new Vector2(1.5f, 3f), new Vector2(-3f, 3f), new Vector2(12f, 16f));
            }
            else if(i == trappedIndex)
            {
                _stackables.Pop().DelayedDestroy();
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

        EventManager.Instance.StackUpdated += () => { _currentNumOfStack = SaveSystem.Instance.CurrentNumOfStack; };
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.StackCollected -= AddToStack;
        EventManager.Instance.StackableExchanged -= RemoveExchangedStackable;

        EventManager.Instance.PlayerTrapped -= DropEverything;
        EventManager.Instance.StackableTrapped -= DropPart;

        EventManager.Instance.StackUpdated -= () => { _currentNumOfStack = SaveSystem.Instance.CurrentNumOfStack; };
    }

    #endregion // Events
}
