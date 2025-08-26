using LMSConsoleApplication.Domain.Entities;
using LMSConsoleApplication.Services;

namespace LMSConsoleApplication.Domain.Specifications;

public class StudentSpecificationByNameOrderByByName:BaseSpecification<Student>
{
    public StudentSpecificationByNameOrderByByName(QueryParams queryParams):base(x=>string.IsNullOrWhiteSpace(queryParams.Search)||((x.FullName.FirstName.ToLower()+" "+x.FullName.LastName.ToLower()).Contains(queryParams.Search)||x.Email.Value.ToLower()== queryParams.Search))
    {
        ApplyPaging(queryParams.PageSize, queryParams.PageSize*(queryParams.PageNumber-1));
        AddOrderBy(x=>x.FullName.FirstName);
        
    }
}