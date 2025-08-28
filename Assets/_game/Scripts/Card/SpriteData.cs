using UnityEngine;

[System.Serializable]
public class SpriteData
{
    [SerializeField] private int _id;
    [SerializeField] private Sprite _suitSprite;

    public int Id => _id;
    public Sprite SuitSprite => _suitSprite;
}
