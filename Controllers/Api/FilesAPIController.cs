using System.Collections.Generic;
using System.Web.Http;
using RapidRents.Web.Services;
using System.Net.Http;
using System.Threading.Tasks;
using RapidRents.Web.Models.Responses;
using System.Net;
using RapidRents.Web.Domain.File;
using RapidRents.Web.Models.Requests.Files;
using RapidRents.Web.Models.Requests.Users;

namespace RapidRents.Web.Controllers.Api
{
    [RoutePrefix("api/files")]
    public class FilesApiController : ApiController
    {
        private IFilesService _service;
        public FilesApiController(IFilesService svc)
        {
            _service = svc;
        }

        [Route("upload"), HttpPost]
        public async Task<HttpResponseMessage> Upload()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            InMemoryMultipartStreamProviderService provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartStreamProviderService>(new InMemoryMultipartStreamProviderService());

            ItemResponse<List<BaseFile>> response = new ItemResponse<List<BaseFile>>();

            response.Item = await _service.Upload(provider);
            return Request.CreateResponse(response);
        }

        [Route("{id:int}"), HttpPut]
        public HttpResponseMessage Update(FileUpdateRequests model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            SuccessResponse response = new SuccessResponse();

            _service.Update(model);

            return Request.CreateResponse(response);
        }

        [Route, HttpGet]
        public HttpResponseMessage Get()
        {
            ItemsResponse<File> response = new ItemsResponse<File>();

            response.Items = _service.GetAll();

            return Request.CreateResponse(response);
        }

        [Route("{id:int}"), HttpGet]
        public HttpResponseMessage GetById(int id)
        {
            ItemResponse<File> response = new ItemResponse<File>();

            response.Item = _service.GetById(id);

            return Request.CreateResponse(response);
        }

        [Route("{id:int}"), HttpDelete]
        public HttpResponseMessage DeleteById(int id)
        {
            _service.DeleteById(id);

            SuccessResponse response = new SuccessResponse();

            return Request.CreateResponse(response);
        }

        [Route("avatar/upload"), HttpPut]
        public async Task<HttpResponseMessage> AvatarUpload()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            InMemoryMultipartStreamProviderService provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartStreamProviderService>(new InMemoryMultipartStreamProviderService());
            HttpContent file = provider.Files[0];
            
            ItemResponse<string> response = new ItemResponse<string>();

            response.Item = await _service.AvatarUpload(file);

            return Request.CreateResponse(response);
        }
    }
}
