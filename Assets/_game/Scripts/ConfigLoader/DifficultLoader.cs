using System.Collections.Generic;
using UnityEngine;

public class DifficultLoader : MonoBehaviour, IDifficultLoader
{
    [SerializeField] private DifficultsConfig _config;

    public IEnumerable<DifficultData> GetDifficultData()
    {
        return _config.Difficults;
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
