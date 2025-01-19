using Freelando.Api.Converters;
using Freelando.Dados.UnitOfWork;
using Freelando.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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
    }
}

public class PropostaSummary
{
    public Guid Id_Proposta { get; set; }
    public DateTime Data_Proposta { get; set; }
    public DateTime Prazo_Entrega { get; set; }
}