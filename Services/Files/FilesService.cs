using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using System.Net.Http.Headers;
using System.Data.SqlClient;
using System.Data;
using RapidRents.Web.Models.Requests.Files;
using RapidRents.Web.Models;
using System.IO;
using RapidRents.Data;
using RapidRents.Web.Domain.File;
using RapidRents.Web.Domain;

namespace RapidRents.Web.Services
{
    public class FilesService : BaseService, IFilesService
    {
        private readonly IStorageService _service;
        private readonly IUserService _userService;
        public FilesService(IStorageService svc, IUserService services)
        {
            _service = svc;
            _userService = services;
        }

        public int Insert(FileAddRequests model)
        {
            int id = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.FileService_Insert"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@FilePath", model.FilePath);
                   paramCollection.AddWithValue("@FileName", model.FileName);
                   paramCollection.AddWithValue("@UserId", _userService.GetCurrentUserId());

                   SqlParameter p = new SqlParameter("@Id", SqlDbType.Int);
                   p.Direction = ParameterDirection.Output;

                   paramCollection.Add(p);

               }, returnParameters: delegate (SqlParameterCollection param)
               {
                   int.TryParse(param["@id"].Value.ToString(), out id);
               }
               );
            return id;
        }

        public async Task<List<BaseFile>> Upload(InMemoryMultipartStreamProviderService provider)
        {
            int counter = provider.Files.Count;
            List<BaseFile> fileNames = new List<BaseFile>();

            for (int i = 0; i < counter; i++)
            {
                FileAddRequests model = new FileAddRequests();
                HttpContent file = provider.Files[i];
                string fileName = await ProcessFile(file);
                model.FileName = fileName;
                model.FilePath = "C17/" + fileName;

                int messageId = Insert(model);

                BaseFile fileObj = new BaseFile();
                fileObj.Id = messageId;
                fileObj.FilePath = model.FilePath;

                fileNames.Add(fileObj);
            }

            return fileNames;
        }

        private async Task<string> ProcessFile(HttpContent file)
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 99999999);
            string rawFileName = file.Headers.ContentDisposition.FileName;
            MediaTypeHeaderValue type = file.Headers.ContentType;
            string fileName = randomNumber + "_" + rawFileName.Replace("\"", string.Empty);
            Stream fileStream = await file.ReadAsStreamAsync();
            _service.UploadFile(fileName, fileStream);
            return fileName;
        }

        public void Update(FileUpdateRequests model)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.FileService_Update"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@FilePath", model.FilePath);
                   paramCollection.AddWithValue("@FileName", model.FileName);
                   paramCollection.AddWithValue("@Id", model.Id);
               });
        }

        public List<Domain.File.File> GetAll()
        {
            List<Domain.File.File> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.FileService_Select"
               , inputParamMapper: null
               , map: delegate (IDataReader reader, short set)
               {
                   Domain.File.File p = MapFile(reader);

                   if (list == null)
                   {
                       list = new List<Domain.File.File>();
                   }

                   list.Add(p);
               }
               );
            return list;
        }

        public Domain.File.File GetById(int id)
        {
            Domain.File.File File = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.FileService_Select_ById"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Id", id);
               }
               , map: delegate (IDataReader reader, short set)
               {
                   File = MapFile(reader);
               }
               );
            return File;
        }

        public Domain.File.File MapFile(IDataReader reader)
        {
            Domain.File.File File = new Domain.File.File();
            int startingIndex = 0;

            File.Id = reader.GetSafeInt32(startingIndex++);
            File.FilePath = reader.GetSafeString(startingIndex++);
            File.FileName = reader.GetSafeString(startingIndex++);
            File.DateAdded = reader.GetSafeDateTime(startingIndex++);
            File.DateModified = reader.GetSafeDateTime(startingIndex++);
            File.UserId = reader.GetSafeString(startingIndex++);
            return File;
        }

        public void DeleteById(int id)
        {
            Domain.File.File getFile = GetById(id);
            string filePath = getFile.FilePath;

            _service.DeleteFile(filePath);

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.FileService_Delete_ById"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Id", id);
               });
        }

        public async Task<string> AvatarUpload(HttpContent file)
        {
            string fileName = await ProcessFile(file);
            string filePath = "C17/" + fileName;

            string userId = _userService.GetCurrentUserId();
            User user = _userService.GetUser(userId);

            if (user != null)
            {
                UpdateProfilePhoto(filePath);
            }
            else
            {
                InsertUserByPhoto(userId, filePath);
            }
            return filePath;
        }

        public void UpdateProfilePhoto(string filePath)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Users_UpdateAvatarPhotobyAspNetUserId"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@AspNetUserId", _userService.GetCurrentUserId());
                   paramCollection.AddWithValue("@AvatarFilePath", filePath);
               });
        }


        public string InsertUserByPhoto(string userId, string filePath)
        {
            //int id = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Users_InsertAvatarFilePath"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   if (userId != null && filePath !=null)
                   {
                       paramCollection.AddWithValue("@AspNetUserId", userId);
                       paramCollection.AddWithValue("@AvatarFilePath", filePath);
                   }

                   SqlParameter p = new SqlParameter("@Id", System.Data.SqlDbType.Int);
                   p.Direction = System.Data.ParameterDirection.Output;

                   paramCollection.Add(p);
               });

            return filePath;
        }
    }
}
