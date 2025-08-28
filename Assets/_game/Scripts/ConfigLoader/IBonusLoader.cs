using System.Collections.Generic;
public interface IBonusLoader : ILoader, IService
{
    public IEnumerable<BonusData> GetBonusData();
}
