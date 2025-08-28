using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class FindPairBonusButton : BonusButton
{
    [SerializeField] private float _duration;
    [SerializeField] private TextMeshProUGUI _notification;
    public override void Init(int id, Sprite sprite)
    {
        base.Init(id, sprite);
        _signalBus.Subscirbe<NoOpenCardSignal>(NotChangeCount);

    }

    private void NotChangeCount(NoOpenCardSignal signal)
    {
        _countBonus++;
        PlayerPrefs.SetInt(StringConstant.BONUS_COUNT + _id, _countBonus);
        _countBonusText.text = $"x{_countBonus}";
        Notify();
    }
    private async void Notify()
    {
        Color textColor = _notification.color;
        textColor.a = 1f;
        _notification.color = textColor;
        _notification.text = $"No open card";
        var elipsedTime = 0f;
        while (elipsedTime < .5f)
        {
            elipsedTime += Time.deltaTime;
            textColor.a = Mathf.Lerp(1, 0, elipsedTime / 2f);
            _notification.color = textColor;
            await UniTask.Yield();

        }
    }
    protected override void BonusActivate()
    {
        _signalBus.Invoke(new FindPairBonusSignal(_duration));
    }
    private void OnDestroy()
    {
        _signalBus.Unsubscribe<NoOpenCardSignal>(NotChangeCount);
    }
}
