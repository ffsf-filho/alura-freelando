using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ProjetoExtension
{
    public static void AddEndPointProjeto(this WebApplication app)
    {
        app.MapGet("/projetos", async ([FromServices] ProjetoConverter converter, FreelandoContext contexto) =>
        {
            var projetos = converter.EntityListToResponseList(
                    contexto.Projetos
                        .Include(p => p.Cliente)
                        .Include(p => p.Especialidades)
                        .ToList()
                );
            return Results.Ok( await Task.FromResult(projetos));
        }).WithTags("Projeto").WithOpenApi();

        app.MapPost("/projeto", async ([FromServices] ProjetoConverter converter, FreelandoContext contexto, ProjetoRequest projetoRequest) =>
        {
            var projeto = converter.RequestToEntity(projetoRequest);
            await contexto.Projetos.AddAsync(projeto);
            await contexto.SaveChangesAsync();

            return Results.Created($"/projeto/{projeto.Id}", projeto);
        }).WithTags("Projeto").WithOpenApi();
    }
}