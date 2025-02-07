



using Management.Core.Models;
using Management.Core.Models.DepartmentModels;

namespace Management.Core.Interfaces.DepartmentInterfaces.RepositoryInterfaces;

public interface IDepartmentRepository
{
    Task<GetAllResult<IList<DepartmentMod>>> GetAllDepartmentsAsyncRep();
    Task<GetOneResult<DepartmentMod>> GetDepartmentByIdAsyncRep(int id);
    Task<PostResult<DepartmentMod>> PostDepartmentAsyncRep(DepartmentMod departmentMod);
    Task<PutResult<DepartmentMod>> UpdateDepartmentAsyncRep(DepartmentMod departmentMod);
    Task<DeleteResult<DepartmentMod>> DeleteDepartmentAsyncRep(int id);
}