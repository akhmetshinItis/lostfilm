using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace MyORMLibrary;

public class ORMContext<T> where T : class, new()
{
    private readonly IDbConnection _dbConnection;

    public ORMContext(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    public void Create(T entity)
    {
        var properties = entity.GetType().GetProperties()
            .Where(p => p.Name.ToLower() != "id");
        var columns = string.Join(", ", properties.Select(p => p.Name));
        var values = string.Join(", ", properties.Select(p => '@' + p.Name));
        string query = $"INSERT INTO {typeof(T).Name}s ({columns}) VALUES ({values})";
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            foreach (var property in properties)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = '@' + property.Name;
                parameter.Value = property.GetValue(entity) ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }
            _dbConnection.Open();
            command.ExecuteNonQuery();
            _dbConnection.Close();
        }
    }
    
    
    public T GetById(int Id)
    {
        string query = $"SELECT * FROM {typeof(T).Name}s WHERE Id = @Id";

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@Id";
            parameter.Value = Id;
            command.Parameters.Add(parameter);

            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    var result = Map(reader);
                    _dbConnection.Close();
                    return result;
                }
            }
        }

        return null;
    }

    public void Update(T entity)
    {
        var properties = entity.GetType().GetProperties()
            .Where(p => p.Name != "Id");
        var values = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));
        string query = $"UPDATE {typeof(T).Name}s SET {values} WHERE Id = @Id";
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@Id";
            idParameter.Value = entity.GetType().GetProperty("Id").GetValue(entity);
            command.Parameters.Add(idParameter);
            foreach (var property in properties)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = '@' + property.Name;
                parameter.Value = property.GetValue(entity) ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }
            _dbConnection.Open();
            command.ExecuteNonQuery();
            _dbConnection.Close();
        }
    }

    public void Delete(int id, string tableName)
    {
        string query = $"DELETE FROM {tableName} WHERE Id = @Id";
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@Id";
            parameter.Value = id;
            command.Parameters.Add(parameter);
            _dbConnection.Open();
            command.ExecuteNonQuery();
            _dbConnection.Close();
        }
    }
    public void Delete(T entity)
    {
        var properties = entity.GetType().GetProperties();
        var condition = string.Join(" AND ", properties.Select(p => $"{p.Name} = @{p.Name}"));
        string query = $"DELETE FROM {typeof(T).Name}s WHERE {condition}";
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            foreach (var property in properties)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = '@' + property.Name;
                parameter.Value = property.GetValue(entity);
                command.Parameters.Add(parameter);
            }
            _dbConnection.Open();
            command.ExecuteNonQuery();
            _dbConnection.Close();
        }
    }
    
    public List<T> GetAll()
    {
        string query = $"SELECT * FROM {typeof(T).Name}s";
        using (var command = _dbConnection.CreateCommand())
        {
            var result = new List<T>();
            command.CommandText = query;
            try
            {
                _dbConnection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(Map(reader));
                    }

                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<T>();
            }
            finally
            {
                _dbConnection.Close();
            }
        }
    }
    
    
    private T Map(IDataReader reader)
    {
        var obj = new T();
        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            if (reader[property.Name] != DBNull.Value)
            {
                property.SetValue(obj, reader[property.Name]);
            }
        }
        return obj;
    }
    
    // --------------------
    
    public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
    {
        var sqlQuery = ExpressionParser<T>.BuildSqlQuery(predicate, singleResult: false);
        return ExecuteQueryMultiple(sqlQuery);
    }
    
    public T FirstOrDefault(Expression<Func<T, bool>> predicate)
    {
        var sqlQuery = ExpressionParser<T>.BuildSqlQuery(predicate, singleResult: true);
        var result = ExecuteQuerySingle(sqlQuery);
        if (_dbConnection.State == ConnectionState.Open)
        {
            _dbConnection.Close();
        }

        return result;
    }
    
    private T ExecuteQuerySingle(string query)
    {
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return Map(reader);
                }
            }
            _dbConnection.Close();
        }
 
        return null;
    }
 
    private IEnumerable<T> ExecuteQueryMultiple(string query)
    {
        var results = new List<T>();
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    results.Add(Map(reader));
                }
            }
            _dbConnection.Close();
        }
        return results;
    }
}

// public T FirstOrDefault(Predicate<T> predicate)
// {
//     var query = $"SELECT * FROM {typeof(T).Name}s";
//     using (var command = _dbConnection.CreateCommand())
//     {
//         command.CommandText = query;
//         _dbConnection.Open();
//         using (var reader = command.ExecuteReader())
//         {
//             while (reader.Read())
//             {
//                 var entity = Map(reader);
//                 if (predicate(entity))
//                 {
//                     return entity;
//                 }
//             }
//         }
//         _dbConnection.Close();
//     }
//     return null;
// }

// public List<T> Where(Predicate<T> predicate)
// {
//     var query = $"SELECT * FROM {typeof(T).Name}s";
//     using (var command = _dbConnection.CreateCommand())
//     {
//         command.CommandText = query;
//         _dbConnection.Open();
//         using (var reader = command.ExecuteReader())
//         {
//             var result = new List<T>();
//             while (reader.Read())
//             {
//                 var entity = Map(reader);
//                 if (predicate(entity))
//                 {
//                     result.Add(entity);
//                 }
//             }
//
//             return result;
//         }
//     }
// }