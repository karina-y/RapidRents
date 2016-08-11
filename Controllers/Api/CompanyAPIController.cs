using RapidRents.Web.Domain.Company;
using RapidRents.Web.Models.Requests.Company;
using RapidRents.Web.Models.Responses;
using RapidRents.Web.Services.Company;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace RapidRents.Web.Controllers.Api
{
    [RoutePrefix("api/company")]
    public class CompanyAPIController : ApiController
    {        
        private ICompanyService _service;
        public Services.IUserService _userService = null;
        public CompanyAPIController(ICompanyService svc, Services.IUserService UserService)
        {
            _service = svc;
            _userService = UserService;
        }

        [Route, HttpPost]
        public HttpResponseMessage AddCompany(CompanyAddRequests model)
        {
            string userId = _userService.GetCurrentUserId();
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            ItemResponse<int> response = new ItemResponse<int>();

            response.Item = _service.Insert(model, userId);
            return Request.CreateResponse(response);
        }

        [Route("{id:int}"), HttpPut]
        public HttpResponseMessage UpdateCompany(CompanyUpdateRequests model, int id)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            _service.Update(model);
            SuccessResponse response = new SuccessResponse();
            return Request.CreateResponse(response);

        }


        [Route, HttpGet]
        public HttpResponseMessage GetCompanies()
        {
            ItemsResponse<Company> response = new ItemsResponse<Company>();
            response.Items = _service.GetAll();
            return Request.CreateResponse(response);
        }


        [Route("{id:int}"), HttpDelete]
        public HttpResponseMessage DeleteCompany(int Id)
        {
            ItemResponse<Company> response = new ItemResponse<Company>();

            response.Item = _service.DeleteCompanyById(Id);

            return Request.CreateResponse(response);
        }

        [Route("{id:int}"), HttpGet]
        public HttpResponseMessage GetCompany(int Id)
        {
            ItemResponse<Company> response = new ItemResponse<Company>();

            response.Item = _service.GetCompanyById(Id);

            return Request.CreateResponse(response);
        }
    }
}
