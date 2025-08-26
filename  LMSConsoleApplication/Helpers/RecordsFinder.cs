using LMSConsoleApplication.Data;
using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Helpers;

public static class RecordsFinder
{

    public static TEntity? FindEntity<TEntity>(List<TEntity> set, Guid id) where TEntity : IEntity
    {
        return set.FirstOrDefault(x => x.Id == id);
    }
    
}