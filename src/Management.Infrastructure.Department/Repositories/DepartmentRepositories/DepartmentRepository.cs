



using Management.Infrastructure.Department.Data;
using Management.Core.Interfaces.DepartmentInterfaces.RepositoryInterfaces;
using Management.Core.Models;
using Management.Core.Models.DepartmentModels;

using Microsoft.EntityFrameworkCore;
using System.Net;   //  HttpStatusCode

namespace Management.Infrastructure.Department.Repositories.DepartmentRepositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly AppDbContextDepartment _appDbContextDepartment;

    public DepartmentRepository(AppDbContextDepartment appDbContextDepartment)
    {
        _appDbContextDepartment = appDbContextDepartment;
    }

    //  Get All
    //  Korrekte Liste returnen
    //  Null-Prüfung / Leere Liste: Überprüfe, ob die Ergebnisliste null ist.
    public async Task<GetAllResult<IList<DepartmentMod>>> GetAllDepartmentsAsyncRep()
    {
        try
        {
            IList<DepartmentMod> result = await _appDbContextDepartment.PostDepartment.ToListAsync();

            // Überprüfung auf leere Liste
            if (result.Count == 0 || result == null)
            {
                return new GetAllResult<IList<DepartmentMod>>
                {
                    Success = false,
                    Data = new List<DepartmentMod>(),
                    StatusCode = (int)HttpStatusCode.NotFound,
                    CreatedAt = DateTime.Now,
                    Message = "Keine Departments gefunden."
                };
            }

            // Erfolgreiches Ergebnis zurückgeben
            return new GetAllResult<IList<DepartmentMod>>
            {
                Success = true,
                Data = result,
                StatusCode = (int)HttpStatusCode.OK,
                CreatedAt = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"----> Exception caught: {ex.Message}");
            return new GetAllResult<IList<DepartmentMod>>
            {
                Success = false,
                Data = null,
                StatusCode = (int)HttpStatusCode.InternalServerError,
                CreatedAt = DateTime.Now,
                Message = $"Fehler: {ex.Message}"
            };
        }
    }

    // //  Get By Id
    // //  Prüfung auf existierenden Eintrag: Überprüfe, ob ein Eintrag mit der angegebenen ID existiert.
    // //  Null-Prüfung: Überprüfe, ob das Ergebnis null ist.
    public async Task<GetOneResult<DepartmentMod>> GetDepartmentByIdAsyncRep(int id)
    {
        var department = await _appDbContextDepartment.PostDepartment.FindAsync(id);
        if (department == null)
            return new GetOneResult<DepartmentMod>
            {
                Success = false,
                Data = null,
                StatusCode = (int)HttpStatusCode.NotFound,
                CreatedAt = DateTime.Now,
                Message = ""
            };

        return new GetOneResult<DepartmentMod>
        {
            Success = true,
            Data = department,
            StatusCode = (int)HttpStatusCode.OK,
            CreatedAt = DateTime.Now,
            Message = ""
        };
    }

    //  Post
    //  Null-Prüfung: Überprüfe, ob das übergebene Objekt null ist.
    //  Validierung: Überprüfe, ob das übergebene Objekt alle erforderlichen Felder enthält.
    public async Task<PostResult<DepartmentMod>> PostDepartmentAsyncRep(DepartmentMod? departmentMod)
    {
        if (departmentMod == null)
            return new PostResult<DepartmentMod>
            {
                Success = false,
                Data = null,
                StatusCode = (int)HttpStatusCode.BadRequest,
                CreatedAt = DateTime.Now,
                Message = "DepartmentMod cannot be null"
            };

        await _appDbContextDepartment.PostDepartment.AddAsync(departmentMod);
        await _appDbContextDepartment.SaveChangesAsync();

        return new PostResult<DepartmentMod>
        {
            Success = true,
            Data = departmentMod,
            StatusCode = (int)HttpStatusCode.Created,
            CreatedAt = DateTime.Now,
            Message = ""
        };
    }

    //  Update
    //  Erfolgreiche Aktualisierung: Teste, ob ein vorhandenes Department erfolgreich aktualisiert wird.
    //  Null-Parameter: Teste, ob die Methode korrekt damit umgeht, wenn departmentMod null ist und eine BadRequest-Antwort zurückgibt.
    //  Nicht gefunden (Existenzprüfung): Teste, ob ein nicht vorhandenes Department ordnungsgemäß behandelt wird und eine NotFound-Antwort zurückgibt.
    public async Task<PutResult<DepartmentMod>> UpdateDepartmentAsyncRep(DepartmentMod? departmentMod)
    {
        if (departmentMod == null)
            return new PutResult<DepartmentMod>
            {
                Success = false,
                Data = null,
                StatusCode = (int)HttpStatusCode.BadRequest,
                CreatedAt = DateTime.Now,
                Message = "DepartmentMod cannot be null"
            };

        var existingDepartment = await _appDbContextDepartment.PostDepartment.FindAsync(departmentMod.Id);
        if (existingDepartment == null)
            return new PutResult<DepartmentMod>
            {
                Success = false,
                Data = null,
                StatusCode = (int)HttpStatusCode.NotFound,
                CreatedAt = DateTime.Now,
                Message = "DepartmentMod not found"
            };

        _appDbContextDepartment.Entry(existingDepartment).CurrentValues.SetValues(departmentMod);
        await _appDbContextDepartment.SaveChangesAsync();

        return new PutResult<DepartmentMod>
        {
            Success = true,
            Data = departmentMod,
            StatusCode = (int)HttpStatusCode.OK,
            CreatedAt = DateTime.Now,
            Message = "Erfolgreiches Updaten"
        };
    }

    //  Delete
    //  Existenz-Prüfung: Überprüfe, ob das zu löschende Objekt existiert.
    //  Null-Prüfung: Überprüfe, ob die ID null oder ungültig ist.
    public async Task<DeleteResult<DepartmentMod>> DeleteDepartmentAsyncRep(int id)
    {
        var department = await _appDbContextDepartment.PostDepartment.FindAsync(id);
        if (department == null)
            return new DeleteResult<DepartmentMod>
            {
                Success = false,
                Data = null,
                StatusCode = (int)HttpStatusCode.NotFound,
                CreatedAt = DateTime.Now,
                Message = "DepartmentMod not found"
            };

        _appDbContextDepartment.PostDepartment.Remove(department);
        await _appDbContextDepartment.SaveChangesAsync();

        return new DeleteResult<DepartmentMod>
        {
            Success = true,
            Data = department,
            StatusCode = (int)HttpStatusCode.OK,
            CreatedAt = DateTime.Now,
            Message = "Erfolgreich deleted"
        };
    }

    // public async Task<DeleteResult<int>> DeleteDepartmentAsyncRep(int id)
    // {
    //     var result = _appDbContextDepartment.PostDepartment.Find(id);
    //     if(result != null)
    //     {
    //         _appDbContextDepartment.PostDepartment.Remove(result);
    //         await _appDbContextDepartment.SaveChangesAsync();
    //         return new DeleteResult<int> { Success = true };
    //     }
    //     else
    //     {
    //         return new DeleteResult<int> { Success = false };
    //     }
    // }
}



    // public async Task<GetOneResult<DepartmentMod>> GetDepartmentByIdAsyncRep(int id)
    // {
    //     DepartmentMod? result = await _appDbContextDepartment.PostDepartment.FindAsync(id);
    //     if(result != null)
    //         return new GetOneResult<DepartmentMod> { Success = true, Data = result };
    //     else
    //         return new GetOneResult<DepartmentMod> { Success = false, Message = "Department nicht gefunden." };
    // }
    // public async Task<PostResult<DepartmentMod>> PostDepartmentAsyncRep(DepartmentMod department)
    // {
    //     try
    //     {
    //         _appDbContextDepartment.PostDepartment.Add(department);
    //         await _appDbContextDepartment.SaveChangesAsync();
    //         return new PostResult<DepartmentMod> { Success = true, Data = department};
    //     }
    //     catch (DbUpdateException ex)
    //     {
    //         return new PostResult<DepartmentMod> { Success = false, Message = ex.Message };
    //     }
    //     catch(Exception ex)
    //     {
    //         return new PostResult<DepartmentMod> { Success = false, Message = ex.Message };
    //     }
    // }


    //  Get All
    //  Null-Prüfung: Überprüfe, ob die Ergebnisliste null ist.
    //  Leere Liste: Überprüfe, ob die Ergebnisliste leer ist.
    //  Dateninhalt: Stelle sicher, dass die zurückgegebenen Daten die erwarteten Eigenschaften aufweisen.
    // public async Task<GetAllResult<IList<DepartmentMod>>> GetAllDepartmentsAsyncRep()
    // {
    //     try
    //     {
    //         var result = await _appDbContextDepartment.PostDepartment.ToListAsync();
            
    //         if (result == null || result.Count == 0)
    //         {
    //             return new GetAllResult<IList<DepartmentMod>>
    //             {
    //                 Success = false,
    //                 Data = new List<DepartmentMod>(),
    //                 StatusCode = (int)HttpStatusCode.NotFound,
    //                 CreatedAt = DateTime.Now,
    //                 Message = "Keine Departments gefunden."
    //             };
    //         }

    //         return new GetAllResult<IList<DepartmentMod>>
    //         {
    //             Success = true,
    //             Data = result,
    //             StatusCode = (int)HttpStatusCode.OK,
    //             CreatedAt = DateTime.Now
    //         };
    //     }
    //     catch (Exception ex)
    //     {
    //         return new GetAllResult<IList<DepartmentMod>>
    //         {
    //             Success = false,
    //             Data = null,
    //             StatusCode = (int)HttpStatusCode.InternalServerError,
    //             CreatedAt = DateTime.Now,
    //             Message = $"Fehler: {ex.Message}"
    //         };
    //     }
    // }

    // //  Get By Id
    // //  Prüfung auf existierenden Eintrag: Überprüfe, ob ein Eintrag mit der angegebenen ID existiert.
    // //  Null-Prüfung: Überprüfe, ob das Ergebnis null ist.
    // public async Task<GetOneResult<DepartmentMod>> GetDepartmentByIdAsyncRep(int id)
    // {
    //     var department = await _appDbContextDepartment.PostDepartment.FindAsync(id);
    //     if (department == null)
    //         return new GetOneResult<DepartmentMod>
    //         {
    //             Success = false,
    //             Data = null,
    //             StatusCode = (int)HttpStatusCode.NotFound,
    //             CreatedAt = DateTime.Now
    //         };

    //     return new GetOneResult<DepartmentMod>
    //     {
    //         Success = true,
    //         Data = department,
    //         StatusCode = (int)HttpStatusCode.OK,
    //         CreatedAt = DateTime.Now
    //     };
    // }

    // //  Post
    // //  Null-Prüfung: Überprüfe, ob das übergebene Objekt null ist.
    // //  Validierung: Überprüfe, ob das übergebene Objekt alle erforderlichen Felder enthält.
    // public async Task<PostResult<DepartmentMod>> PostDepartmentAsyncRep(DepartmentMod departmentMod)
    // {
    //     if (departmentMod == null)
    //         return new PostResult<DepartmentMod>
    //         {
    //             Success = false,
    //             Data = null,
    //             StatusCode = (int)HttpStatusCode.BadRequest,
    //             CreatedAt = DateTime.Now,
    //             Message = "DepartmentMod cannot be null"
    //         };

    //     await _appDbContextDepartment.PostDepartment.AddAsync(departmentMod);
    //     await _appDbContextDepartment.SaveChangesAsync();

    //     return new PostResult<DepartmentMod>
    //     {
    //         Success = true,
    //         Data = departmentMod,
    //         StatusCode = (int)HttpStatusCode.Created,
    //         CreatedAt = DateTime.Now
    //     };
    // }
    // public async Task<PutResult<DepartmentMod>> UpdateDepartmentAsyncRep(DepartmentMod department)
    // {
    //     try
    //     {
    //         var existingDepartment = await _appDbContextDepartment.PostDepartment.FindAsync(department.Id);
            
    //         if (existingDepartment == null)
    //         {
    //             throw new InvalidOperationException("Das angegebene Department-ID wurde nicht gefunden.");
    //         }
            
    //         _appDbContextDepartment.Entry(existingDepartment).CurrentValues.SetValues(department);
    //         await _appDbContextDepartment.SaveChangesAsync();
            
    //         return new PutResult<DepartmentMod> { Success = true, Data = department };
    //     }
    //     catch (DbUpdateException ex)
    //     {
    //         return new PutResult<DepartmentMod> { Success = false, Message = ex.Message };
    //     }
    //     catch (InvalidOperationException ex)
    //     {
    //         return new PutResult<DepartmentMod> { Success = false, Message = ex.Message, CreatedAt = DateTime.Now };
    //     }
    //     catch(Exception ex)
    //     {
    //         return new PutResult<DepartmentMod> { Success = false, Message = ex.Message };
    //     }
    // }

    // //  Update
    // //  Existenz-Prüfung: Überprüfe, ob das zu aktualisierende Objekt existiert.
    // //  Null-Prüfung: Überprüfe, ob das übergebene Objekt null ist.
    // //  Validierung: Überprüfe, ob das übergebene Objekt alle erforderlichen Felder enthält.
    // public async Task<PutResult<DepartmentMod>> UpdateDepartmentAsyncRep(DepartmentMod departmentMod)
    // {
    //     if (departmentMod == null)
    //         return new PutResult<DepartmentMod>
    //         {
    //             Success = false,
    //             Data = null,
    //             StatusCode = (int)HttpStatusCode.BadRequest,
    //             CreatedAt = DateTime.Now,
    //             Message = "DepartmentMod cannot be null"
    //         };

    //     var existingDepartment = await _appDbContextDepartment.PostDepartment.FindAsync(departmentMod.Id);
    //     if (existingDepartment == null)
    //         return new PutResult<DepartmentMod>
    //         {
    //             Success = false,
    //             Data = null,
    //             StatusCode = (int)HttpStatusCode.NotFound,
    //             CreatedAt = DateTime.Now,
    //             Message = "DepartmentMod not found"
    //         };

    //     _appDbContextDepartment.Entry(existingDepartment).CurrentValues.SetValues(departmentMod);
    //     await _appDbContextDepartment.SaveChangesAsync();

    //     return new PutResult<DepartmentMod>
    //     {
    //         Success = true,
    //         Data = departmentMod,
    //         StatusCode = (int)HttpStatusCode.OK,
    //         CreatedAt = DateTime.Now
    //     };
    // }

    // //  Delete
    // //  Existenz-Prüfung: Überprüfe, ob das zu löschende Objekt existiert.
    // //  Null-Prüfung: Überprüfe, ob die ID null oder ungültig ist.
    // public async Task<DeleteResult<int>> DeleteDepartmentAsyncRep(int id)
    // {
    //     var department = await _appDbContextDepartment.PostDepartment.FindAsync(id);
    //     if (department == null)
    //         return new DeleteResult<int>
    //         {
    //             Success = false,
    //             Data = 0,
    //             StatusCode = (int)HttpStatusCode.NotFound,
    //             CreatedAt = DateTime.Now,
    //             Message = "DepartmentMod not found"
    //         };

    //     _appDbContextDepartment.PostDepartment.Remove(department);
    //     await _appDbContextDepartment.SaveChangesAsync();

    //     return new DeleteResult<int>
    //     {
    //         Success = true,
    //         Data = id,
    //         StatusCode = (int)HttpStatusCode.OK,
    //         CreatedAt = DateTime.Now
    //     };
    // }

// }

// public class DepartmentRepository : IDepartmentRepository
// {
//     private readonly AppDbContextDepartment _appDbContextDepartment;

//     public DepartmentRepository(AppDbContextDepartment appDbContextDepartment)
//     {
//         _appDbContextDepartment = appDbContextDepartment;
//     }

    // public async Task<GetAllResult<IList<DepartmentMod>>> GetAllDepartmentsAsyncRep()
    // {
    //     IList<DepartmentMod> result = await _appDbContextDepartment.PostDepartment.ToListAsync();
    //     if(result != null)
    //         return new GetAllResult<IList<DepartmentMod>>
    //         {
    //             Success = result != null && result.Count > 0,
    //             Data = result,
    //             StatusCode = result == null || result.Count == 0 ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.OK,
    //             CreatedAt = DateTime.Now
    //         };
    //     else
    //         return new GetAllResult<IList<DepartmentMod>>
    //             { Success = false, Message = "Departmentliste nicht gefunden.", CreatedAt = DateTime.Now };
    // }

//     public async Task<GetOneResult<DepartmentMod>> GetDepartmentByIdAsyncRep(int id)
//     {
//         DepartmentMod? result = await _appDbContextDepartment.PostDepartment.FindAsync(id);
//         if(result != null)
//             return new GetOneResult<DepartmentMod> { Success = true, Data = result };
//         else
//             return new GetOneResult<DepartmentMod> { Success = false, Message = "Department nicht gefunden." };
//     }

//     public async Task<PostResult<DepartmentMod>> PostDepartmentAsyncRep(DepartmentMod department)
//     {
//         try
//         {
//             _appDbContextDepartment.PostDepartment.Add(department);
//             await _appDbContextDepartment.SaveChangesAsync();
//             return new PostResult<DepartmentMod> { Success = true, Data = department};
//         }
//         catch (DbUpdateException ex)
//         {
//             return new PostResult<DepartmentMod> { Success = false, Message = ex.Message };
//         }
//         catch(Exception ex)
//         {
//             return new PostResult<DepartmentMod> { Success = false, Message = ex.Message };
//         }
//     }

//     public async Task<PutResult<DepartmentMod>> UpdateDepartmentAsyncRep(DepartmentMod department)
//     {
//         try
//         {
//             var existingDepartment = await _appDbContextDepartment.PostDepartment.FindAsync(department.Id);
            
//             if (existingDepartment == null)
//             {
//                 throw new InvalidOperationException("Das angegebene Department-ID wurde nicht gefunden.");
//             }
            
//             _appDbContextDepartment.Entry(existingDepartment).CurrentValues.SetValues(department);
//             await _appDbContextDepartment.SaveChangesAsync();
            
//             return new PutResult<DepartmentMod> { Success = true, Data = department };
//         }
//         catch (DbUpdateException ex)
//         {
//             return new PutResult<DepartmentMod> { Success = false, Message = ex.Message };
//         }
//         catch (InvalidOperationException ex)
//         {
//             return new PutResult<DepartmentMod> { Success = false, Message = ex.Message, CreatedAt = DateTime.Now };
//         }
//         catch(Exception ex)
//         {
//             return new PutResult<DepartmentMod> { Success = false, Message = ex.Message };
//         }
//     }

//     public async Task<DeleteResult<int>> DeleteDepartmentAsyncRep(int id)
//     {
//         var result = _appDbContextDepartment.PostDepartment.Find(id);
//         if(result != null)
//         {
//             _appDbContextDepartment.PostDepartment.Remove(result);
//             await _appDbContextDepartment.SaveChangesAsync();
//             return new DeleteResult<int> { Success = true };
//         }
//         else
//         {
//             return new DeleteResult<int> { Success = false };
//         }
//     }
// }