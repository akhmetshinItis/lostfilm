using System.Linq.Expressions;
using HttpServerLibrary.Models;
using Microsoft.Data.SqlClient;
using my_http.Models.Entities;
using MyORMLibrary;

namespace my_http.Repositories.EntityRepositories;

public class MovieRepository : IRepository<Movie>
{
    private readonly ORMContext<Movie> _dbContext;

    public MovieRepository()
    { 
        var connection = new SqlConnection(AppConfig.GetInstance().ConnectionString);
        _dbContext = new ORMContext<Movie>(connection);
    }
    
    public List<Movie> GetAll()
    {
        return _dbContext.GetAll();
    }

    public void Delete(Movie entity)
    {
        _dbContext.Delete(entity);
    }

    public void Update(Movie entity)
    {
        _dbContext.Update(entity);
    }

    public Movie GetById(int id)
    {
        return _dbContext.GetById(id);
    }

    public void Create(Movie entity)
    {
        _dbContext.Create(entity);
    }

    public IEnumerable<Movie> Where(Expression<Func<Movie, bool>> predicate)
    {
        return _dbContext.Where(predicate);
    }

    public Movie FirstOrDefault(Expression<Func<Movie, bool>> predicate)
    {
        return _dbContext.FirstOrDefault(predicate);
    }
}