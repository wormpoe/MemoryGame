using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Linq;

public class TimeController : IDisposable, IService
{
    private bool _isGameRunning = false;
    private SignalBus _signalBus;
    private float _currentTime = 0f;
    public float CurrentTime => _currentTime;
    private float _startTime = 0f;
    private int _diffId;
    private IDifficultLoader _difficultLoader;
    public void Init()
    {
        _difficultLoader = ServiceLocator.Current.Get<IDifficultLoader>();
        _signalBus = ServiceLocator.Current.Get<SignalBus>();
        _signalBus.Subscirbe<StartGameSignal>(StartTimer);
        _signalBus.Subscirbe<StopGameSignal>(StopTimer);
        _signalBus.Subscirbe<EndGameSignal>(GameStop);

        OnInit();
    }

    private async void OnInit()
    {
        await UniTask.WaitUntil(_difficultLoader.IsLoaded);

        var id = PlayerPrefs.GetInt(StringConstant.CURRENT_DIFFICULT, 0);
        var diffData = _difficultLoader.GetDifficultData().FirstOrDefault(x => x.Id == id);
        if (diffData == null)
        {
            Debug.LogError($"Can't find difficult with id {id}");
            return;
        }
        _diffId = diffData.Id;
    }
    private async void RunTimer()
    {
        while (_isGameRunning)
        {
            _currentTime = Time.time - _startTime;
            _signalBus.Invoke(new TimerChangeSignal(_currentTime));
            await UniTask.Yield();
        }
    }

    private void StartTimer(StartGameSignal signal)
    {
        _startTime = Time.time - _currentTime;
        _isGameRunning = true;
        RunTimer();
    }
    private void StopTimer(StopGameSignal signal)
    {
        _isGameRunning = false;
    }
    private void GameStop(EndGameSignal signal)
    {
        _isGameRunning = false;
        _signalBus.Invoke(new FinalTimeSignal(_currentTime));
    }
    public void Dispose()
    {
        _signalBus.Unsubscribe<StartGameSignal>(StartTimer);
        _signalBus.Unsubscribe<StopGameSignal>(StopTimer);
        _signalBus.Unsubscribe<EndGameSignal>(GameStop);
    }
}
