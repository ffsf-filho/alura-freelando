namespace Freelando.Modelo;

public class Especialidade
{
    public Guid Id { get; set; }
    public string? Descricao { get; set; }

    public Especialidade()
    {
        
    }

    public Especialidade(Guid id, string? descricao)
    {
        Id = id;
        Descricao = descricao;
    }
}