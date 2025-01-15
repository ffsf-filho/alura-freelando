
using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class EspecialidadeExtension
{
    public static void AddEndPointEspecialidades(this WebApplication app)
    {
        app.MapGet("/especialidades", async ([FromServices] EspecialidadeConverter converter, FreelandoContext contexto) =>
        {
            var especialidades = converter.EntityListToResponseList(contexto.Especialidades.ToList());
            return Results.Ok((especialidades));
        }).WithTags("Especialidade").WithOpenApi();

        app.MapPost("/especialidade", async ([FromServices] EspecialidadeConverter converter, FreelandoContext contexto, EspecialidadeRequest especialidadeRequest) =>
        {
            var especialidade = converter.RequestToEntity(especialidadeRequest);
            await contexto.Especialidades.AddAsync(especialidade);
            await contexto.SaveChangesAsync();
            return Results.Created($"/especialidade/{especialidade.Id}",especialidade);
        }).WithTags("Especialidade").WithOpenApi();

        app.MapPut("/especialidade/{id}", async ([FromServices] EspecialidadeConverter converter, FreelandoContext contexto, Guid id, EspecialidadeRequest especialidadeRequest) =>
        {
            var especialidade = await contexto.Especialidades.FindAsync(id);

            if (especialidade is null)
            {
                return Results.NotFound();
            }

            var especialidadeAtualizada = converter.RequestToEntity(especialidadeRequest);
            especialidade.Descricao = especialidadeAtualizada.Descricao;
            especialidade.Projetos = especialidadeAtualizada.Projetos;

            await contexto.SaveChangesAsync();

            return Results.Ok(especialidade);
        }).WithTags("Especialidade").WithOpenApi();

        app.MapDelete("/especialidade/{id}", async ([FromServices] EspecialidadeConverter converter, FreelandoContext contexto, Guid id) =>
        {
            var especialidade = await contexto.Especialidades.FindAsync(id);

            if (especialidade is null)
            {
                return Results.NotFound();
            }

            contexto.Especialidades.Remove(especialidade);
            await contexto.SaveChangesAsync();

            return Results.NoContent();
        }).WithTags("Especialidade").WithOpenApi();
    }
}