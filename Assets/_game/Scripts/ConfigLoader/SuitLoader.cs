using System.Collections.Generic;
using UnityEngine;

public class SuitLoader : MonoBehaviour, ISuitLoader
{
    [SerializeField] private SuitsConfig _config;

    public IEnumerable<SuitData> GetSuitData()
    {
        return _config.SuitData;
    }

    public bool IsLoaded()
    {
        return true;
    }

    public void Load()
    {
        var signalBus = ServiceLocator.Current.Get<SignalBus>();
        signalBus.Invoke(new DataLoadedSignal(this));
    }

}
