using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * This is the container, responsible for keeping the type to instance collection, which is used by the DependenciesProvider.
 */
public class Kernel
{
    private Dictionary<Type, object> container;

    public Kernel()
    {
        container = new Dictionary<Type, object>();
    }

    // TODO: Remove generic, use obj.GetType()
    public void Add<T>(T obj)
        where T: class
    {
        if(container.ContainsKey(typeof(T)))
        {
            Debug.LogError("Type {" + typeof(T).Name + "} already exists.");
            return;
        }

        container.Add(typeof(T), obj);
    }

    public List<T> GetAllOfType<T>()
        where T: class
    {
        List<T> list = container.Values
            .Where(x => (x as T) != null) 
            .Select(x => x as T)
            .ToList();

        if (list == null)
        {
            return new List<T>();
        }

        return list;
    }

    public T GetInstanceOfType<T>()
        where T: class
    {
        if(container.ContainsKey(typeof(T)))
        {
            return container[typeof(T)] as T;
        }
        else
        {
            Debug.LogWarning("Instance of type " + typeof(T).Name + " does not exist in the container.");
            return null;
        }
    }

    public void Clear()
    {
        container.Clear();
    }
}