using UnityEngine;
using System.Collections.Generic;
using System;

public class LoaderMenu : MonoBehaviour
{
    [SerializeField] private GUIHolder _guiHolder;
    [SerializeField] private DifficultLoader _difficultLoader;
    [SerializeField] private SuitLoader _suitLoader;
    [SerializeField] private BonusLoader _bonusLoader;

    private ConfigDataLoader _configDataLoader;
    private GoldController _goldController;

    private SignalBus _signalBus;

    private IDifficultLoader _idifficultLoader;
    private ISuitLoader _isuitLoader;
    private IBonusLoader _ibonusLoader;

    private readonly List<IDisposable> _disposables = new();
    private void Awake()
    {
        _signalBus = new SignalBus();
        _goldController = new GoldController();
        _configDataLoader = new ConfigDataLoader();

        _idifficultLoader = _difficultLoader;
        _isuitLoader = _suitLoader;
        _ibonusLoader = _bonusLoader;

        RegisterServices();
        Init();
        AddDisposables();
    }
    private void Init()
    {
        var loaders = new List<ILoader>
        {
            _idifficultLoader,
            _isuitLoader,
            _ibonusLoader,
        };

        _goldController.Init();
        _configDataLoader.Init(loaders);
    }
    private void RegisterServices()
    {
        ServiceLocator.Init();

        ServiceLocator.Current.Register(_signalBus);
        ServiceLocator.Current.Register(_guiHolder);
        ServiceLocator.Current.Register(_idifficultLoader);
        ServiceLocator.Current.Register(_isuitLoader);
        ServiceLocator.Current.Register(_goldController);
        ServiceLocator.Current.Register(_ibonusLoader);

    }
    private void AddDisposables()
    {
        _disposables.Add(_goldController);
    }

    private void OnDestroy()
    {
        foreach(var disposable in _disposables)
        {
            disposable.Dispose();
        }
    }
}
