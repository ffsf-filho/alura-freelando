using Freelando.Api.Converters;
using Freelando.Dados.UnitOfWork;
using Freelando.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Freelando.Api.Endpoints;

public static class PropostasExtensions
{
    public static void AddEndPointPropostas(this WebApplication app)
    {
        app.MapGet("/propostas", async ([FromServices] IUnitOfWork unitOfWork) =>
        {
            string fraseSQL = "EXEC dbo.sp_BuscarTodasPropostas";
            var propostas = await unitOfWork.contexto.Propostas.FromSqlRaw(fraseSQL).ToListAsync();

            return Results.Ok(propostas);
        }).WithTags("Propostas").WithOpenApi();

        app.MapGet("/propostas/projeto", async ([FromServices] IUnitOfWork unitOfWork, [FromQuery] Guid id_projeto) =>
        {
            string fraseSQL = $"EXEC dbo.sp_PropostaPorProjeto @Id_Projeto='{id_projeto}'";
            var propostas = await unitOfWork.contexto.Propostas.FromSqlRaw(fraseSQL).ToListAsync();

            return Results.Ok(propostas);
        }).WithTags("Propostas").WithOpenApi();

        app.MapGet("/propostas/summary", async ([FromServices] IUnitOfWork unitOfWork) =>
        {
            string fraseSQL = $"EXEC dbo.sp_PropostaSummary";
            var propostas = await unitOfWork.contexto.Database.SqlQueryRaw<PropostaSummary>(fraseSQL).ToListAsync();

            return Results.Ok(propostas);
        }).WithTags("Propostas").WithOpenApi();

        app.MapPost("/proposta", async ([FromServices] ProjetoConverter converter, [FromServices] IUnitOfWork unitOfWork, Propostas proposta) =>
        {
            Guid idProposta = Guid.NewGuid();
            string fraseSQL = "EXEC dbo.sp_InserirProposta @Id_Proposta, @Id_Projeto, @Id_Profissional, @Valor_Proposta, @Prazo_Entrega, @Mensagem";

            object[] parametros = new object[]
            {
                new SqlParameter("@Id_Proposta", idProposta),
                new SqlParameter("@Id_Projeto", proposta.ProjetoId),
                new SqlParameter("@Id_Profissional", proposta.ProfissionalId),
                new SqlParameter("@Valor_Proposta", proposta.ValorProposta),
                new SqlParameter("@Prazo_Entrega", proposta.PrazoEntrega),
                new SqlParameter("@Mensagem", proposta.Mensagem ?? (object)DBNull.Value)
            };

            await unitOfWork.contexto.Database.ExecuteSqlRawAsync(fraseSQL, parametros);

            return Results.Ok();
        }).WithTags("Propostas").WithOpenApi();

        app.MapPost("/propostas/upload", async ([FromForm] IFormFile file, [FromServices] IUnitOfWork unitOfWork) =>
        {
            if(file is null || file.Length == 0)
            {
                return Results.BadRequest("Arquivo não encontrado.");
            }

            var propostas = new List<Propostas>();

            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                var content = await stream.ReadToEndAsync();
                propostas = JsonSerializer.Deserialize<List<Propostas>>(content);
            }

            unitOfWork.contexto.Propostas.AddRange(propostas!);
            await unitOfWork.contexto.SaveChangesAsync();

            return Results.Ok(propostas);
        }).WithTags("Propostas").Accepts<IFormFile>("multipart/form-data").DisableAntiforgery();

        app.MapPut("/propostas/upload/update", async ([FromForm] IFormFile file, [FromServices] IUnitOfWork unitOfWork) =>
        {
            if (file == null || file.Length == 0)
            {
                return Results.BadRequest("Arquivo não encontrado.");
            }

            var profissionaisId = new List<Guid>();

            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                var content = await stream.ReadToEndAsync();
                profissionaisId = JsonSerializer.Deserialize<List<Guid>>(content);
            }

            unitOfWork.contexto.Propostas.Where(p => profissionaisId!.Contains(p.Id)).
            ExecuteUpdate(p => p.SetProperty(p => p.ValorProposta, p => p.ValorProposta + p.ValorProposta * 0.3m));

            await unitOfWork.contexto.SaveChangesAsync();

            return Results.Ok("Propostas atualizadas!");

        }).WithTags("Propostas").Accepts<IFormFile>("multipart/form-data").DisableAntiforgery();

        app.MapDelete("/propostas/upload/delete", async ([FromForm] IFormFile file, [FromServices] IUnitOfWork unitOfWork) =>
        {
            if (file == null || file.Length == 0)
            {
                return Results.BadRequest("Arquivo não encontrado.");
            }

            var propostasId = new List<Guid>();

            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                var content = await stream.ReadToEndAsync();
                propostasId = JsonSerializer.Deserialize<List<Guid>>(content);
            }

            unitOfWork.contexto.Propostas.Where(p => propostasId!.Contains(p.Id)).ExecuteDelete();

            await unitOfWork.contexto.SaveChangesAsync();

            return Results.Ok("Propostas removidas!");

        }).WithTags("Propostas").Accepts<IFormFile>("multipart/form-data").DisableAntiforgery();
    }
}
    
public class PropostaSummary
{
    public Guid Id_Proposta { get; set; }
    public DateTime Data_Proposta { get; set; }
    public DateTime Prazo_Entrega { get; set; }
}