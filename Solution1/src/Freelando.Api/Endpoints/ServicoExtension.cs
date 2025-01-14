﻿using Freelando.Api.Converters;
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
    }
}