using System.Linq.Expressions;

namespace my_http.Repositories;

public interface IRepository<T> where T : class
{
    List<T> GetAll();
    void Delete(T entity);
    void Update(T entity);
    T GetById(int id);
    void Create(T entity);
    IEnumerable<T> Where(Expression<Func<T, bool>> predicate);
    T FirstOrDefault(Expression<Func<T, bool>> predicate);
}