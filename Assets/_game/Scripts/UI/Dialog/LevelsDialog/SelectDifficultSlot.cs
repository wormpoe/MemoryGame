using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;
using YG.LanguageLegacy;

public class SelectDifficultSlot : MonoBehaviour
{
    [SerializeField] private Button _selectDifficultButton;
    [SerializeField] private TextMeshProUGUI _difficultName;
    [SerializeField] private LangYGAdditionalText _maxScoreValue;
    [SerializeField] private LangYGAdditionalText _minTimeValue;

    public void Init(DifficultData difficultData)
    {
        int id = difficultData.Id;
        _difficultName.text = DifficultName(id);
        _maxScoreValue.additionalText = " " + PlayerPrefs.GetInt(StringConstant.MAX_SCORE_DIFFICULT + id, 0).ToString();
        int time = PlayerPrefs.GetInt(StringConstant.MIN_TIME_DIFFICULT + id, 0);
        int min = time / 100 / 60;
        int sec = time / 100 % 60;
        int millisec = time % 100;
        _minTimeValue.additionalText = " " + string.Format("{0:00}:{1:00}:{2:00}", min, sec, millisec);
        _selectDifficultButton.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt(StringConstant.CURRENT_DIFFICULT, id);
            SceneManager.LoadScene(StringConstant.MAIN_SCENE_NAME);
        });
    }
    private string DifficultName(int id)
    {
        return YG2.lang == "ru" 
        ? id switch
        {
            0 => "Легко",
            1 => "Нормально",
            2 => "Сложно",
            _ => " "
        }
        : id switch
        {
            0 => "Easy",
            1 => "Normal",
            2 => "Hard",
            _ => " "
        };
    }
    private void OnDestroy()
    {
        _selectDifficultButton.onClick.RemoveAllListeners();
    }
}
