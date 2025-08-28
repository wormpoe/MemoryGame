using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager
{
    private const string PrefabsFilePath = "Dialogs/";
    private static readonly Dictionary<Type, string> PrefabsDictionary = new Dictionary<Type, string>()
    {
        {typeof(MenuDialog), "MenuDialog" },
        {typeof(SelectDifficultDialog), "ChoiceDifficultyDialog" },
        {typeof(ShopDialog), "ShopDialog" },
        {typeof(LoadingDialog), "LoadingDialog" },
        {typeof(PauseMenuDialog), "PauseMenuDialog" },
        {typeof(BuyBonusDialog), "BuyBonusDialog" },
        {typeof(WinDialog), "WinDialog" },
    };

    public static T ShowDialog<T>() where T : Dialog
    {
        var go = GetPrefabByType<T>();
        if (go == null)
        {
            Debug.LogError("Show window - object not found");
            return null;
        }
        return GameObject.Instantiate(go, GuiHolder); ;
    }

    private static T GetPrefabByType<T>() where T : Dialog
    {
        var prefabName = PrefabsDictionary[typeof(T)];
        if (string.IsNullOrEmpty(prefabName))
        {
            Debug.LogError("cant find prefab / add " + typeof(T));
        }

        var path = PrefabsFilePath + PrefabsDictionary[typeof(T)];
        var dialog = Resources.Load<T>(path);
        if (dialog == null)
        {
            Debug.LogError("cant find prefab");
        }
        return dialog;
    }

    public static Transform GuiHolder
    {
        get { return ServiceLocator.Current.Get<GUIHolder>().transform; }
    }
}
