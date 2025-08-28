using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuDialog : Dialog
{
    [SerializeField] private Button _goToMenuButton;
    [SerializeField] private Button _goBackButton;

    private SignalBus _signalBus;
    private void Awake()
    {
        _signalBus = ServiceLocator.Current.Get<SignalBus>();
        _goToMenuButton.onClick.AddListener(GoToMenu);
        _goBackButton.onClick.AddListener(GoBack);
    }

    private void GoBack()
    {
        _signalBus.Invoke(new StartGameSignal());
        Hide();
    }

    private void GoToMenu()
    {
        SceneManager.LoadScene(StringConstant.MENU_SCENE_NAME);
        Hide();
    }

}
