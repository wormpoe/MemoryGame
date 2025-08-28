using UnityEngine;
using UnityEngine.SceneManagement;
using YG.LanguageLegacy;

public class LoadingDialog : Dialog
{
    [SerializeField] private LangYGAdditionalText _progressValue;

    private SignalBus _signalBus;

    private void Awake()
    {
        _signalBus = ServiceLocator.Current.Get<SignalBus>();

        _signalBus.Subscirbe<LoadProgressChangedSignal>(LoadProgressChanged);
        _signalBus.Subscirbe<AllDataLoadedSignal>(OnAdllResourcesLoaded);

        _progressValue.additionalText = " " + 0 + "%";
    }

    private void OnAdllResourcesLoaded(AllDataLoadedSignal signal)
    {
        Hide();
        if (SceneManager.GetActiveScene().name == StringConstant.MENU_SCENE_NAME)
        {
            DialogManager.ShowDialog<MenuDialog>();
        }
    }

    private void LoadProgressChanged(LoadProgressChangedSignal signal)
    {
        float progress = signal.Progress * 100;
        _progressValue.additionalText = " " + progress + "%";
    }
}
