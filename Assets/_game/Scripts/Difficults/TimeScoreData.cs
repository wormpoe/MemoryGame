using UnityEngine;

[System.Serializable]
public class TimeScoreData 
{
    [SerializeField] private float _requiredTime;
    [SerializeField] private float _timeRatio;
    [SerializeField] private int _timeBonusGold;

    public float RequiredTime => _requiredTime;
    public float TimeRatio => _timeRatio;
    public int TimeBonusGold => _timeBonusGold;
}
