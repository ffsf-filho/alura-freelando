using Freelando.Api.Requests;
using Freelando.Api.Responses;
using Freelando.Modelo;

namespace Freelando.Api.Converters;

public class ProjetoConverter
{
    public ProjetoResponse EntityToResponse(Projeto projeto)
    {
        return (projeto is null)
            ? new ProjetoResponse(Guid.Empty, "", "", StatusProjeto.Disponivel.ToString())
            : new ProjetoResponse(projeto.Id, projeto.Titulo, projeto.Descricao, projeto.Status.ToString());
    }

    public Projeto RequestToEntity(ProjetoRequest projetoRequest)
    {
        return (projetoRequest is null)
            ? new Projeto(Guid.Empty, "", "", StatusProjeto.Disponivel)
            : new Projeto(projetoRequest.Id, projetoRequest.Titulo!, projetoRequest.Descricao!, projetoRequest.Status);
    }

    public ICollection<ProjetoResponse> EntityListToResponseList(IEnumerable<Projeto>? projetos)
    {
        return (projetos is null)
            ? new List<ProjetoResponse>()
            : projetos.Select(p => EntityToResponse(p)).ToList();
    }

    public ICollection<Projeto> RequestListToEntityList(IEnumerable<ProjetoRequest>? projetos)
    {
        return (projetos is null)
            ? new List<Projeto>()
            : projetos.Select(a => RequestToEntity(a)).ToList();
    }
}
