using System.Data.Common;
using System.Data.SqlClient;
using HttpServerLibrary;
using HttpServerLibrary.Core;
using HttpServerLibrary.Core.Attributes;
using HttpServerLibrary.Core.HttpResponse;
using my_http.Models;
using MyORMLibrary;

namespace MyServer.Endpoints;

public class UserEndpoints : EndpointBase
{
    [Get("users")]
    public IHttpResponseResult GetAllUsers()
    {
        
        var connectionString = @"Data Source = localhost;Initial Catalog=master;User ID=sa;Password=P@ssw0rd;";
        var sqlConection = new SqlConnection(connectionString);
        var res = new ORMContext<UserOld>(sqlConection);
        return Json(res.GetAll());
    }

    [Get("CreateUser")]
    public IHttpResponseResult CreateUser()
    {
        var connectionString = @"Data Source = localhost;Initial Catalog=master;User ID=sa;Password=P@ssw0rd;";
        var sqlConection = new SqlConnection(connectionString);
        UserOld testUserOld = new UserOld { Login = "badBoy@gmail.com", Name = "Sergey" };
        var res = new ORMContext<UserOld>(sqlConection);
        res.Create(testUserOld);
        return Json(res.GetAll());
    }
    
    [Get("upuser")]
    public IHttpResponseResult UpdateUser()
    {
        var connectionString = @"Data Source = localhost;Initial Catalog=master;User ID=sa;Password=P@ssw0rd;";
        var sqlConection = new SqlConnection(connectionString);
        UserOld updatedUserOld = new UserOld { Id = 1, Login = "badBoy@gmail.com", Name = "Sergey" };
        var res = new ORMContext<UserOld>(sqlConection);
        res.Update(updatedUserOld);
        var list = new List<UserOld>();
        list.FirstOrDefault();
        return Json(res.GetAll());
    }
    
    [Get("deleteuser")]
    public IHttpResponseResult DeleteUser()
    {
        var connectionString = @"Data Source = localhost;Initial Catalog=master;User ID=sa;Password=P@ssw0rd;";
        var sqlConection = new SqlConnection(connectionString);
        var res = new ORMContext<UserOld>(sqlConection);
        res.Delete(new UserOld { Id = 3, Login = "badBoy@gmail.com", Name = "Sergey" });
        return Json(res.GetAll());
    }
    
    // [Get("user")]
    // public IHttpResponseResult GetUserById(int id)
    // {
    //     var res = new ORMContext(@"Data Source = localhost;Initial Catalog=personsDB;User ID=sa;Password=P@ssw0rd;");
    //     return Json(res.ReadById<User>(id, "Users"));
    // }

    // [Post("user")]
    // public IHttpResponseResult CreateUser(object? requestBody)
    // {
    //     return null;
    // }
}    