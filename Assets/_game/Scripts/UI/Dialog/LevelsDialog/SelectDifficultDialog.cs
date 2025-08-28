using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SelectDifficultDialog : Dialog
{
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _shopButton;
    [SerializeField] private GridLayoutGroup _layoutGroup;
    [SerializeField] private SelectDifficultSlot _difficultPrefab;

    private IDifficultLoader _difficultLoader;
    private void Awake()
    {
        _menuButton.onClick.AddListener(OnMenuButtonClick);
        _shopButton.onClick.AddListener(OnShopButtonClick);

        _difficultLoader = ServiceLocator.Current.Get<IDifficultLoader>();
        var difficults = _difficultLoader.GetDifficultData();
        difficults = difficults.OrderBy(x => x.Id);
        GenerateDifficult(difficults);
    }

    private void GenerateDifficult(IEnumerable<DifficultData> difficults)
    {
        foreach (var difficult in difficults)
        {
            var go = GameObject.Instantiate(_difficultPrefab, _layoutGroup.transform);
            go.Init(difficult);
        }
    }

    private void OnMenuButtonClick()
    {
        DialogManager.ShowDialog<MenuDialog>();
        Hide();
    }
    private void OnShopButtonClick()
    {
        DialogManager.ShowDialog<ShopDialog>();
        Hide();
    }
    private void OnDestroy()
    {
        if (_menuButton != null)
        {
            _menuButton.onClick.RemoveAllListeners();
        }
        if (_shopButton != null)
        {
            _shopButton.onClick.RemoveAllListeners();
        }
    }
}
