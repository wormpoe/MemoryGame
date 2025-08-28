using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusSlot : MonoBehaviour
{
    [SerializeField] private Button _buyBonusButton;
    [SerializeField] private Image _bonusLogo;
    [SerializeField] private TextMeshProUGUI _bonusCountText;
    private int _id;
    private SignalBus _signalBus;

    public void Init(int id, Sprite logo, SignalBus bus)
    {
        _id = id;
        _bonusLogo.sprite = logo;
        _buyBonusButton.onClick.AddListener(TryBuy);
        var count = PlayerPrefs.GetInt(StringConstant.BONUS_COUNT + _id, 0);
        _bonusCountText.text = count.ToString();
        _signalBus = bus;
        _signalBus.Subscirbe<ChangeBonusCountSignal>(RefreshBonusCount);
    }

    private void RefreshBonusCount(ChangeBonusCountSignal signal)
    {
        var count = PlayerPrefs.GetInt(StringConstant.BONUS_COUNT + _id, 0);
        _bonusCountText.text = count.ToString();
    }

    private void TryBuy()
    {
        PlayerPrefs.SetInt(StringConstant.CURRENT_BONUS, _id);
        DialogManager.ShowDialog<BuyBonusDialog>();
    }
}
