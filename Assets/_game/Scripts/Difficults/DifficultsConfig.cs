using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DifficultsConfig", menuName = "GameConfigs/DifficultsConfig")]
public class DifficultsConfig : ScriptableObject
{
    [SerializeField] private List<DifficultData> _difficults;

    public List<DifficultData> Difficults => _difficults;
}
