using UnityEngine;

[System.Serializable]
public class BonusData
{
    [SerializeField] private int _id;

    [SerializeField] private string _name;

    [SerializeField] private Sprite _bonusLogo;

    [SerializeField] private BonusButton _bonusButtonPrefab;

    [SerializeField] private int _price;

    public int Id => _id;
    public string Name => _name;
    public Sprite BonusLogo => _bonusLogo;
    public BonusButton BonusPrefab => _bonusButtonPrefab;
    public int Price => _price;
}
