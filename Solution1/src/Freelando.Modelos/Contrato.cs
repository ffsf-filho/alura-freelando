using Freelando.Modelos;

namespace Freelando.Modelo;

public class Contrato
{
    public Guid Id { get; set; }
    public double Valor { get; set; }
    public Vigencia? Vigencia { get; set; }

    public Contrato()
    {
        
    }

    public Contrato(Guid id, double valor)
    {
        Id = id;
        Valor = valor;
    }

    public Contrato(Guid id, double valor, Vigencia? vigencia)
    {
        Id = id;
        Valor = valor;
        Vigencia = vigencia;
    }
}