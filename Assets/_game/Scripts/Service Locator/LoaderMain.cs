using System;
using System.Collections.Generic;
using UnityEngine;

public class LoaderMain : MonoBehaviour
{
    [SerializeField] private ResolutionTracker _resolutionTracker;
    [SerializeField] private GUIHolder _guiHolder;
    [SerializeField] private DifficultLoader _difficultLoader;
    [SerializeField] protected BonusLoader _bonusLoader;
    [SerializeField] private SuitLoader _suitLoader;
    [SerializeField] private CardSpawner _cardsSpawner;
    [SerializeField] private BonusSpawner _bonusSpawner;
    [SerializeField] private HUD _hud;
    [SerializeField] private LensMove _lensMove;
    [SerializeField] private Shield _shield;

    private ConfigDataLoader _configDataLoader;
    private SignalBus _signalBus;

    private CardsController _cardsController;
    private TimeController _timeController;
    private ScoreController _scoreController;
    private GoldController _goldController;

    private IDifficultLoader _iDiffLoader;
    protected IBonusLoader _iBonusLoader;
    private ISuitLoader _iSuitLoader;

    private readonly List<IDisposable> _disposables = new();
    private void Awake()
    {
        _signalBus = new SignalBus();
        _cardsController = new CardsController();
        _timeController = new TimeController();
        _scoreController = new ScoreController();
        _goldController = new GoldController();
        _configDataLoader = new ConfigDataLoader();

        _iDiffLoader = _difficultLoader;
        _iBonusLoader = _bonusLoader;
        _iSuitLoader = _suitLoader;

        RegisterServices();
        Init();
        AddDisposables();
    }
    private void RegisterServices()
    {
        ServiceLocator.Init();

        ServiceLocator.Current.Register(_signalBus);
        ServiceLocator.Current.Register(_cardsController);
        ServiceLocator.Current.Register(_timeController);
        ServiceLocator.Current.Register(_scoreController);
        ServiceLocator.Current.Register(_goldController);
        ServiceLocator.Current.Register(_cardsSpawner);
        ServiceLocator.Current.Register(_bonusSpawner);
        ServiceLocator.Current.Register(_resolutionTracker);
        ServiceLocator.Current.Register(_guiHolder);
        ServiceLocator.Current.Register(_hud);
        ServiceLocator.Current.Register(_lensMove);
        ServiceLocator.Current.Register(_shield);

        ServiceLocator.Current.Register(_iDiffLoader);
        ServiceLocator.Current.Register(_iSuitLoader);
        ServiceLocator.Current.Register(_iBonusLoader);
    }

    private void Init()
    {
        _cardsController.Init();
        _timeController.Init();
        _scoreController.Init();
        _goldController.Init();
        _cardsSpawner.Init();
        _bonusSpawner.Init();
        _resolutionTracker.Init();
        _hud.Init();
        _lensMove.Init();
        _shield.Init();

        var loaders = new List<ILoader>
        {
            _iDiffLoader,
            _iSuitLoader,
            _iBonusLoader
        };
        _configDataLoader.Init(loaders);
    }
    private void AddDisposables()
    {
        _disposables.Add(_cardsController);
        _disposables.Add(_timeController);
        _disposables.Add(_scoreController);
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
