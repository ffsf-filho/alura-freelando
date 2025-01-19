using Freelando.Dados.Repository;

namespace Freelando.Dados.UnitOfWork;

public class UnitOfWork(FreelandoContext? context) : IUnitOfWork
{
    private EspecialidadeRepository? especialidadeRepository;
    private ClienteRepository? clienteRepository;
    private ContratoRepository? contratoRepository;
    private ProfissionalRepository? profissionalRepository;
    private ServicoRepository? servicoRepository;
    private CandidaturaRepository? candidaturaRepository;
    private ProjetoRepository? projetoRepository;

    public IEspecialidadeRepository EspecialidadeRepository
    {
        get
        {
            especialidadeRepository ??= new EspecialidadeRepository(context!);

            return especialidadeRepository;
        }
    }

    public ICandidaturaRepository CandidaturaRepository
    {
        get
        {
            candidaturaRepository ??= new CandidaturaRepository(context!);

            return candidaturaRepository;
        }
    }

    public IServicoRepository ServicoRepository
    {
        get
        {
            servicoRepository ??= new ServicoRepository(context!);

            return servicoRepository;
        }
    }

    public IClienteRepository ClienteRepository
    {
        get
        {
            clienteRepository ??= new ClienteRepository(context!);

            return clienteRepository;
        }
    }

    public IContratoRepository ContratoRepository
    {
        get
        {
            contratoRepository ??= new ContratoRepository(context!);

            return contratoRepository;
        }
    }

    public IProfissionalRepository ProfissionalRepository
    {
        get
        {
            profissionalRepository ??= new ProfissionalRepository(context!);
            
            return profissionalRepository;
        }
    }

    public IProjetoRepository ProjetoRepository
    {
        get
        {
            projetoRepository ??= new ProjetoRepository(context!);

            return projetoRepository;
        }
    }

    public FreelandoContext contexto => context!;
    public FreelandoContext? context = context;

    public async Task Commit()
    {
        await context!.SaveChangesAsync();
    }

    public void Dispose()
    {
        context!.Dispose();
    }
}
