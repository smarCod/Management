



using Management.Core.Models.DepartmentModels.DepartmentMediator.Responses;

using MediatR;

namespace Management.Core.Models.DepartmentModels.DepartmentMediator.Command;


public record UpdateDepartmentCommand(int Id, string Name) : IRequest<DepartmentCommandResponse>;
// {
//     public int Id { get; set; }
//     public string Name { get; set; } = string.Empty;
// }