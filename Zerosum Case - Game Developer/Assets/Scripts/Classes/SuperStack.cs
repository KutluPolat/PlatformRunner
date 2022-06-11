using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperStack<T>
{
    private List<T> _collection = new List<T>();
    private int _lastIndex { get { return _collection.Count - 1; } }
    private T _objectCache;

    public void Push(T @object)
    {
        _collection.Add(@object);
        SaveSystem.Instance.CurrentNumOfStack = _collection.Count;
        EventManager.Instance.OnStackUpdated();
    }

    public T Pull(int index)
    {
        if (index < _collection.Count)
        {
            return _collection[index];
        }
        else
        {
            Debug.LogError("Index is out of range");
            Debug.Break();
            return _collection[0];
        }
    }

    public T Peek()
    {
        return _collection[_lastIndex];
    }

    public List<T> PeekAll()
    {
        return _collection;
    }

    public T Pop()
    {
        _objectCache = _collection[_lastIndex];
        _collection.RemoveAt(_lastIndex);
        SaveSystem.Instance.CurrentNumOfStack = _collection.Count;
        EventManager.Instance.OnStackUpdated(); 
        return _objectCache;
    }

    public bool Remove(T @object)
    {
        if (_collection.Contains(@object))
        {
            _collection.Remove(@object);
            SaveSystem.Instance.CurrentNumOfStack = _collection.Count;
            EventManager.Instance.OnStackUpdated();
            return true;
        }
        else
        {
            return false;
        }
    }

    public int IndexOf(T @object)
    {
        return _collection.IndexOf(@object);
    }
}
