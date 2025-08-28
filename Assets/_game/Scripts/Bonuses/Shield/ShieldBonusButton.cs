using UnityEngine;

public class ShieldBonusButton : BonusButton
{
    private bool _isActive = false;

    public override void Init(int id, Sprite sprite)
    {
        base.Init(id, sprite);
        _signalBus.Subscirbe<BreakShieldSignal>(ShieldBreak);
    }

    protected override void BonusActivate()
    {
        if (!_isActive)
        {
            _signalBus.Invoke(new ShieldBonusSignal());
            _activateBonus.interactable = false;
        }
        _isActive = true;
    }
    private void ShieldBreak(BreakShieldSignal signal)
    {
        _isActive = false;
        _activateBonus.interactable = true;
    }
    private void OnDestroy()
    {
        _signalBus.Unsubscribe<BreakShieldSignal>(ShieldBreak);
    }
}
