




using Management.Core.Models.DepartmentModels.DepartmentMediator.Command;
using Management.Core.Models.DepartmentModels.DepartmentMediator.Responses;
using Management.Core.Models.DepartmentModels;
using Management.Core.Interfaces.DepartmentInterfaces.RepositoryInterfaces;

using MediatR;

public class DepartmentCommandHandler :
    IRequestHandler<PostDepartmentCommand, DepartmentCommandResponse>,
    IRequestHandler<UpdateDepartmentCommand, DepartmentCommandResponse>,
    IRequestHandler<DeleteDepartmentCommand, DepartmentCommandResponse>
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentCommandHandler(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<DepartmentCommandResponse> Handle(PostDepartmentCommand request, CancellationToken cancellationToken)
    {
        if(request.Name == "")
        {
            return new DepartmentCommandResponse(ErrorMessage: "Leerer Departmentname");
        }
        DepartmentMod department = new DepartmentMod { Name = request.Name };
        await _departmentRepository.PostDepartmentAsyncRep(department);
        return new DepartmentCommandResponse(0, request.Name);
    }

    public async Task<DepartmentCommandResponse> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        if (request.Id <= 0)
        {
            return new DepartmentCommandResponse(ErrorMessage: "Ungültige ID");
        }

        if (string.IsNullOrEmpty(request.Name))
        {
            return new DepartmentCommandResponse(ErrorMessage: "Leerer Departmentname");
        }

        var departmentToUpdate = await _departmentRepository.GetDepartmentByIdAsyncRep(request.Id);
        if (departmentToUpdate.Data == null)
        {
            return new DepartmentCommandResponse(ErrorMessage: "Department nicht gefunden");
        }

        departmentToUpdate.Data.Name = request.Name;
        await _departmentRepository.UpdateDepartmentAsyncRep(departmentToUpdate.Data);
        return new DepartmentCommandResponse(departmentToUpdate.Data.Id, departmentToUpdate.Data.Name);
    }

    public async Task<DepartmentCommandResponse> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        if (request.Id <= 0)
        {
            return new DepartmentCommandResponse(ErrorMessage: "Zu kleine Id beim deleten");
        }
        var deleteResult = await _departmentRepository.DeleteDepartmentAsyncRep(request.Id);
        if (!deleteResult.Success)
        {
            return new DepartmentCommandResponse(ErrorMessage: "Fehler beim Löschen des Departments.");
        }
        return new DepartmentCommandResponse(request.Id);
    }
}
