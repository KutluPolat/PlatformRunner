using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class ObjectPool : MonoBehaviour
{
    [SerializeField, SceneObjectsOnly, BoxGroup("Object Pool")] private List<GameObject> _pool = new List<GameObject>();
    [SerializeField, SceneObjectsOnly, BoxGroup("Object Pool")] private Transform _parentOfPool;
    [SerializeField, BoxGroup("Object Pool")] private GameObject _prefabOfPoolObject;
    private float _originalScale;

    private Stack<GameObject> _availableObjects = new Stack<GameObject>();

    protected virtual void Awake()
    {
        foreach(GameObject gameObj in _pool)
        {
            _availableObjects.Push(gameObj);
        }

        _originalScale = _availableObjects.Peek().transform.localScale.x;
    }

    public GameObject GetObjectFromPool()
    {
        if(_availableObjects.Count > 0)
        {
            _availableObjects.Peek().transform.localScale = Vector3.one * _originalScale;
            _availableObjects.Peek().SetActive(true);
            return _availableObjects.Pop();
        }
        else
        {
            GameObject newGameObject = Instantiate(_prefabOfPoolObject, _parentOfPool.transform.position, _parentOfPool.transform.rotation, _parentOfPool);
            _availableObjects.Push(newGameObject);
            _pool.Add(newGameObject);
            return GetObjectFromPool();
        }
    }

    public void SendObjectBackToPool(GameObject targetObject)
    {
        if (_pool.Contains(targetObject))
        {
            _availableObjects.Push(targetObject);
            targetObject.SetActive(false);
        }
    }
}
