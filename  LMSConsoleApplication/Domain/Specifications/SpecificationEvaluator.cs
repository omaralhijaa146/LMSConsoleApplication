using LMSConsoleApplication.Domain.Entities;

namespace LMSConsoleApplication.Domain.Specifications;

public static class SpecificationEvaluator<T> where T :IEntity
{

    public static IEnumerable<T> GetQuery(IEnumerable<T> inputQuery,ISpecification<T> specification)
    {
        var query = inputQuery;
        
        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria);
        }

        if (specification.OrderBy != null)
        {
            query = query.OrderBy(specification.OrderBy);
        }

        if (specification.OrderByDescending != null)
        {
            query= query.OrderByDescending(specification.OrderByDescending);
        }

        if (specification.IsPagingEnabled)
        {
            query = query.Skip(specification.Skip).Take(specification.Take);
        }
        
        return query;
    }
    
}