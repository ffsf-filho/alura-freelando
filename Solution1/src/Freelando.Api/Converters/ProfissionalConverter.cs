using Freelando.Api.Requests;
using Freelando.Api.Responses;
using Freelando.Modelo;

namespace Freelando.Api.Converters;

public class ProfissionalConverter
{
    public ProfissionalResponse EntityToResponse(Profissional? profissional)
    {
        if (profissional == null)
        {
            return new ProfissionalResponse(Guid.Empty, null, null, null, null);
        }
        return new ProfissionalResponse(profissional.Id, profissional.Nome, profissional.Cpf, profissional.Email, profissional.Telefone);
    }

    public Profissional RequestToEntity(ProfissionalRequest? profissionalRequest)
    {
        if (profissionalRequest == null)
        {
            return new Profissional(Guid.Empty, null, null, null, null);
        }
        return new Profissional(profissionalRequest.Id, profissionalRequest.Nome, profissionalRequest.Cpf, profissionalRequest.Email, profissionalRequest.Telefone);
    }

    public ICollection<ProfissionalResponse> EntityListToResponseList(IEnumerable<Profissional> profissionais)
    {
        return (profissionais == null)
            ? new List<ProfissionalResponse>()
            : profissionais.Select(a => EntityToResponse(a)).ToList();
    }

    public ICollection<Profissional> RequestListToEntityList(IEnumerable<ProfissionalRequest> profissionalRequests)
    {
        if (profissionalRequests == null)
        {
            return new List<Profissional>();
        }
        return profissionalRequests.Select(a => RequestToEntity(a)).ToList();
    }
}
