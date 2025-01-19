﻿using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ProfissionalExtension
{
    public static void AddEndPointProfissional(this WebApplication app)
    {
        app.MapGet("/profissionais", async ([FromServices] ProfissionalConverter converter, [FromServices] IUnitOfWork unitOfWork) =>
        {
            var profissional = converter.EntityListToResponseList(unitOfWork.contexto.Profissionais.Include(e => e.Especialidades).ToList());
            var entries = unitOfWork.contexto.ChangeTracker.Entries();

            return Results.Ok(await Task.FromResult(profissional));
        }).WithTags("Profissional").WithOpenApi();

        app.MapPost("/profissional", async ([FromServices] ProfissionalConverter converter, [FromServices] IUnitOfWork unitOfWork, ProfissionalRequest profissionalRequest) =>
        {
            var profissional = converter.RequestToEntity(profissionalRequest);
            await unitOfWork.ProfissionalRepository.Adicionar(profissional);
            await unitOfWork.Commit();

            return Results.Created($"/profissional/{profissional.Id}", profissional);
        }).WithTags("Profissional").WithOpenApi();

        app.MapPut("/profissional/{id}", async ([FromServices] ProfissionalConverter converter, [FromServices] IUnitOfWork unitOfWork, Guid id, ProfissionalRequest profissionalRequest) =>
        {
            var profissional = await unitOfWork.ProfissionalRepository.BuscarPorId(x => x.Id == id);

            if (profissional is null)
            {
                return Results.NotFound();
            }

            var profissionalAtualizado = converter.RequestToEntity(profissionalRequest);
            profissional.Nome = profissionalAtualizado.Nome;
            profissional.Cpf = profissionalAtualizado.Cpf;
            profissional.Email = profissionalAtualizado.Email;
            profissional.Telefone = profissionalAtualizado.Telefone;

            await unitOfWork.ProfissionalRepository.Atualizar(profissional);
            await unitOfWork.Commit();

            return Results.Ok((profissional));
        }).WithTags("Profissional").WithOpenApi();

        app.MapDelete("/profissional/{id}", async ([FromServices] ProfissionalConverter converter, [FromServices] IUnitOfWork unitOfWork, Guid id) =>
        {
            var profissional = await unitOfWork.ProfissionalRepository.BuscarPorId(x => x.Id == id);

            if (profissional is null)
            {
                return Results.NotFound();
            }

            await unitOfWork.ProfissionalRepository.Deletar(profissional);
            await unitOfWork.Commit();

            return Results.NoContent();
        }).WithTags("Profissional").WithOpenApi();
    }
}