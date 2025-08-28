using UnityEngine;
using UnityEngine.UI;

public class BuyGoldDialog : Dialog
{
    [SerializeField] private Button _exitButton;
    [SerializeField] private LayoutGroup _slotHolder;
    [SerializeField] private BuyGoldSlot _slotPrefab;

    private void Awake()
    {
        _exitButton.onClick.AddListener(() => Hide());
    }
}
