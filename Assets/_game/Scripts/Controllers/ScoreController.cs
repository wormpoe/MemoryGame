using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

public class ScoreController : IDisposable, IService
{
    private int _score;
    public int Score => _score;

    private int _gold;
    public int Gold => _gold;

    private float _ratio;
    private int _correctCount;
    private int _baseAddScore;
    private float _ratioStep;
    private float _minRatio;
    private float _maxRatio;
    private int _diffId;
    private bool _isShielded;
    private List<TimeScoreData> _timeScore;
    private SignalBus _signalBus;
    private IDifficultLoader _difficultLoader;

    public void Init()
    {
        _difficultLoader = ServiceLocator.Current.Get<IDifficultLoader>();
        _signalBus = ServiceLocator.Current.Get<SignalBus>();
        _signalBus.Subscirbe<AddScoreSignal>(ScoreAdd);
        _signalBus.Subscirbe<RatioChangeSignal>(RatioChange);
        _signalBus.Subscirbe<FinalTimeSignal>(FinalScore);
        _signalBus.Subscirbe<ShieldBonusSignal>(ShieldBonusActivate);
        _score = 0;
        _gold = 0;
        _ratio = 1f;
        _correctCount = 0;
        _isShielded = false;
        OnInit();
    }


    private void ShieldBonusActivate(ShieldBonusSignal signal) => _isShielded = true;

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
        _baseAddScore = diffData.BaseScore;
        _ratioStep = diffData.RatioStep;
        _minRatio = diffData.MinScoreRatio;
        _maxRatio = diffData.MaxScoreRatio;
        _timeScore = diffData.TimeScore;
        _diffId = diffData.Id;
        _gold = diffData.WinGold;
    }

    private void ScoreAdd(AddScoreSignal signal)
    {
        _score += (int)(_baseAddScore * _ratio);
        _signalBus.Invoke(new ScoreChangeSignal(_score));
    }
    private void RatioChange(RatioChangeSignal signal)
    {
        if (!signal.IsCorrect && _isShielded)
        {
            _signalBus.Invoke(new BreakShieldSignal());
            _isShielded = false;
            return;
        }
        _correctCount = signal.IsCorrect ? _correctCount + 1 : 0;
        _ratio += signal.IsCorrect ? _ratioStep : -_ratioStep;
        _signalBus.Invoke(new StreakChangeSignal(_correctCount));
        if (_ratio > _maxRatio) _ratio = _maxRatio;
        else if (_ratio < _minRatio) _ratio = _minRatio;
        Debug.Log($"ratio{_ratio}/streak{_correctCount}");
    }
    private void FinalScore(FinalTimeSignal signal)
    {
        float finalTime = signal.Time;
        float timeRatio;
        Debug.Log(_score);
        foreach (var maxTime in _timeScore)
        {
            if (finalTime > maxTime.RequiredTime) return;
            timeRatio = maxTime.TimeRatio;
            Debug.Log($"{timeRatio} {finalTime}");
            _score = (int)(_score * timeRatio);
            _gold += maxTime.TimeBonusGold;
            break;
        }
        Debug.Log(_score);
        _signalBus.Invoke(new ScoreChangeSignal(_score));
        var winDialog = DialogManager.ShowDialog<WinDialog>();
        winDialog.Init(_gold, _score, finalTime, _diffId);

    }
    public void Dispose()
    {
        _signalBus.Unsubscribe<AddScoreSignal>(ScoreAdd);
        _signalBus.Unsubscribe<RatioChangeSignal>(RatioChange);
        _signalBus.Unsubscribe<FinalTimeSignal>(FinalScore);
    }
}
