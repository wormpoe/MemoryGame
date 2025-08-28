using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SuitData
{
    [SerializeField] private Sprite _suitLogo;
    [SerializeField] private List<SpriteData> _spriteData;
    public Sprite SuitLogo => _suitLogo;

    public List<SpriteData> Get(int count)
    {
        return _spriteData.OrderBy(x => Random.value).Take(count).ToList();
    }
}
