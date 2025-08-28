using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG.LanguageLegacy;

public class WinDialog : Dialog
{
    [SerializeField] private LangYGAdditionalText _winGoldValue;
    [SerializeField] private LangYGAdditionalText _time;
    [SerializeField] private LangYGAdditionalText _score;
    [SerializeField] private GameObject _bestScore;
    [SerializeField] private GameObject _bestTime;

    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _goToMenuButton;
    private void Awake()
    {
        _bestScore.SetActive(false);
        _bestTime.SetActive(false);

        _restartButton.onClick.AddListener(() => SceneManager.LoadScene(StringConstant.MAIN_SCENE_NAME));
        _goToMenuButton.onClick.AddListener(() => SceneManager.LoadScene(StringConstant.MENU_SCENE_NAME));

    }
    public void Init(int gold, int score, float time, int id)
    {
        TimeChange(id, time);
        ScoreChange(id, score);
        var signnalBus = ServiceLocator.Current.Get<SignalBus>();
        _winGoldValue.additionalText = " " + gold + "!";
        signnalBus.Invoke(new GoldAddSignal(gold));
    }
    private void TimeChange(int id, float time)
    {
        int min = (int)(time / 60);
        int sec = (int)(time % 60);
        int millisec = (int)(time * 100 % 100);

        _time.additionalText = " " + string.Format("Time: {0:00}:{1:00}:{2:00}", min, sec, millisec);
        var bestTime = PlayerPrefs.GetInt(StringConstant.MIN_TIME_DIFFICULT + id, -1);
        time *= 100;
        if ((int)time < bestTime || bestTime < 0)
        {
            _bestTime.SetActive(true);
            PlayerPrefs.SetInt(StringConstant.MIN_TIME_DIFFICULT + id, (int)time);
        }

    }
    private void ScoreChange(int id, int score)
    {
        _score.additionalText = " " + score;
        var bestScore = PlayerPrefs.GetInt(StringConstant.MAX_SCORE_DIFFICULT + id, 0);
        if (score > bestScore)
        {
            _bestScore.SetActive(true);
            PlayerPrefs.SetInt(StringConstant.MAX_SCORE_DIFFICULT + id, score);
        }
    }
}
