using System.Linq.Expressions;
using HttpServerLibrary.Models;
using Microsoft.Data.SqlClient;
using my_http.Models.Entities;
using MyORMLibrary;

namespace my_http.Repositories.EntityRepositories
{
    public class GenreRepository : IRepository<Genre>
    {
        private readonly ORMContext<Genre> _dbContext;

        public GenreRepository()
        { 
            var connection = new SqlConnection(AppConfig.GetInstance().ConnectionString);
            _dbContext = new ORMContext<Genre>(connection);
        }
        
        public List<Genre> GetAll()
        {
            return _dbContext.GetAll();
        }

        public void Delete(Genre entity)
        {
            _dbContext.Delete(entity);
        }

        public void Update(Genre entity)
        {
            _dbContext.Update(entity);
        }

        public Genre GetById(int id)
        {
            return _dbContext.GetById(id);
        }

        public void Create(Genre entity)
        {
            _dbContext.Create(entity);
        }

        public IEnumerable<Genre> Where(Expression<Func<Genre, bool>> predicate)
        {
            return _dbContext.Where(predicate);
        }

        public Genre FirstOrDefault(Expression<Func<Genre, bool>> predicate)
        {
            return _dbContext.FirstOrDefault(predicate);
        }
    }
}