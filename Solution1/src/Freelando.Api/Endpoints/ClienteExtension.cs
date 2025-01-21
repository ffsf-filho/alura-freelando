using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Api.Responses;
using Freelando.Api.Services;
using Freelando.Dados;
using Freelando.Dados.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Transactions;

namespace Freelando.Api.Endpoints;

public static class ClienteExtension
{
    public static void AddEndPointClientes(this WebApplication app)
    {
        const string chaveCahe = "clientes";

        app.MapGet("/clientes", async ([FromServices] ClienteConverter converter, [FromServices] IUnitOfWork unitOfWork, IMemoryCache cache) =>
        {
            if(!cache.TryGetValue(chaveCahe, out ICollection<ClienteResponse> clientesCache))
            {
                clientesCache = converter.EntityListToResponseList(await unitOfWork.ClienteRepository.BuscarTodos());
                cache.Set(chaveCahe, clientesCache, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }

            return Results.Ok(await Task.FromResult(clientesCache));
        }).WithTags("Cliente").WithOpenApi();

        app.MapGet("/clientes/redis", async ([FromServices] ClienteConverter converter, [FromServices] IUnitOfWork unitOfWork, [FromServices] ICacheService cacheService) =>
        {
            var clientesCache = await cacheService.GetCachedDataAsync<IEnumerable<ClienteResponse>>(chaveCahe);

            if (clientesCache is not null)
            {
                return Results.Ok(clientesCache);
            }

            var clientes = converter.EntityListToResponseList(await unitOfWork.ClienteRepository.BuscarTodos());
            await cacheService.SetCachedDataAsync(chaveCahe, clientes, TimeSpan.FromMinutes(5));

            return Results.Ok(await Task.FromResult(clientes));
        }).WithTags("Cliente").WithOpenApi();

        app.MapGet("/clientes/identificador-nome", async ([FromServices] ClienteConverter converter, [FromServices] IUnitOfWork unitOfWork) =>
        {
            var clientes = unitOfWork.contexto.Clientes.Select(c => new { Identificador = c.Id, Nome = c.Nome });

            return Results.Ok(await Task.FromResult(clientes));
        }).WithTags("Cliente").WithOpenApi();

        app.MapGet("/clientes/identificador-epecialidades", async ([FromServices] ClienteConverter converter, [FromServices] IUnitOfWork unitOfWork) =>
        {
            var clientes = unitOfWork.contexto.Clientes
                .Include(x => x.Projetos)
                .ThenInclude(p => p.Especialidades)
                .AsSplitQuery()
                .ToList();

            return Results.Ok(await Task.FromResult(clientes));
        }).WithTags("Cliente").WithOpenApi();

        app.MapGet("/clientes/por-email", async ([FromServices] ClienteConverter converter, [FromServices] IUnitOfWork unitOfWork, string email) =>
        {
            var clientes = unitOfWork.contexto.Clientes
                .Where(x => x.Email!.Equals(email))
                .ToList();

            return Results.Ok(await Task.FromResult(clientes));
        }).WithTags("Cliente").WithOpenApi();

        app.MapPost("/cliente", async ([FromServices] ClienteConverter converter, [FromServices] IUnitOfWork unitOfWork, ClienteRequest clienteRequest, [FromServices] ICacheService cacheService ) =>
        {
            var cliente = converter.RequestToEntity(clienteRequest);
            await unitOfWork.ClienteRepository.Adicionar(cliente);
            await cacheService.RemoveCachedDataAsync(chaveCahe);
            await unitOfWork.Commit();

            return Results.Created($"/cliente/{cliente.Id}", cliente);
        }).WithTags("Cliente").WithOpenApi();

        app.MapPost("/clientes/new", async ([FromServices] ClienteConverter converter, [FromServices] IUnitOfWork unitOfWork, [FromServices] FreelandoClientesContext clientesContexto, ClienteRequest clienteRequest) =>
        {
            TransactionManager.ImplicitDistributedTransactions = true;

            using (var transactionScope = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                var cliente = converter.RequestToEntity(clienteRequest);
                cliente.Id = Guid.NewGuid();
                await unitOfWork.ClienteRepository.Adicionar(cliente);
                await unitOfWork.Commit();

                var newCliente = new ClienteNew { Id = Guid.NewGuid(), Nome = cliente.Nome, Email = cliente.Email, DataInclusao = DateTime.Now };
                await clientesContexto.ClienteNew.AddAsync(newCliente);
                clientesContexto.SaveChanges();

                transactionScope.Complete();
                return Results.Created($"/clientes/{cliente.Id}", cliente);
            }

        }).WithTags("Cliente").WithOpenApi();

        app.MapPut("/cliente/{id}", async ([FromServices] ClienteConverter converter, [FromServices] IUnitOfWork unitOfWork, Guid id, ClienteRequest clienteRequest) =>
        {
            var cliente = await unitOfWork.ClienteRepository.BuscarPorId(x => x.Id == id);

            if (cliente is null)
            {
                return Results.NotFound();
            }

            var clienteAtualizado = converter.RequestToEntity(clienteRequest);
            cliente.Nome = clienteAtualizado.Nome;
            cliente.Cpf = clienteAtualizado.Cpf;
            cliente.Email = clienteAtualizado.Email;
            cliente.Telefone = clienteAtualizado.Telefone;

            await unitOfWork.ClienteRepository.Atualizar(cliente);
            await unitOfWork.Commit();

            return Results.Ok((cliente));
        }).WithTags("Cliente").WithOpenApi();

        app.MapDelete("/cliente/{id}", async ([FromServices] ClienteConverter converter, [FromServices] IUnitOfWork unitOfWork, Guid id) =>
        {
            var cliente = await unitOfWork.ClienteRepository.BuscarPorId(x => x.Id == id);

            if (cliente is null)
            {
                return Results.NotFound();
            }

            await unitOfWork.ClienteRepository.Deletar(cliente);
            await unitOfWork.Commit();

            return Results.NoContent();
        }).WithTags("Cliente").WithOpenApi();
    }
}