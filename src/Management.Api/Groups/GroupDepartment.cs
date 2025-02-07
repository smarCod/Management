


using Management.Core.Models.DepartmentModels.DepartmentMediator.Queries;
using Management.Core.Models.DepartmentModels.DepartmentMediator.Command;
using Management.Core.Models.DepartmentModels.DepartmentMediator.Responses;

using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Management.Api.Groups;

public static class GroupDepartment
{
    public static RouteGroupBuilder GroupDepartmentApi(this RouteGroupBuilder builder)
    {
        builder.MapGet("/v1/department/getalldepartments/", GetAllDepartments);
        builder.MapGet("/v1/department/getdepartmentbyid/{id:int}", GetDepartmentById);
        builder.MapPost("/v1/department/postdepartment/{name}", PostDepartment);
        builder.MapPut("/v1/department/updatedepartment/{id:int}/{name}", UpdateDepartment);
        builder.MapDelete("/v1/department/deletedepartment/{id:int}", DeleteDepartment);
        return builder;
    }

    //  Department Queries
    public static async Task<Results<Ok<IList<DepartmentQueryResponse>>, NotFound>> GetAllDepartments(IMediator mediator)
    {
        var query = new GetAllDepartmentsQuery();
        var result = await mediator.Send(query);

        Console.WriteLine($"Result: {result}"); // Debugging-Ausgabe
        Console.WriteLine($"Result is null: {result == null}");
        Console.WriteLine($"Result Any: {result?.Any()}");
    
        // Prüfe zuerst auf null
        if (result == null)
        {
            return TypedResults.NotFound();
        }

        // Prüfe dann auf leere Liste
        if (!result.Any())
        {
            return TypedResults.NotFound();
        }

        // Nur wenn beides nicht zutrifft, gib Ok zurück
        return TypedResults.Ok(result);
    }

    public static async Task<Results<Ok<DepartmentQueryResponse>, NotFound>> GetDepartmentById(IMediator mediator, int id)
    {
        var query = new GetDepartmentByIdQuery(id);
        var result = await mediator.Send(query);
        
        if(result == null)
        {
            return TypedResults.NotFound();
        }
        
        return TypedResults.Ok(result);
    }

    // Department Commands
    public static async Task<Results<Ok<DepartmentCommandResponse>, BadRequest>> PostDepartment(IMediator mediator, string name)
    {
        var query = new PostDepartmentCommand(name);
        var result = await mediator.Send(query);
        if (result == null)
        {
            return TypedResults.BadRequest();
        }
        return TypedResults.Ok(result);
    }

    public static async Task<Results<Ok<DepartmentCommandResponse>, BadRequest>> UpdateDepartment(IMediator mediator, int id, string name)
    {
        var query = new UpdateDepartmentCommand(id, name);
        var result = await mediator.Send(query);
        if (result == null)
        {
            return TypedResults.BadRequest();
        }
        return TypedResults.Ok(result);
        // Vielleicht erweitern!
        // if (result.Id <= 0)
        // {
        //     return TypedResults.BadRequest(new DepartmentCommandResponse(ErrorMessage: "Ungültige ID"));
        // }

        // if (string.IsNullOrEmpty(result.Name))
        // {
        //     return TypedResults.BadRequest(new DepartmentCommandResponse(ErrorMessage: "Leerer Departmentname"));
        // }

        // var query = new UpdateDepartmentCommand(id, name);
        // var result = await mediator.Send(query);
        // if (result == null)
        // {
        //     return TypedResults.BadRequest(new DepartmentCommandResponse(ErrorMessage: "Department konnte nicht aktualisiert werden"));
        // }
        // return TypedResults.Ok(result);
    }

    public static async Task<Results<Ok<DepartmentCommandResponse>, NotFound>> DeleteDepartment(IMediator mediator, int id)
    {
        var command = new DeleteDepartmentCommand(id);
        var result = await mediator.Send(command);
        if (result == null)
        {
            return TypedResults.NotFound();
        }
        return TypedResults.Ok(result);
    }

}

