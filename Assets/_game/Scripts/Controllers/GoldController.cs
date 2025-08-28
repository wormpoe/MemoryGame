using System;
using UnityEngine;

public class GoldController : IService, IDisposable
{
    private int _gold;
    public int Gold => _gold;

    private SignalBus _signalBus;

    public void Init()
    {
        _gold = PlayerPrefs.GetInt(StringConstant.GOLD_COUNT, 0);
        if (_gold < 0)
        {
            PlayerPrefs.SetInt(StringConstant.GOLD_COUNT, 0);
        }
        _signalBus = ServiceLocator.Current.Get<SignalBus>();

        _signalBus.Subscirbe<GoldAddSignal>(OnAddGold);
        _signalBus.Subscirbe<SpendGoldSignal>(OnSpendGold);
        _signalBus.Subscirbe<GoldChangeSignal>(GoldChange);
    }

    private void OnSpendGold(SpendGoldSignal signal)
    {
        _gold -= signal.Value;
        _signalBus.Invoke(new GoldChangeSignal(_gold));
    }

    private void OnAddGold(GoldAddSignal signal)
    {
        _gold += signal.Value;
        _signalBus.Invoke(new GoldChangeSignal(_gold));
    }

    private void GoldChange(GoldChangeSignal signal)
    {
        PlayerPrefs.SetInt(StringConstant.GOLD_COUNT, signal.Gold);
    }

    public bool HaveEnoughGold(int price)
    {
        return _gold >= price;
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<GoldAddSignal>(OnAddGold);
        _signalBus.Unsubscribe<GoldChangeSignal>(GoldChange);
        _signalBus.Unsubscribe<SpendGoldSignal>(OnSpendGold);
    }
}
