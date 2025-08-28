using System.Collections.Generic;
using System.Linq;

public class ConfigDataLoader : IService
{
    private List<ILoader> _loaders;
    private SignalBus _signalBus;

    private int _loadedSystem = 0;

    public void Init(List<ILoader> loaders)
    {
        _loaders = loaders;

        _signalBus = ServiceLocator.Current.Get<SignalBus>();

        _signalBus.Subscirbe<DataLoadedSignal>(OnConfigLoaded);

        DialogManager.ShowDialog<LoadingDialog>();

        LoadAll();
    }

    private void LoadAll()
    {
        foreach (var loader in _loaders)
        {
            loader.Load();
        }
    }

    private void OnConfigLoaded(DataLoadedSignal signal)
    {
        _loadedSystem++;
        _signalBus.Invoke(new LoadProgressChangedSignal((float)_loadedSystem / _loaders.Count));
        if (_loadedSystem == _loaders.Count)
        {
            _signalBus.Invoke(new AllDataLoadedSignal());
        }
    }

}
