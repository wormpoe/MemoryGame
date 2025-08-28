using System.Collections.Generic;

public interface IDifficultLoader : ILoader, IService
{
    public IEnumerable<DifficultData> GetDifficultData();
}
