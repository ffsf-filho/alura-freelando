namespace Freelando.Modelos;

public class Endereco
{
    public string? Logradouro { get; set; }
    public string? Numero { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public string? Cep { get; set; }

    protected Endereco()
    {
    }

    public Endereco(string? logradouro, string? numero, string? complemento, string? bairro, string? cidade, string? estado, string? cep)
    {

        Logradouro = logradouro;
        Numero = numero;
        Complemento = complemento;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        Cep = cep;
    }
}