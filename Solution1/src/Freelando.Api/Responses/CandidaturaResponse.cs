using Freelando.Modelo;

namespace Freelando.Api.Responses;
//record
public record CandidaturaResponse(Guid Id, double? ValorProposto, string? DescricaoProposta, string? DuracaoProposta, string? Status);
