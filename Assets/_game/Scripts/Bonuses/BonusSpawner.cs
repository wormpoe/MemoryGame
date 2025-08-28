using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class BonusSpawner : MonoBehaviour, IService
{
    [SerializeField] private HorizontalLayoutGroup _holder;
    private IBonusLoader _bonusLoader;

    public void Init()
    {
        _bonusLoader = ServiceLocator.Current.Get<IBonusLoader>();
        OnInit();
    }

    private async void OnInit()
    {
        await UniTask.WaitUntil(_bonusLoader.IsLoaded);

        var bonusData = _bonusLoader.GetBonusData();
        if (bonusData == null)
        {
            Debug.LogError("");
            return;
        }
        
        foreach(var data in bonusData)
        {
            var go = GameObject.Instantiate(data.BonusPrefab, _holder.transform);
            go.Init(data.Id, data.BonusLogo);
        }
    }
}
