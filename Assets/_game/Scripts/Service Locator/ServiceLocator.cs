using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator
{
    private ServiceLocator() { }
    private readonly Dictionary<string, IService> _service = new();
    public static ServiceLocator Current { get; private set; }
    public static void Init()
    {
        Current = new ServiceLocator();
    }
    public T Get<T>() where T : IService
    {
        string key = typeof(T).Name;
        if (!_service.ContainsKey(key))
        {
            Debug.LogError($"{key} not registrated with {GetType().Name}");
            throw new InvalidOperationException();
        }
        return (T)_service[key];
    }
    public void Register<T>(T service) where T : IService
    {
        string key = typeof(T).Name;
        if (_service.ContainsKey(key))
        {
            Debug.LogError($"Attemted to register service of type {key} which is already registred with the {GetType().Name}");
            return;
        }
        _service.Add(key, service);
    }
    public void Unregister<T>() where T : IService
    {
        string key = typeof(T).Name;
        if(!_service.ContainsKey(key))
        {
            Debug.LogError($"Attemted to unregister service of type {key} which is not registred with the {GetType().Name}");
            return;
        }
        _service.Remove(key);
    }
}
