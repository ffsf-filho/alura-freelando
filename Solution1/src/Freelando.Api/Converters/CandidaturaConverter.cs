using Freelando.Api.Requests;
using Freelando.Api.Responses;
using Freelando.Modelo;

namespace Freelando.Api.Converters;

public class CandidaturaConverter
{
    public CandidaturaResponse EntityToResponse(Candidatura? candidatura)
    {
        if (candidatura == null)
        {
            return new CandidaturaResponse(Guid.Empty, 0.0, "", DuracaoEmDias.DeQuinzeATrinta.ToString(), StatusCandidatura.Aprovada.ToString());
        }
        return new CandidaturaResponse(candidatura.Id, candidatura.ValorProposto, candidatura.DescricaoProposta, candidatura.DuracaoProposta.ToString(), candidatura.Status.ToString());
    }

    public Candidatura RequestToEntity(CandidaturaRequest? candidaturaRequest)
    {
        if (candidaturaRequest == null)
        {
            return new Candidatura(Guid.Empty, 0.0, null, DuracaoEmDias.MenosDeUm, StatusCandidatura.Aprovada);
        }
        return new Candidatura(candidaturaRequest.Id, candidaturaRequest.ValorProposto, candidaturaRequest.DescricaoProposta, candidaturaRequest.DuracaoProposta!.Value, candidaturaRequest.Status!.Value);
    }

    public ICollection<CandidaturaResponse> EntityListToResponseList(IEnumerable<Candidatura> candidaturas)
    {
        return (candidaturas == null)
            ? new List<CandidaturaResponse>()
            : candidaturas.Select(a => EntityToResponse(a)).ToList();
    }

    public ICollection<Candidatura> RequestListToEntityList(IEnumerable<CandidaturaRequest> candidaturaRequests)
    {
        if (candidaturaRequests == null)
        {
            return new List<Candidatura>();
        }
        return candidaturaRequests.Select(a => RequestToEntity(a)).ToList();
    }
}
