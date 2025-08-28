using System.Collections.Generic;
using UnityEngine;

public class BonusLoader : MonoBehaviour, IBonusLoader
{
    [SerializeField] private BonusesConfig _config;
    public IEnumerable<BonusData> GetBonusData()
    {
        return _config.BonusData;
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
