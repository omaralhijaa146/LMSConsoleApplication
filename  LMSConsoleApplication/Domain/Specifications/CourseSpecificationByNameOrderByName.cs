using LMSConsoleApplication.Domain.Entities;
using LMSConsoleApplication.Services;

namespace LMSConsoleApplication.Domain.Specifications;

public class CourseSpecificationByNameOrderByName : BaseSpecification<Course>
{
    public CourseSpecificationByNameOrderByName(QueryParams queryParams):base(
        x=>string.IsNullOrWhiteSpace(queryParams.Search)||x.Name.Contains(queryParams.Search)
    )
    {
        ApplyPaging(queryParams.PageSize, queryParams.PageSize*(queryParams.PageNumber-1));
        AddOrderBy(x=>x.Name);
    }
    
}