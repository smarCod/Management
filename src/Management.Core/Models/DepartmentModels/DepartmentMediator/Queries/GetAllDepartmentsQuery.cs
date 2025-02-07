


using Management.Core.Models.DepartmentModels.DepartmentMediator.Responses;

using MediatR;

namespace Management.Core.Models.DepartmentModels.DepartmentMediator.Queries;


public class GetAllDepartmentsQuery : IRequest<IList<DepartmentQueryResponse>> {}
