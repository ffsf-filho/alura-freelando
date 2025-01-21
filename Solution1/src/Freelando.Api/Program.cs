using Freelando.Api.Converters;
using Freelando.Api.Endpoints;
using Freelando.Api.Services;
using Freelando.Dados;
using Freelando.Dados.Repository;
using Freelando.Dados.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FreelandoContext>((options) =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
});

builder.Services.AddDbContext<FreelandoClientesContext>((options) =>
{
    options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FreelandoClientes;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
});

builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
});

builder.Services.AddScoped<ICacheService, CacheService>();

builder.Services.AddScoped<IEspecialidadeRepository, EspecialidadeRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddTransient<FreelandoContext>();
builder.Services.AddTransient(typeof(CandidaturaConverter));
builder.Services.AddTransient(typeof(ClienteConverter));
builder.Services.AddTransient(typeof(ContratoConverter));
builder.Services.AddTransient(typeof(EspecialidadeConverter));
builder.Services.AddTransient(typeof(ProfissionalConverter));
builder.Services.AddTransient(typeof(ProjetoConverter));
builder.Services.AddTransient(typeof(ServicoConverter));

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(
    options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
    );

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(
        options =>
        {
            options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    );

//Endpoints
app.AddEndPointCandidatura();
app.AddEndPointClientes();
app.AddEndPointContrato();
app.AddEndPointProfissional();
app.AddEndPointEspecialidades();
app.AddEndPointProjeto();
app.AddEndPointPropostas();
app.AddEndPointRelatorios();
app.AddEndPointServico();

app.UseHttpsRedirection();

app.Run();