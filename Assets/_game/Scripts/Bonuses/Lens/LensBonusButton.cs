using UnityEngine;
using Cysharp.Threading.Tasks;
public class LensBonusButton : BonusButton
{
    [SerializeField] private float _duration;

    protected override async void BonusActivate()
    {
        _activateBonus.interactable = false;
        _signalBus.Invoke(new LensBonusSignal(_duration));
        _signalBus.Invoke(new ChangeActiveLensSignal());
        await UniTask.WaitForSeconds(_duration);
        _signalBus.Invoke(new ChangeActiveLensSignal());
        _activateBonus.interactable = true;
    }
}
