﻿using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ProjetoExtension
{
    public static void AddEndPointProjeto(this WebApplication app)
    {
        app.MapGet("/projetos", async ([FromServices] ProjetoConverter converter, IUnitOfWork unitOfWork) =>
        {
            var projetos = converter.EntityListToResponseList(
                    unitOfWork.contexto.Projetos
                        .Include(p => p.Cliente)
                        .Include(p => p.Especialidades)
                        .ToList()
                );
            return Results.Ok(await Task.FromResult(projetos));
        }).WithTags("Projeto").WithOpenApi();

        app.MapGet("/projetos/vigencia", async ([FromServices] ProjetoConverter converter, IUnitOfWork unitOfWork) =>
        {
            var projetos = unitOfWork.ProjetoRepository.BuscarTodos();

            return Results.Ok(await Task.FromResult(projetos));
        }).WithTags("Projeto").WithOpenApi();

        app.MapPost("/projeto", async ([FromServices] ProjetoConverter converter, IUnitOfWork unitOfWork, ProjetoRequest projetoRequest) =>
        {
            var projeto = converter.RequestToEntity(projetoRequest);
            await unitOfWork.ProjetoRepository.Adicionar(projeto);
            await unitOfWork.Commit();

            return Results.Created($"/projeto/{projeto.Id}", projeto);
        }).WithTags("Projeto").WithOpenApi();

        app.MapPut("/projeto/{id}", async ([FromServices] ProjetoConverter converter, [FromServices] IUnitOfWork unitOfWork, Guid id, ProjetoRequest projetoRequest) =>
        {
            var projeto = await unitOfWork.ProjetoRepository.BuscarPorId(x => x.Id == id);

            if (projeto is null)
            {
                return Results.NotFound();
            }

            var projetoAtualizado = converter.RequestToEntity(projetoRequest);
            projeto.Titulo = projetoAtualizado.Titulo;
            projeto.Descricao = projetoAtualizado.Descricao;
            projeto.Status = projetoAtualizado.Status;

            await unitOfWork.ProjetoRepository.Atualizar(projeto);
            await unitOfWork.Commit();

            return Results.Ok((projeto));
        }).WithTags("Projeto").WithOpenApi();

        app.MapDelete("/projeto/{id}", async ([FromServices] ProjetoConverter converter, [FromServices] IUnitOfWork unitOfWork, Guid id) =>
        {
            var projeto = await unitOfWork.ProjetoRepository.BuscarPorId(x => x.Id == id);

            if (projeto is null)
            {
                return Results.NotFound();
            }

            await unitOfWork.ProjetoRepository.Deletar(projeto);
            await unitOfWork.Commit();

            return Results.NoContent();
        }).WithTags("Projeto").WithOpenApi();
    }
}