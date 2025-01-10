using Freelando.Api.Requests;
using Freelando.Api.Responses;
using Freelando.Modelo;

namespace Freelando.Api.Converters;

public class ServicoConverter
{
    public ServicoResponse EntityToResponse(Servico? servico)
    {
        if (servico == null)
        {
            return new ServicoResponse(Guid.Empty, null, null, StatusServico.Disponivel.ToString());
        }
        return new ServicoResponse(servico.Id, servico.Titulo, servico.Descricao, servico.Status.ToString());
    }

    public Servico RequestToEntity(ServicoRequest? servicoRequest)
    {
        if (servicoRequest == null)
        {
            return new Servico(Guid.Empty, null, null, StatusServico.Disponivel);
        }
        return new Servico(servicoRequest.Id, servicoRequest.Titulo, servicoRequest.Descricao, servicoRequest.Status);
    }

    public ICollection<ServicoResponse> EntityListToResponseList(IEnumerable<Servico> servicos)
    {
        return (servicos == null)
            ? new List<ServicoResponse>()
            : servicos.Select(a => EntityToResponse(a)).ToList();
    }

    public ICollection<Servico> RequestListToEntityList(IEnumerable<ServicoRequest> servicosRequests)
    {
        if (servicosRequests == null)
        {
            return new List<Servico>();
        }
        return servicosRequests.Select(a => RequestToEntity(a)).ToList();
    }
}
