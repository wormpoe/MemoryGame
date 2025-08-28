using System.Collections.Generic;
public interface ISuitLoader : ILoader, IService
{
    public IEnumerable<SuitData> GetSuitData();
}
