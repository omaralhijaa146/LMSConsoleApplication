using LMSConsoleApplication.Domain.Entities;
using LMSConsoleApplication.Services;

namespace LMSConsoleApplication.Domain.Specifications;

public class TrainerSpecificationByNameOrderByName:BaseSpecification<Trainer>
{
    public TrainerSpecificationByNameOrderByName(QueryParams queryParams):base(
        x=>string.IsNullOrWhiteSpace(queryParams.Search)||((x.FullName.FirstName.ToLower()+" "+x.FullName.LastName.ToLower()).Contains(queryParams.Search)||x.Email.Value.ToLower()== queryParams.Search)
    )
    {
        ApplyPaging(queryParams.PageSize, queryParams.PageSize*(queryParams.PageNumber-1));
        AddOrderBy(x=>x.FullName.FirstName);
    }
}