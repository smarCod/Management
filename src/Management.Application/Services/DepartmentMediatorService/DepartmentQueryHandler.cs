

using Management.Core.Models.DepartmentModels.DepartmentMediator.Queries;
using Management.Core.Models.DepartmentModels.DepartmentMediator.Responses;
using Management.Core.Interfaces.DepartmentInterfaces.RepositoryInterfaces;

using MediatR;
using Microsoft.Extensions.Logging;

namespace Management.Application.Services.DepartmentMediatorService;

public class DepartmentQueryHandler :
    IRequestHandler<GetAllDepartmentsQuery, IList<DepartmentQueryResponse>>,
    IRequestHandler<GetDepartmentByIdQuery, DepartmentQueryResponse>
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILogger<DepartmentQueryHandler> _logger;

    public DepartmentQueryHandler(IDepartmentRepository departmentRepository, ILogger<DepartmentQueryHandler> logger)
    {
        _departmentRepository = departmentRepository;
        _logger = logger;
    }
    public async Task<IList<DepartmentQueryResponse>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
    {
        var departments = await _departmentRepository.GetAllDepartmentsAsyncRep();
        if (departments.Data != null && departments.Data.Any())
        {
            var result = departments.Data.Select(x => new DepartmentQueryResponse(x.Id, x.Name)).ToList();
            return result;
        }
        _logger.LogWarning("No departments found");

        // Rückgabe einer Liste mit einer speziellen Fehlermeldung
        return new List<DepartmentQueryResponse>
        {
            new DepartmentQueryResponse(ErrorMessage: "Keine Departments gefunden.")
        };
    }
    // public async Task<IList<DepartmentQueryResponse>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
    // {
    //     var departments = await _departmentRepository.GetAllDepartmentsAsyncRep();
    //     if (departments.Data != null && departments.Data.Any())
    //     {
    //         var result = departments.Data?.Select(x => new DepartmentQueryResponse(x.Id, x.Name)).ToList() ?? new List<DepartmentQueryResponse>();
    //         return result;
    //     }
    //     _logger.LogWarning("No departments found");

    //     throw new InvalidOperationException("Keine Departments gefunden.");
    // }
    public async Task<DepartmentQueryResponse> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        var department = await _departmentRepository.GetDepartmentByIdAsyncRep(request.Id);

        if (department.Success && department.Data != null)
        {
            _logger.LogInformation("Department found: {DepartmentName}", department.Data.Name);
            return new DepartmentQueryResponse(department.Data.Id, department.Data.Name);
        }
        _logger.LogWarning("Department not found for ID {Id}", request.Id);
        return new DepartmentQueryResponse(0, "", "Department nicht gefunden.");
    }
}