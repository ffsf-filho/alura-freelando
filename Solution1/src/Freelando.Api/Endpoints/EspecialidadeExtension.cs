
using Freelando.Api.Converters;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class EspecialidadeExtension
{
    public static void AddEndPointEspecialidades(this WebApplication app)
    {
        app.MapGet("/especialidades", async ([FromServices] EspecialidadeConverter converter, FreelandoContext context) =>
        {
            var especialidades = converter.EntityListToResponseList(context.Especialidades.ToList());
            return Results.Ok((especialidades));
        }).WithTags("Especialidade").WithOpenApi();
    }
}