using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using YG.LanguageLegacy;

public class HUD : MonoBehaviour, IService
{
    [SerializeField] private TextMeshProUGUI _textTimer;
    [SerializeField] private LangYGAdditionalText _scoreValue;
    [SerializeField] private LangYGAdditionalText _streakValue;
    [SerializeField] private TextMeshProUGUI _textStreak;
    [SerializeField] private Button _openPauseMenuButton;


    private Color _baseColor;
    private SignalBus _signalBus;

    public void Init()
    {
        _signalBus = ServiceLocator.Current.Get<SignalBus>();
        _signalBus.Subscirbe<TimerChangeSignal>(UpdateTimerDisplay);
        _signalBus.Subscirbe<ScoreChangeSignal>(UpdateScoreDisplay);
        _signalBus.Subscirbe<StreakChangeSignal>(UpdateStreakDisplay);
        _openPauseMenuButton.onClick.AddListener(OpenPauseMenu);

        _baseColor = _textStreak.color;
    }

    private void UpdateStreakDisplay(StreakChangeSignal signal)
    {
        _streakValue.additionalText = " " + signal.Value;
        StreakColorChanger((signal.Value == 0 ? Color.red : Color.green), signal.Value);
    }

    private void UpdateScoreDisplay(ScoreChangeSignal signal)
    {
        _scoreValue.additionalText = " " + signal.Score;
    }

    private void UpdateTimerDisplay(TimerChangeSignal signal)
    {
        var time = signal.Time;
        int min = (int)(time / 60);
        int sec = (int)(time % 60);
        int millisec = (int)(time * 100 % 100);

        _textTimer.text = string.Format("{0:00}:{1:00}:{2:00}", min, sec, millisec);
    }

    private void OpenPauseMenu()
    {
        _signalBus.Invoke(new StopGameSignal());
        DialogManager.ShowDialog<PauseMenuDialog>();
    }

    private async void StreakColorChanger(Color color, int value)
    {
        Color originalColor = ColorCalibration(Mathf.Min(value, 5));
        _textStreak.color = color;
        float time = 0;
        while (time < 1f)
        {
            time += Time.deltaTime;
            _textStreak.color = Color.Lerp(color, originalColor, time / 1f);
            await UniTask.Yield();
        }
    }

    private Color ColorCalibration(int value)
    {
        if (value == 0) return _baseColor;
        float greenAmount = (float)(value + 1) / 5;
        return new Color(
            _baseColor.r * (1 - greenAmount),
            _baseColor.g + (1 - _baseColor.g) * greenAmount,
            _baseColor.b * (1 - greenAmount));

    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<TimerChangeSignal>(UpdateTimerDisplay);
        _signalBus.Unsubscribe<ScoreChangeSignal>(UpdateScoreDisplay);
    }
}
