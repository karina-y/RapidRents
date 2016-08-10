using System.IO;

namespace RapidRents.Web.Services
{
    public interface IStorageService
    {
        string GeneratePreSignedURL(string key, int expireInSeconds);
        bool UploadFile(string key, Stream stream);
        bool DeleteFile(string key);
    }
}
