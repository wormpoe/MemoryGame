using System;
using System.Collections.Generic;
using UnityEngine;

public class SignalBus : IService
{
    private Dictionary<string, List<object>> _signalCallback = new Dictionary<string, List<object>>();

    public void Subscirbe<T>(Action<T> callback)
    {
        string key = typeof(T).Name;
        if(_signalCallback.ContainsKey(key))
        {
            _signalCallback[key].Add(callback);
        }
        else
        {
            _signalCallback.Add(key, new List<object>() { callback });
        }
    }
    public void Unsubscribe<T>(Action<T> callback)
    {
        string key = typeof(T).Name;
        if (_signalCallback.ContainsKey(key))
        {
            _signalCallback[key].Remove(callback);
        }
        else
        {
            Debug.LogError($"Trying to unsubscribe for not existing {key}!");
        }
    }
    public void Invoke<T>(T signal)
    {
        string key = typeof(T).Name;
        if(_signalCallback.ContainsKey(key))
        {
            foreach (var obj in _signalCallback[key])
            {
                var callback = obj as Action<T>;
                callback?.Invoke(signal);
            }
        }
    }
}
