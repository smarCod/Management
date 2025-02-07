


using Management.Core.Models.DepartmentModels.DepartmentMediator.Responses;

using MediatR;

namespace Management.Core.Models.DepartmentModels.DepartmentMediator.Queries;


public record GetDepartmentByIdQuery(int Id) : IRequest<DepartmentQueryResponse>;


// public class GetDepartmentByIdQuery : IRequest<DepartmentQueryResponse>
// {
//     public int Id { get; set; }
// }
