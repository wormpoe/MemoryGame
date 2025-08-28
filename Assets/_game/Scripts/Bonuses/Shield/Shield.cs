using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class Shield : MonoBehaviour, IService
{
    [SerializeField] private Image _shield;
    [SerializeField] Sprite _shieldSprite;
    [SerializeField] Sprite _brokeShieldSprite;

    private SignalBus _signalBus;
    public void Init()
    {
        _shield.gameObject.SetActive(false);
        _signalBus = ServiceLocator.Current.Get<SignalBus>();
        _signalBus.Subscirbe<ShieldBonusSignal>(ActivateShield);
        _signalBus.Subscirbe<BreakShieldSignal>(StartBreakAnimation);
    }
    private async void StartBreakAnimation(BreakShieldSignal signal)
    {
        _shield.sprite = _brokeShieldSprite;
        await UniTask.WaitForSeconds(.5f);
        _shield.gameObject.SetActive(false);
    }

    private void ActivateShield(ShieldBonusSignal signal)
    {
        _shield.gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        _signalBus.Unsubscribe<ShieldBonusSignal>(ActivateShield);
        _signalBus.Unsubscribe<BreakShieldSignal>(StartBreakAnimation);
    }
}
