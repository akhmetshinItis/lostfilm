using HttpServerLibrary;
using HttpServerLibrary.Core.Attributes;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using Microsoft.Data.SqlClient;
using my_http.Helpers;
using my_http.Helpers.AuthHelpers;
using my_http.Models;
using MyORMLibrary;
using TemlateEngine;

namespace my_http.Endpoints;

public class WalletEndpoint : EndpointBase
{
    private ResponseHelper _responseHelper = new ResponseHelper();
    private AuthorizationHelper _authorizationHelper = new AuthorizationHelper();

    
    [Get("wallet")]
    public IHttpResponseResult Index()
    {
        var localPath = "Views/Templates/Pages/wallet/my-wallet.html";
        var responseText = _responseHelper.GetResponseTextCustomPath(localPath);
        if (!_authorizationHelper.IsAuthorized(Context)) // Используем метод проверки авторизации
        {
            return Redirect("auth/login");
        }
        
        var templateEngine = new HtmlTemplateEngine();
        var userId = Int32.Parse(SessionStorage.GetUserId(Context.Request.Cookies["session-token"].Value));
        
        var connectionString = AppConfig.GetInstance().ConnectionString;
        using var sqlConnection = new SqlConnection(connectionString);
        
        var dbContextWallet = new ORMContext<Wallet>(sqlConnection);
        var wallet = dbContextWallet.FirstOrDefault(w => w.UserId == userId);
        if (wallet is null)
        {
            return Redirect("dashboard");
        }
        var dbContextUser = new ORMContext<UserOld>(sqlConnection);
        var user = dbContextUser.GetById(userId);
        var walletVm = new WalletVm { Balance = (int)wallet.Balance, UserName = user.Name };
        var text = templateEngine.Render(responseText, walletVm);
        
        return Html(text);
    }
}