using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG.LanguageLegacy;

public class ShopDialog : Dialog
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private HorizontalLayoutGroup _suitGroup;
    [SerializeField] private SuitSlot _suitSlotPrefab;
    [SerializeField] private HorizontalLayoutGroup _bonusGroup;
    [SerializeField] private BonusSlot _bonusSlotPrefab;
    [SerializeField] private LangYGAdditionalText _goldValue;
    // test button
    [SerializeField] private Button _giveGoldButton;
    [SerializeField] private Button _restartGameButtom;

    private SignalBus _signalBus;
    private GoldController _goldController;

    private void Awake()
    {
        _closeButton.onClick.AddListener(OnCloseButton);
        _giveGoldButton.onClick.AddListener(OnGiveGold);
        _restartGameButtom.onClick.AddListener(RestartGame);
        _signalBus = ServiceLocator.Current.Get<SignalBus>();
        _signalBus.Subscirbe<GoldChangeSignal>(ChangedGold);
        _goldController = ServiceLocator.Current.Get<GoldController>();

        _goldValue.additionalText = " " + _goldController.Gold;

        InitSuitSlots();
        InitBonusSlot();
    }

    private void RestartGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(StringConstant.MENU_SCENE_NAME);
    }

    private void InitSuitSlots()
    {
        var suitLoader = ServiceLocator.Current.Get<ISuitLoader>();
        var suitsData = suitLoader.GetSuitData();
        int id = 0;
        foreach(var suitData in suitsData)
        {
           var item = GameObject.Instantiate(_suitSlotPrefab, _suitGroup.transform);
            item.Init(id, suitData.SuitLogo, _signalBus);
            id++;
        }
    }
    private void InitBonusSlot()
    {
        var bonusLoader = ServiceLocator.Current.Get<IBonusLoader>();
        var bonusesData = bonusLoader.GetBonusData();
        int id = 0;
        foreach(var bonusData in bonusesData)
        {
            var go = GameObject.Instantiate(_bonusSlotPrefab, _bonusGroup.transform);
            go.Init(id, bonusData.BonusLogo, _signalBus);
            id++;
        }
    }
    private void OnCloseButton()
    {
        DialogManager.ShowDialog<MenuDialog>();
        Hide();
    }
    private void ChangedGold(GoldChangeSignal signal)
    {
        _goldValue.additionalText = " " + _goldController.Gold;
    }
    private void OnGiveGold()
    {
        _signalBus.Invoke(new GoldAddSignal(200));
    }
    private void OnDestroy()
    {
        _closeButton.onClick?.RemoveAllListeners();
        _giveGoldButton.onClick?.RemoveAllListeners();
        _restartGameButtom.onClick?.RemoveAllListeners();
    }
}
