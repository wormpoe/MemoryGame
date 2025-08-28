using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SuitSlot : MonoBehaviour
{
    [SerializeField] private Button _selectSuitButton;
    [SerializeField] private TextMeshProUGUI _suitName;
    [SerializeField] private GameObject _selectionFlag;
    [SerializeField] private Image _logo;

    private int _id;
    private SignalBus _signalBus;

    public void Init(int id, Sprite suitLogo, SignalBus bus)
    {
        _id = id;
        _logo.sprite = suitLogo;
        _signalBus = bus;
        _signalBus.Subscirbe<UnselectedSuitSignal>(UnselectSuit);
        _selectSuitButton.onClick.AddListener(SelectSuit);
        _selectionFlag.SetActive(PlayerPrefs.GetInt(StringConstant.SELECTED_SUIT, 0) == _id);
    }

    private void SelectSuit()
    {
        _signalBus.Invoke(new UnselectedSuitSignal());
        _selectionFlag.SetActive(true);
        PlayerPrefs.SetInt(StringConstant.SELECTED_SUIT, _id);
    }

    private void UnselectSuit(UnselectedSuitSignal signal)
    {
        _selectionFlag.SetActive(false);
    }
    private void OnDestroy()
    {
        _signalBus.Unsubscribe<UnselectedSuitSignal>(UnselectSuit);
        _selectSuitButton.onClick.RemoveAllListeners();
    }
}
