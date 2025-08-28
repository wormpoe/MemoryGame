using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SuitsConfig", menuName = "GameConfigs/SuitsConfig")]
public class SuitsConfig : ScriptableObject
{
    [SerializeField] private List<SuitData> _suitData;

    public List<SuitData> SuitData => _suitData;
}
