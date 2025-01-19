using System.Linq.Expressions;

namespace Freelando.Dados.Repository;

public interface IRepository<T>
{
    Task<IQueryable<T>> BuscarTodos();
    Task<T> BuscarPorId(Expression<Func<T, bool>> predicate);
    Task Adicionar(T entity);
    Task Atualizar(T entity);
    Task Deletar(T entity);
}