using Freelando.Modelo;

namespace Freelando.Dados.Repository;

public class ServicoRepository : Repository<Servico>, IServicoRepository
{
    public ServicoRepository(FreelandoContext context) : base(context)
    {
        
    }
}