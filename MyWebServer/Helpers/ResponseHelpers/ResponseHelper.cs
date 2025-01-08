using HttpServerLibrary.Models;

namespace my_http.Helpers;

public class ResponseHelper : IResponseHelper
{
    public string GetResponseText(string localPath)
    {
        var filePath = AppConfig.GetInstance().StaticDirectoryPath + localPath;
        if (!File.Exists(filePath))
        {
            filePath = AppConfig.GetInstance().StaticDirectoryPath + "404.html";
            if (!File.Exists(filePath))
            {
                return "error 404 file login.html not found";
            }
        }
        var responseText = File.ReadAllText(filePath);
        return responseText;
    }

    public string GetResponseTextCustomPath(string path)
    {
        if (!File.Exists(path))
        {
            path = AppConfig.GetInstance().StaticDirectoryPath + "404.html";
            if (!File.Exists(path))
            {
                return "error 404 file login.html not found";
            }
        }
        var responseText = File.ReadAllText(path);
        return responseText;
    }
}