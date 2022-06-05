using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour, IEvents
{
    #region Variables

    [SerializeField] private Transform _carryPoint;
    [SerializeField, Range(0.01f, 1f)] private float _lerpSpeed;
    [SerializeField, Min(0.3f)] private float _vertDist = 1f;

    private SuperStack<StackableController> _stackableStack = new SuperStack<StackableController>();
    private Transform _node, _connectedNode;

    #endregion // Variables

    #region Start

    private void Start()
    {
        SubscribeEvents();
    }

    #endregion // Start

    #region Updates

    private void Update()
    {
        StackStackables();  
    }

    #endregion // Updates

    #region Methods

    private void AddToStack(StackableController newStackable)
    {
        _stackableStack.Push(newStackable);
    }

    private void StackStackables()
    {
        for (int i = 0; i < _stackableStack.NumOfObjectsInCollection; i++)
        {
            _node = _stackableStack.Pull(i).transform;

            switch (i)
            {
                case 0:

                    _node.position = Vector3.Lerp(_node.position, _carryPoint.position, _lerpSpeed);

                    break;

                default:

                    _node.position = Vector3.Lerp(_node.position, _connectedNode.position + _vertDist * Vector3.up, _lerpSpeed);

                    break;
            }

            _connectedNode = _stackableStack.Pull(i).transform;
        }
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
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.StackCollected -= AddToStack;
    }

    #endregion // Events
}
