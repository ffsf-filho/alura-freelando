namespace Freelando.Modelo;

public class Candidatura
{
    public Guid Id { get; set; }
    public double ValorProposto { get; set; }
    public string? DescricaoProposta { get; set; }
    public DuracaoEmDias DuracaoProposta { get; set; }
    public StatusCandidatura Status { get; set; }

    public Candidatura()
    {

    }

    public Candidatura(Guid id, double valorProposto, string? descricaoProposta, DuracaoEmDias duracaoProposta, StatusCandidatura status)
    {
        Id = id;
        ValorProposto = valorProposto;
        DescricaoProposta = descricaoProposta;
        DuracaoProposta = duracaoProposta;
        Status = status;
    }
}