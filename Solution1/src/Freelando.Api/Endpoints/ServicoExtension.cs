using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;

namespace Freelando.Api.Endpoints;

public static class ServicoExtensions
{
    public static void AddEndPointServico(this WebApplication app)
    {
        app.MapGet("/servicos", async ([FromServices] ServicoConverter converter, [FromServices] FreelandoContext contexto) =>
        {
            var servico = converter.EntityListToResponseList([.. contexto.Servicos]);
            return Results.Ok(await Task.FromResult(servico));
        }).WithTags("Servicos").WithOpenApi();

        app.MapPost("/servico", async ([FromServices] ServicoConverter converter, [FromServices] FreelandoContext contexto, ServicoRequest servicoRequest) =>
        {
            var servico = converter.RequestToEntity(servicoRequest);
            await contexto.Servicos.AddAsync(servico);
            await contexto.SaveChangesAsync();

            return Results.Created($"/servico/{servico.Id}", servico);
        }).WithTags("Servicos").WithOpenApi();
    }
}