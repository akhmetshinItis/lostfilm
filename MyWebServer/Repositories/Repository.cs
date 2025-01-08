using System.Linq.Expressions;
using HttpServerLibrary.Models;
using Microsoft.Data.SqlClient;
using my_http.Repositories;
using MyORMLibrary;

namespace MyHttp.Repositories;

public class Repository<T> : IRepository<T> where T : class, new()
{
    private readonly ORMContext<T> _dbContext;

    public Repository()
    { 
        var connection = new SqlConnection(AppConfig.GetInstance().ConnectionString);
        _dbContext = new ORMContext<T>(connection);
    }

    public List<T> GetAll()
    {
        return _dbContext.GetAll();
    }

    public T GetById(int id)
    {
        return _dbContext.GetById(id);
    }

    public void Create(T entity)
    {
        _dbContext.Create(entity);
    }

    public void Update(T entity)
    {
        _dbContext.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbContext.Delete(entity);
    }

    public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
    {
        return _dbContext.Where(predicate);
    }

    public T FirstOrDefault(Expression<Func<T, bool>> predicate)
    {
        return _dbContext.FirstOrDefault(predicate);
    }
}