using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class MenuDialog : Dialog
{
    [SerializeField] private Button _choiceDifficultButton;
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _swapLanguageButton;
    [SerializeField] private TextMeshProUGUI _languageName;

    private void Awake()
    {
        _choiceDifficultButton.onClick.AddListener(OnChoiceButtonClick);
        _shopButton.onClick.AddListener(OnShopButtonClick);
        _swapLanguageButton.onClick.AddListener(SwapLanguage);
        if (YG2.lang == "ru")
        {
            _languageName.text = "рус";
        }
        else
        {
            _languageName.text = "en";
        }
    }

    private void SwapLanguage()
    {
        if (YG2.lang == "ru")
        {
            YG2.SwitchLanguage("en");
            _languageName.text = "en";
        }
        else
        {
            YG2.SwitchLanguage("ru");
            _languageName.text = "рус";
        }
    }

    private void OnChoiceButtonClick()
    {
        DialogManager.ShowDialog<SelectDifficultDialog>();
        Hide();
    }
    private void OnShopButtonClick()
    {
        DialogManager.ShowDialog<ShopDialog>();
        Hide();
    }
    private void OnDestroy()
    {
        if (_choiceDifficultButton != null)
        {
            _choiceDifficultButton.onClick.RemoveAllListeners();
        }
        if (_shopButton != null)
        {
            _shopButton.onClick.RemoveAllListeners();
        }
    }
}
