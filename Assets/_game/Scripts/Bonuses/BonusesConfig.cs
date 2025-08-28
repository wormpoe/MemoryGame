using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BonusesConfig", menuName = "GameConfigs/BonusesConfig")]
public class BonusesConfig : ScriptableObject
{
    [SerializeField] private List<BonusData> _bonusData;

    public List<BonusData> BonusData => _bonusData;
}
