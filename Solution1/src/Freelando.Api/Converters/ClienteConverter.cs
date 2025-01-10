using Freelando.Api.Requests;
using Freelando.Api.Responses;
using Freelando.Modelo;

namespace Freelando.Api.Converters;

public class ClienteConverter
{
    public ClienteResponse EntityToResponse(Cliente? cliente)
    {
        if (cliente == null)
        {
            return new ClienteResponse(Guid.Empty, "", "", "", "");
        }
        return new ClienteResponse(cliente.Id, cliente.Nome, cliente.Cpf, cliente.Email, cliente.Telefone);
    }

    public Cliente RequestToEntity(ClienteRequest? cliente)
    {
        if (cliente == null) { return new Cliente(Guid.Empty, "", "", "", ""); }
        return new Cliente(cliente.Id, cliente.Nome!, cliente.Cpf!, cliente.Email!, cliente.Telefone!);
    }

    public ICollection<ClienteResponse>? EntityListToResponseList(IEnumerable<Cliente>? clientes)
    {
        return (clientes is null)
            ? new List<ClienteResponse>()
            : clientes.Select(a => EntityToResponse(a)).ToList();
    }

    public ICollection<Cliente> RequestListToEntityList(IEnumerable<ClienteRequest>? clientes)
    {
        if (clientes is null) { return new List<Cliente>(); }
        return clientes.Select(a => RequestToEntity(a)).ToList();
    }
}
