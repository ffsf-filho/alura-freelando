using Freelando.Api.Requests;
using Freelando.Api.Responses;
using Freelando.Modelo;

namespace Freelando.Api.Converters;

public class EspecialidadeConverter
{
    public EspecialidadeResponse EntityToResponse(Especialidade? especialidade)
    {
        if (especialidade == null) { new EspecialidadeResponse(Guid.Empty, ""); }
        return new EspecialidadeResponse(especialidade!.Id, especialidade.Descricao);
    }

    public Especialidade RequestToEntity(EspecialidadeRequest? especialidade)
    {
        if (especialidade == null) { return new Especialidade(Guid.Empty, ""); }
        return new Especialidade(especialidade.Id, especialidade.Descricao);
    }

    public ICollection<EspecialidadeResponse> EntityListToResponseList(IEnumerable<Especialidade>? especialidades)
    {
        if (especialidades is null) { return new List<EspecialidadeResponse>(); }
        return especialidades.Select(a => EntityToResponse(a)).ToList();
    }

    public ICollection<Especialidade> RequestListToEntityList(IEnumerable<EspecialidadeRequest>? especialidades)
    {
        if (especialidades is null) { return new List<Especialidade>(); }
        return especialidades.Select(a => RequestToEntity(a)).ToList();
    }
}