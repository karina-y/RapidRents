using System.Collections.Generic;
using System.Data.SqlClient;
using RapidRents.Web.Models.Requests.Company;
using RapidRents.Data;
using System.Data;

namespace RapidRents.Web.Services.Company
{
    public class CompanyService : BaseService, ICompanyService
    {
        public int Insert(CompanyAddRequests model, string userId)
        {
            int id = 0;
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Company_Insert"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@UserId", userId);
                   paramCollection.AddWithValue("@Name", model.Name);
                   paramCollection.AddWithValue("@Url", model.Url);
                   paramCollection.AddWithValue("@Phone", model.Phone);
                   paramCollection.AddWithValue("@Email", model.Email);
                   paramCollection.AddWithValue("@MonOpen", model.MonOpen);
                   paramCollection.AddWithValue("@MonClose", model.MonClose);
                   paramCollection.AddWithValue("@TueOpen", model.TueOpen);
                   paramCollection.AddWithValue("@TueClose", model.TueClose);
                   paramCollection.AddWithValue("@WedOpen", model.WedOpen);
                   paramCollection.AddWithValue("@WedClose", model.WedClose);
                   paramCollection.AddWithValue("@ThuOpen", model.ThuOpen);
                   paramCollection.AddWithValue("@ThuClose", model.ThuClose);
                   paramCollection.AddWithValue("@FriOpen", model.FriOpen);
                   paramCollection.AddWithValue("@FriClose", model.FriClose);
                   paramCollection.AddWithValue("@SatOpen", model.SatOpen);
                   paramCollection.AddWithValue("@SatClose", model.SatClose);
                   paramCollection.AddWithValue("@SunOpen", model.SunOpen);
                   paramCollection.AddWithValue("@SunClose", model.SunClose);

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

        public void Update(CompanyUpdateRequests model)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Company_Update"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Id", model.Id);
                   paramCollection.AddWithValue("@Name", model.Name);
                   paramCollection.AddWithValue("@Url", model.Url);
                   paramCollection.AddWithValue("@Phone", model.Phone);
                   paramCollection.AddWithValue("@Email", model.Email);
                   paramCollection.AddWithValue("@MonOpen", model.MonOpen);
                   paramCollection.AddWithValue("@MonClose", model.MonClose);
                   paramCollection.AddWithValue("@TueOpen", model.TueOpen);
                   paramCollection.AddWithValue("@TueClose", model.TueClose);
                   paramCollection.AddWithValue("@WedOpen", model.WedOpen);
                   paramCollection.AddWithValue("@WedClose", model.WedClose);
                   paramCollection.AddWithValue("@ThuOpen", model.ThuOpen);
                   paramCollection.AddWithValue("@ThuClose", model.ThuClose);
                   paramCollection.AddWithValue("@FriOpen", model.FriOpen);
                   paramCollection.AddWithValue("@FriClose", model.FriClose);
                   paramCollection.AddWithValue("@SatOpen", model.SatOpen);
                   paramCollection.AddWithValue("@SatClose", model.SatClose);
                   paramCollection.AddWithValue("@SunOpen", model.SunOpen);
                   paramCollection.AddWithValue("@SunClose", model.SunClose);
               });
        }

        public List<Domain.Company.Company> GetAll()
        {
            List<Domain.Company.Company> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Company_Select"
               , inputParamMapper: null
               , map: delegate (IDataReader reader, short set)
               {
                   Domain.Company.Company p = new Domain.Company.Company();
                   int startingIndex = 0;

                   p.Id = reader.GetSafeInt32(startingIndex++);
                   p.UserId = reader.GetSafeString(startingIndex++);
                   p.DateAdded = reader.GetSafeDateTime(startingIndex++);
                   p.DateModified = reader.GetSafeDateTime(startingIndex++);
                   p.Name = reader.GetSafeString(startingIndex++);
                   p.Url = reader.GetSafeString(startingIndex++);
                   p.Phone = reader.GetSafeString(startingIndex++);
                   p.Email = reader.GetSafeString(startingIndex++);
                   p.MonOpen = reader.GetSafeString(startingIndex++);
                   p.MonClose = reader.GetSafeString(startingIndex++);
                   p.TueOpen = reader.GetSafeString(startingIndex++);
                   p.TueClose = reader.GetSafeString(startingIndex++);
                   p.WedOpen = reader.GetSafeString(startingIndex++);
                   p.WedClose = reader.GetSafeString(startingIndex++);
                   p.ThuOpen = reader.GetSafeString(startingIndex++);
                   p.ThuClose = reader.GetSafeString(startingIndex++);
                   p.FriOpen = reader.GetSafeString(startingIndex++);
                   p.FriClose = reader.GetSafeString(startingIndex++);
                   p.SatOpen = reader.GetSafeString(startingIndex++);
                   p.SatClose = reader.GetSafeString(startingIndex++);
                   p.SunOpen = reader.GetSafeString(startingIndex++);
                   p.SunClose = reader.GetSafeString(startingIndex++);

                   if (list == null)
                   {
                       list = new List<Domain.Company.Company>();
                   }
                   list.Add(p);
               }
               );
            return list;
        }

        public Domain.Company.Company GetCompanyById(int Id)
        {
            Domain.Company.Company p = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Company_SelectById"
           , inputParamMapper: delegate (SqlParameterCollection paramCollection)
           {
               paramCollection.AddWithValue("@Id", Id);
           }
           , map: delegate (IDataReader reader, short set)
           {
               p = new Domain.Company.Company();
               int startingIndex = 0;

               p.Id = reader.GetSafeInt32(startingIndex++);
               p.UserId = reader.GetSafeString(startingIndex++);
               p.DateAdded = reader.GetSafeDateTime(startingIndex++);
               p.DateModified = reader.GetSafeDateTime(startingIndex++);
               p.Name = reader.GetSafeString(startingIndex++);
               p.Url = reader.GetSafeString(startingIndex++);
               p.Phone = reader.GetSafeString(startingIndex++);
               p.Email = reader.GetSafeString(startingIndex++);
               p.MonOpen = reader.GetSafeString(startingIndex++);
               p.MonClose = reader.GetSafeString(startingIndex++);
               p.TueOpen = reader.GetSafeString(startingIndex++);
               p.TueClose = reader.GetSafeString(startingIndex++);
               p.WedOpen = reader.GetSafeString(startingIndex++);
               p.WedClose = reader.GetSafeString(startingIndex++);
               p.ThuOpen = reader.GetSafeString(startingIndex++);
               p.ThuClose = reader.GetSafeString(startingIndex++);
               p.FriOpen = reader.GetSafeString(startingIndex++);
               p.FriClose = reader.GetSafeString(startingIndex++);
               p.SatOpen = reader.GetSafeString(startingIndex++);
               p.SatClose = reader.GetSafeString(startingIndex++);
               p.SunOpen = reader.GetSafeString(startingIndex++);
               p.SunClose = reader.GetSafeString(startingIndex++);
           });
            return p;
        }

        public Domain.Company.Company DeleteCompanyById(int Id)
        {
            Domain.Company.Company p = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Company_DeleteById"
           , inputParamMapper: delegate (SqlParameterCollection paramCollection)
           {
               paramCollection.AddWithValue("@Id", Id);
           }
           , map: delegate (IDataReader reader, short set)
           {
               p = new Domain.Company.Company();
               int startingIndex = 0;

               p.Id = reader.GetSafeInt32(startingIndex++);
           });
            return p;
        }
    }
}
