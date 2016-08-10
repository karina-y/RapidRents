using RapidRents.Web.Domain.File;
using RapidRents.Web.Models;
using RapidRents.Web.Models.Requests.Files;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;

namespace RapidRents.Web.Services
{
    public interface IFilesService
    {
        Task<List<BaseFile>> Upload(InMemoryMultipartStreamProviderService provider);
        int Insert(FileAddRequests model);
        void Update(FileUpdateRequests model);
        List<File> GetAll();
        File GetById(int id);
        File MapFile(IDataReader reader);
        void DeleteById(int id);
        Task<string> AvatarUpload(HttpContent file);
    }
}
