using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DifficultData
{
    [SerializeField] private int _id;
    [SerializeField] private int _column;
    [SerializeField] private int _row;
    [SerializeField] private int _suitCount;
    [SerializeField] private int _baseScore;
    [SerializeField] private int _winGold;
    [SerializeField] private float _minScoreRatio;
    [SerializeField] private float _maxScoreRatio;
    [SerializeField] private float _ratioStep;
    [SerializeField] private List<TimeScoreData> _timeScore;

    public int Id => _id;
    public int Column => _column;
    public int Row => _row;
    public int SuitCount => _suitCount;
    public int BaseScore => _baseScore;
    public int WinGold => _winGold;
    public float MinScoreRatio => _minScoreRatio;
    public float MaxScoreRatio => _maxScoreRatio;
    public float RatioStep => _ratioStep;
    public List<TimeScoreData> TimeScore => _timeScore;
}
