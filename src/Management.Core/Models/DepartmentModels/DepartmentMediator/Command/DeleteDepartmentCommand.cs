


using Management.Core.Models.DepartmentModels.DepartmentMediator.Responses;

using MediatR;

namespace Management.Core.Models.DepartmentModels.DepartmentMediator.Command;

public record DeleteDepartmentCommand(int Id) : IRequest<DepartmentCommandResponse>;
