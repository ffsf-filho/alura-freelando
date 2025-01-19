
using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados.UnitOfWork;
using Freelando.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Freelando.Api.Endpoints;

public static class EspecialidadeExtension
{
    public static void AddEndPointEspecialidades(this WebApplication app)
    {
        app.MapGet("/especialidades", async ([FromServices] EspecialidadeConverter converter, [FromServices] IUnitOfWork unitOfWork) =>
        {
            var especialidades = converter.EntityListToResponseList(await unitOfWork.EspecialidadeRepository.BuscarTodos());

            return Results.Ok((especialidades));
        }).WithTags("Especialidade").WithOpenApi();

        app.MapGet("/especialidades/{letraInicial}", async ([FromServices] EspecialidadeConverter converter, [FromServices] IUnitOfWork unitOfWork, string letraInicial) =>
        {
            Expression<Func<Especialidade, bool>>? filtroExpression = null;

            if (letraInicial.Length == 1 && char.IsUpper(letraInicial[0]))
            {
                filtroExpression = especialidade => especialidade.Descricao!.StartsWith(letraInicial);
            }

            IQueryable<Especialidade> especialidades = unitOfWork.contexto.Especialidades;

            if (filtroExpression is not null)
            {
                especialidades = especialidades.Where(filtroExpression);
            }

            return Results.Ok(await especialidades.ToListAsync());
        }).WithTags("Especialidade").WithOpenApi();

        app.MapPost("/especialidade", async ([FromServices] EspecialidadeConverter converter, IUnitOfWork unitOfWork, EspecialidadeRequest especialidadeRequest) =>
        {
            var especialidade = converter.RequestToEntity(especialidadeRequest);

            Func<Especialidade, bool> validarDescricao = especialidade => !string.IsNullOrEmpty(especialidade.Descricao) && char.IsUpper(especialidade.Descricao[0]);

            if (!validarDescricao(especialidade))
            {
                return Results.BadRequest("A descrição não pode estar em branco e deve começar com letra maiúscula.");
            }

            await unitOfWork.EspecialidadeRepository.Adicionar(especialidade);
            await unitOfWork.Commit();

            return Results.Created($"/especialidade/{especialidade.Id}", especialidade);
        }).WithTags("Especialidade").WithOpenApi();

        app.MapPut("/especialidade/{id}", async ([FromServices] EspecialidadeConverter converter, [FromServices] IUnitOfWork unitOfWork, Guid id, EspecialidadeRequest especialidadeRequest) =>
        {
            var especialidade = await unitOfWork.EspecialidadeRepository.BuscarPorId(x => x.Id == id);

            if (especialidade is null)
            {
                return Results.NotFound();
            }

            var especialidadeAtualizada = converter.RequestToEntity(especialidadeRequest);
            especialidade.Descricao = especialidadeAtualizada.Descricao;
            especialidade.Projetos = especialidadeAtualizada.Projetos;

            await unitOfWork.EspecialidadeRepository.Atualizar(especialidade);
            await unitOfWork.Commit();

            return Results.Ok(especialidade);
        }).WithTags("Especialidade").WithOpenApi();

        //app.MapDelete("/especialidade/{id}", async ([FromServices] EspecialidadeConverter converter, [FromServices] IUnitOfWork unitOfWork, Guid id) =>
        //{
        //    var especialidade = await unitOfWork.EspecialidadeRepository.BuscarPorId(x => x.Id == id);

        //    if (especialidade is null)
        //    {
        //        return Results.NotFound();
        //    }

        //    await unitOfWork.EspecialidadeRepository.Deletar(especialidade);
        //    await unitOfWork.Commit();

        //    return Results.NoContent();
        //}).WithTags("Especialidade").WithOpenApi();

        app.MapDelete("/especialidade/{id}", async ([FromServices] EspecialidadeConverter converter, [FromServices] IUnitOfWork unitOfWork, Guid id) =>
        {
            using (var transaction = unitOfWork.contexto.Database.BeginTransaction())
            {
                try
                {
                    var especialidade = await unitOfWork.contexto.Especialidades.FindAsync(id);

                    if (especialidade is null)
                    {
                        return Results.NotFound();
                    }

                    unitOfWork.contexto.Especialidades.Remove(especialidade);
                    await unitOfWork.contexto.SaveChangesAsync();

                    transaction.Commit();

                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }).WithTags("Especialidade").WithOpenApi();
    }
}