using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using YG.LanguageLegacy;
using UnityEngine.SceneManagement;

public class BuyBonusDialog : Dialog
{
    [SerializeField] private TextMeshProUGUI _bonusName;
    [SerializeField] private Button _closeDialogButton;
    [SerializeField] private Button _buyBonusButton;
    [SerializeField] private Button _plusButtom;
    [SerializeField] private Button _minusButtom;
    [SerializeField] private Button _maxButtom;
    [SerializeField] private TMP_InputField _countBonusBuyText;
    [SerializeField] private LangYGAdditionalText _goldValueText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _notification;

    private string _baseText;

    private int _id = 0;
    private int _countBonusBuy;
    private int _price;

    private int CurrentPrice => _price * _countBonusBuy;

    private IBonusLoader _bonusLoader;
    private SignalBus _signalBus;
    private GoldController _goldController;

    private void Awake()
    {
        _countBonusBuy = 1;
        _countBonusBuyText.text = _countBonusBuy.ToString();
        _closeDialogButton.onClick.AddListener(CloseDialog);
        _plusButtom.onClick.AddListener(PlusCount);
        _minusButtom.onClick.AddListener(MinusCount);
        _maxButtom.onClick.AddListener(MaxCount);
        _buyBonusButton.onClick.AddListener(TryBuy);
        _countBonusBuyText.onEndEdit.AddListener(WriteCount);
        _id = PlayerPrefs.GetInt(StringConstant.CURRENT_BONUS, 0);

        _bonusLoader = ServiceLocator.Current.Get<IBonusLoader>();
        var bonusData = _bonusLoader.GetBonusData().ToList()[_id];
        if (bonusData == null)
        {
            Debug.LogError("");
            return;
        }
        _bonusName.text = $"Buy bonus \n {bonusData.Name}";
        _price = bonusData.Price;
        _signalBus = ServiceLocator.Current.Get<SignalBus>();
        _signalBus.Subscirbe<GoldChangeSignal>(ChangeGoldText);
        _goldController = ServiceLocator.Current.Get<GoldController>();
        if (SceneManager.GetActiveScene().name == StringConstant.MAIN_SCENE_NAME)
        {
            _goldValueText.gameObject.SetActive(true);
        }
        _goldValueText.additionalText = " " + _goldController.Gold.ToString();
        _priceText.text = "Price: " + (_countBonusBuy * _price).ToString();
    }

    private void ChangeGoldText(GoldChangeSignal signal)
    {
        _goldValueText.additionalText = " " + signal.Gold.ToString();
    }

    private void WriteCount(string arg0)
    {
        if(int.TryParse(arg0, out int result))
        {
            _countBonusBuy = result;
            _priceText.text = string.Format("Price: {0}", (_countBonusBuy * _price).ToString());
        }
        else
        {
            Debug.LogError("not numbers");
        }
    }

    private void PlusCount()
    {
        if (_countBonusBuy > 99) return;
        _countBonusBuy++;
        ChangeText();
    }
    private void MinusCount()
    {
        if (_countBonusBuy <= 1) return;
        _countBonusBuy--;
        ChangeText();
    }
    private void MaxCount()
    {
        if (_goldController.Gold > _price)
        {
            _countBonusBuy = _goldController.Gold / _price;
        }
        ChangeText();
    }

    private void TryBuy()
    {
        if (_goldController.HaveEnoughGold(CurrentPrice))
        {
            var bonusCount = PlayerPrefs.GetInt(StringConstant.BONUS_COUNT + _id, 0) + _countBonusBuy;
            PlayerPrefs.SetInt(StringConstant.BONUS_COUNT + _id, bonusCount);
            _signalBus.Invoke(new SpendGoldSignal(CurrentPrice));
            _signalBus.Invoke(new ChangeBonusCountSignal());
            Hide();
            _signalBus.Invoke(new StartGameSignal());
        }
        else
        {
            Notify();
        }
    }
    private void ChangeText()
    {
        _countBonusBuyText.Select();
        _countBonusBuyText.text = _countBonusBuy.ToString();
        _priceText.text = "Price: " + (_countBonusBuy * _price).ToString();
    }
    private void CloseDialog()
    {
        Hide();
        _signalBus.Invoke(new StartGameSignal());
    }
    private async void Notify()
    {
        if (_baseText == null)
        {
            _baseText = _notification.text;
        }
        Color textColor = _notification.color;
        textColor.a = 1f;
        _notification.color = textColor;
        _notification.text = string.Format(_baseText, CurrentPrice - _goldController.Gold);
        var elipsedTime = 0f;
        while (elipsedTime < .5f)
        {
            elipsedTime += Time.deltaTime;
            textColor.a = Mathf.Lerp(1, 0, elipsedTime / .5f);
            _notification.color = textColor;
            await UniTask.Yield();
        }
    }
    private void OnDestroy()
    {
        _closeDialogButton.onClick.RemoveAllListeners();
        _plusButtom.onClick.RemoveAllListeners();
        _minusButtom.onClick.RemoveAllListeners();
        _buyBonusButton.onClick.RemoveAllListeners();
        _countBonusBuyText.onEndEdit.RemoveAllListeners();
    }
}
