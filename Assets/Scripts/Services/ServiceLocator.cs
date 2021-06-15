using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, IGameService> services = new Dictionary<Type, IGameService>();

    public static void Register<T>(IGameService serviceInstance)
    {
        var type = typeof(T);
        if (services.ContainsKey(typeof(T)))
        {
            Debug.LogError($"{type} is already a registered service.");
            return;
        }
        
        services[typeof(T)] = serviceInstance;
    }

    public static void Unregister<T>()
    {
        var type = typeof(T);
        if (!services.ContainsKey(typeof(T)))
        {
            Debug.LogError($"attempted to unregister service of type {type} which is not registered.");
            return;
        }

        services.Remove(typeof(T));
    }

    public static T TryGet<T>() where T : IGameService
    {
        var type = typeof(T);
        if (!services.ContainsKey(type))
        {
            return null;
        }
        return (T)services[typeof(T)];
    }

    public static T Get<T>() where T : IGameService
    {
        var type = typeof(T);
        if (!services.ContainsKey(type))
        {
            Debug.LogError($"{type} is not a registered service.");
            throw new InvalidOperationException();
        }
        return (T)services[typeof(T)];
    }

    public static void Reset()
    {
        services.Clear();
    }
}
