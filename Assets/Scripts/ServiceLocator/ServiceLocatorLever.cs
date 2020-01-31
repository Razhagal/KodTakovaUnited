using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

/*
 * This class is responsible for calling the methods in ServiceLocator.
 * It also provides monobehaviour dependencies, which references are given in the inspector.
 * This is added in the Script Execution Order as one that is called earlier than the other scripts.
 */
public class ServiceLocatorLever : MonoBehaviour
{
    private void Awake()
    {
        Dictionary<Type, object> monoDependencies = new Dictionary<Type, object>();

        //monoDependencies.Add(typeof(TicksManager), thicksManager);

        foreach (var monoDependency in monoDependencies)
        {
            if(monoDependency.Value == null)
            {
                Debug.LogError("Dependency is missing: " + monoDependency.Key.Name);
            }
        }

        ServiceLocator.Instance.Awake(monoDependencies);
    }

    private void Start()
    {
        ServiceLocator.Instance.Start();
    }

    private IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();

        ServiceLocator.Instance.LateStart();
    }

    private void OnDestroy()
    {
        ServiceLocator.Instance.Dispose();

        ServiceLocator.Instance.Clear();
    }
}
