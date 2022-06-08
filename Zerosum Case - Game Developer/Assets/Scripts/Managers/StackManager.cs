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
    private const float INSTANT_LERP = 1;

    #endregion // Variables

    #region Start

    private void Start()
    {
        SubscribeEvents();
    }

    #endregion // Start

    #region Updates

    private void FixedUpdate()
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

                    LerpToTargetPos(_carryPoint.position);

                    break;

                default:

                    LerpToTargetPos(_connectedNode.position + _vertDist * Vector3.up);

                    break;
            }

            _connectedNode = _stackableStack.Pull(i).transform;
        }
    }

    private void LerpToTargetPos(Vector3 targetPos)
    {
        _node.position = new Vector3(
            Mathf.Lerp(_node.position.x, targetPos.x, _lerpSpeed),
            Mathf.Lerp(_node.position.y, targetPos.y, _lerpSpeed),
            Mathf.Lerp(_node.position.z, targetPos.z, INSTANT_LERP));
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
