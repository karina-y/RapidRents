using RapidRents.Web.Models.Requests.Messaging;
using RapidRents.Web.Models.Responses;
using RapidRents.Web.Services.Messaging;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RapidRents.Web.Services;
using RapidRents.Web.Domain;
using System.Collections.Generic;

namespace RapidRents.Web.Controllers.Api
{
    [RoutePrefix("api/messages")]
    public class MessagingAPIController : ApiController

    {
        public IUserService _userService = null;
        private IMessagingService _service;
        private readonly IMaintenanceRequestServices _MRqstService;

        public MessagingAPIController(IMessagingService svc, IUserService UserService, IMaintenanceRequestServices MRqstService)
        {
            _service = svc;
            _userService = UserService;
            _MRqstService = MRqstService;

        }

        public HttpResponseMessage AddMessage(MessagingAddRequests model)
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

        [Route, HttpPost]
        public HttpResponseMessage SendMessage(MessagingAddRequests model)
        {
            string userId = _userService.GetCurrentUserId();

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            SuccessResponse response = new SuccessResponse();
            _service.SendMessage(model, userId);
            return Request.CreateResponse(response);
        }

        [Route("mrequests/{addressid:int}"), HttpPost]
        public HttpResponseMessage sendMRqstMessage(int AddressId, MessagingAddRequests model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            ItemResponse<List<MaintenanceRequest>> response = new ItemResponse<List<MaintenanceRequest>>();

            response.Item = _MRqstService.GetMaintenanceRqstByAddId(AddressId);
            
            _service.createMtRqstMessage(AddressId, model);

            return Request.CreateResponse(response);
        }
        
    }
}
