


using Management.Core.Models.DepartmentModels.DepartmentMediator.Responses;

using MediatR;

namespace Management.Core.Models.DepartmentModels.DepartmentMediator.Command;

public record PostDepartmentCommand(string Name) : IRequest<DepartmentCommandResponse>;


// public class PostDepartmentCommand : IRequest<DepartmentCommandResponse>
// {
//     public string Name { get; set; } = string.Empty;
// }
