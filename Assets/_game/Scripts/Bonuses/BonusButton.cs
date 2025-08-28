using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BonusButton : MonoBehaviour
{
    [SerializeField] private Image _bonusImage;
    [SerializeField] protected TextMeshProUGUI _countBonusText;
    [SerializeField] protected Button _activateBonus;

    protected int _countBonus;
    protected int _id;
    protected SignalBus _signalBus;

    public virtual void Init(int id, Sprite sprite)
    {
        _signalBus = ServiceLocator.Current.Get<SignalBus>();
        _signalBus.Subscirbe<ChangeBonusCountSignal>(ChangedCount);
        _id = id;
        _bonusImage.sprite = sprite;
        _countBonus = PlayerPrefs.GetInt(StringConstant.BONUS_COUNT +  _id, 0);
        _countBonusText.text = $"x{_countBonus}";
        _activateBonus.onClick.AddListener(TryActivationBonus);
    }
    private void ChangedCount(ChangeBonusCountSignal signal)
    {
        _countBonus = PlayerPrefs.GetInt(StringConstant.BONUS_COUNT + _id, 0);
        _countBonusText.text = $"x{_countBonus}";
    }
    private void TryActivationBonus()
    {
        if (_countBonus > 0)
        {
            _countBonus--;
            PlayerPrefs.SetInt(StringConstant.BONUS_COUNT + _id, _countBonus);
            _countBonusText.text = $"x{_countBonus}";
            BonusActivate();
        }
        else
        {
            _countBonus = 0;
            PlayerPrefs.SetInt(StringConstant.BONUS_COUNT + _id, _countBonus);
            PlayerPrefs.SetInt(StringConstant.CURRENT_BONUS, _id);
            DialogManager.ShowDialog<BuyBonusDialog>();
            _signalBus.Invoke(new StopGameSignal());
        }
    }
    protected abstract void BonusActivate();
}
