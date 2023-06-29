using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolManager<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T prefab;
    [SerializeField] private int capacity;
    [SerializeField] private bool populateOnDemand = true;

    private List<T> pool = new ();
    private List<T> inUsePool = new();

    protected abstract void OnCreateElement(T element);
    protected abstract void OnPullElement(T element);
    protected abstract void OnRecycleElement(T element);

    private void Awake()
    {
        for (int i= 0; i < capacity; i++)
        {
            pool.Add(CreateElement());
        }
    }

    private T CreateElement()
    {
        T element = Instantiate(prefab, transform);
        OnCreateElement(element);
        return element;
    }

    public T Pull()
    {
        T element;

        if (pool.Count > 0)
        {
            element = pool[0];
            pool.RemoveAt(0);
        }
        else
            element = CreateElement();

        OnPullElement(element);
        inUsePool.Add(element);

        return element;
    }

    public void Recycle(T element)
    {
        OnRecycleElement(element);
        inUsePool.Remove(element);
        pool.Add(element);
    }

}