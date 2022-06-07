using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool 
{
    private Queue<GameObject> _inactive;
    private List<GameObject> _active;
    private GameObject _prefab;

    public ObjectPool(GameObject prefab)
    {
        _prefab = prefab;
        _active = new List<GameObject>();
        _inactive = new Queue<GameObject>();
    }
    
    public GameObject Get()
    {
        GameObject obj = _inactive.Count > 0 ? _inactive.Dequeue() : Object.Instantiate(_prefab);
        obj.gameObject.SetActive(true);
        _active.Add(obj);
        return obj;
    }

    public void Return(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        if (_active.Contains(obj))
        {
            _active.Remove(obj);
        }
        
        _inactive.Enqueue(obj);
    }
}
