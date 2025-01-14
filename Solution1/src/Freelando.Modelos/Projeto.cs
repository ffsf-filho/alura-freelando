using Freelando.Modelos;

namespace Freelando.Modelo;

public class Projeto
{
    public Guid Id { get; set; }
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public StatusProjeto Status { get; set; }
    public Cliente? Cliente { get; set; }
    public ICollection<Especialidade> Especialidades { get; set; }
    public ICollection<ProjetoEspecialidade> ProjetosEspecialidades { get; } = [];
    public ICollection<Servico> Servicos { get; set; }

    public Projeto()
    {
        
    }

    public Projeto(Guid id, string? titulo, string? descricao, StatusProjeto status, Cliente? cliente, ICollection<Especialidade> especialidades, ICollection<Servico> servicos)
    {
        Id = id;
        Cliente = cliente;
        Titulo = titulo;
        Descricao = descricao;
        Status = status;
        Especialidades = especialidades;
        Servicos = servicos;
    }
}