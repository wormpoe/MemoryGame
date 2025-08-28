using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;

public class BonusController : IService
{
    private IBonusLoader _bonusLoader;
    private SignalBus _signalBus;

    public void Init()
    {
        _bonusLoader = ServiceLocator.Current.Get<IBonusLoader>();
        _signalBus = ServiceLocator.Current.Get<SignalBus>();
        

    }
    private async void OnInit()
    {
        await UniTask.WaitUntil(_bonusLoader.IsLoaded);

        var bonusData = _bonusLoader.GetBonusData();
        if (bonusData == null)
        {

        }
    }
}
