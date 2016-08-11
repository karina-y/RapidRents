using Newtonsoft.Json;
using RapidRents.Web.Domain.Address;
using RapidRents.Web.Domain.Listings;
using RapidRents.Web.Models.Requests.CompareListings;
using RapidRents.Web.Models.Requests.Listings;
using RapidRents.Web.Models.Responses;
using RapidRents.Web.Services;
using RapidRents.Web.Services.Listings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace RapidRents.Web.Controllers.Api
{
    [RoutePrefix("api/listings")]
    public class ListingsApiController : ApiController
    {
        private IListingsService _listingService = null;
        public IUserService _userService = null;

        public ListingsApiController(IListingsService listingService, IUserService UserService)
        {
            _listingService = listingService;
            _userService = UserService;
        }

        [Route, HttpPost]
        public HttpResponseMessage EchoSearchValidation(ListingsAddRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ItemResponse<int> response = new ItemResponse<int>();

            response.Item = _listingService.InsertNewListing(model);

            return Request.CreateResponse(response);
        }

        [Route("Utilities"), HttpGet]
        public HttpResponseMessage GetListingUtilities()
        {
            ItemResponse<KeyValuePair<int, string>[]> response = new ItemResponse<KeyValuePair<int, string>[]>();

            response.Item = UtilityTypes.Electricity.ToJsonDictionary();

            return Request.CreateResponse(response);
        }

        [Route("{id:int}"), HttpPut]
        public HttpResponseMessage UpdateListingsRecord(ListingsUpdateRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            _listingService.UpdateListingsRecord(model);

            SuccessResponse response = new SuccessResponse();

            return Request.CreateResponse(response);
        }

        [Route(), HttpGet]
        public HttpResponseMessage GetListingsRecords()
        {
            ItemsResponse<Listing> response = new ItemsResponse<Listing>();

            response.Items = _listingService.GetAllListings();

            return Request.CreateResponse(response);
        }

        [Route("featured"), HttpGet]
        public HttpResponseMessage GetFeatured()
        {
            ItemsResponse<Listing> response = new ItemsResponse<Listing>();

            response.Items = _listingService.GetFeatured();

            return Request.CreateResponse(response);
        }

        [Route("latest"), HttpGet]
        public HttpResponseMessage GetLatest()
        {
            ItemsResponse<Listing> response = new ItemsResponse<Listing>();

            response.Items = _listingService.GetLatest();

            return Request.CreateResponse(response);
        }

        [Route("{id:int}"), HttpGet]
        public HttpResponseMessage GetSearchById(int id)
        {
            ItemResponse<Listing> response = new ItemResponse<Listing>();

            response.Item = _listingService.GetListingsById(id);

            return Request.CreateResponse(response);
        }

        [Route("address/{id:int}"), HttpGet]
        public HttpResponseMessage GetListingById(int id)
        {
            ItemResponse<Listing> response = new ItemResponse<Listing>();

            response.Item = _listingService.GetListingByAddressId(id);

            return Request.CreateResponse(response);
        }

        [Route("id/{id:int}"), HttpGet]
        public HttpResponseMessage GetListingByLiId(int id)
        {
            ItemResponse<Listing> response = new ItemResponse<Listing>();

            response.Item = _listingService.GetListingByLiId(id);

            return Request.CreateResponse(response);
        }

        [Route("{id:int}"), HttpDelete]
        public HttpResponseMessage DeleteListingsById(int id)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ItemResponse<int> response = new ItemResponse<int>();

            _listingService.DeleteListingsById(id);

            return Request.CreateResponse(response);
        }

        [Route("compare"), HttpPost]
        public HttpResponseMessage SetCookie(CompareListingsAddRequest model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            HttpResponseMessage resp = Request.CreateResponse(HttpStatusCode.OK, new SuccessResponse());

            List<string> listOfListingIds = DeserializeCookie();

            if (listOfListingIds.Count > 2)
            {
                ErrorResponse errResponse = new ErrorResponse("Too Many Listings added to compare");

                return Request.CreateResponse(errResponse);
            }

            listOfListingIds.Add(model.ListingId);
            string listingIds = JsonConvert.SerializeObject(listOfListingIds);
            CookieHeaderValue cookie = new CookieHeaderValue("listing-ids", listingIds)
            {
                Expires = DateTime.Now.AddDays(1d),
                Domain = Request.RequestUri.Host,
                Path = "/"
            };

            resp.Headers.AddCookies(new CookieHeaderValue[] { cookie });

            return resp;
        }

        public List<string> GetCookiesList()
        {
            string listingId = "";
            CookieHeaderValue cookie = Request.Headers.GetCookies("listing-ids").FirstOrDefault();

            if (cookie != null)
            {
                listingId = cookie["listing-ids"].Value;
            }

            List<string> newCookieList = JsonConvert.DeserializeObject<List<string>>(listingId);

            return newCookieList;
        }

        [Route("compare"), HttpDelete]
        public HttpResponseMessage DeleteAllCookie()
        {
            HttpResponseMessage resp = Request.CreateResponse(HttpStatusCode.OK, new SuccessResponse());

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            HttpCookie myCookie = new HttpCookie("listing-ids");
            CookieHeaderValue cookie = new CookieHeaderValue("listing-ids", string.Empty);
            myCookie.Expires = DateTime.Now.AddYears(-1);
            cookie.Expires = DateTime.Now.AddYears(-1);

            resp.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            return resp;
        }

        [Route("compare/{id:int}"), HttpDelete]
        public HttpResponseMessage DeleteCookieById(int id)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            HttpResponseMessage resp = Request.CreateResponse(HttpStatusCode.OK, new SuccessResponse());

            List<string> listOfListingIds = DeserializeCookie();

            var indexToRemove = -1;

            for (var i = 0; i < listOfListingIds.Count; i++)
            {
                if (listOfListingIds[i] == id.ToString())
                {
                    indexToRemove = i;
                }
            }

            if (indexToRemove != -1)
            {
                listOfListingIds.RemoveAt(indexToRemove);
            }

            string listingIds = JsonConvert.SerializeObject(listOfListingIds);
            CookieHeaderValue cookie = new CookieHeaderValue("listing-ids", listingIds)
            {
                Expires = DateTime.Now.AddDays(1),
                Domain = Request.RequestUri.Host,
                Path = "/"
            };

            resp.Headers.AddCookies(new CookieHeaderValue[] { cookie });

            return resp;
        }

        public List<string> DeserializeCookie()
        {
            string myCookie = string.Empty;
            CookieHeaderValue selectedCookie = Request.Headers.GetCookies("listing-ids").FirstOrDefault() ?? new CookieHeaderValue("listing-ids", string.Empty);

            if (selectedCookie == null)
            {
                myCookie = string.Empty;
            }
            else if (selectedCookie != null)
            {
                myCookie = selectedCookie["listing-ids"].Value;
            }

            List<string> newCookie = JsonConvert.DeserializeObject<List<string>>(myCookie) ?? new List<string>();

            return newCookie;
        }

        [Route("compare"), HttpGet]
        public HttpResponseMessage GetCompareListings()
        {
            List<string> listOfIdsInTheCookie = GetCookiesList();

            if (listOfIdsInTheCookie == null)
            {
                ErrorResponse errResponse = new ErrorResponse("There are no Id's in this cookie");

                return Request.CreateResponse(errResponse);
            }

            List<Listing> listOfListings = new List<Listing>();

            foreach (string id in listOfIdsInTheCookie)
            {
                int listingId = Convert.ToInt32(id);
                listOfListings.Add(_listingService.GetListingsById(listingId));
            }

            ItemsResponse<Listing> response = new ItemsResponse<Listing>();

            response.Items = listOfListings;

            return Request.CreateResponse(response);
        }

        [Route("search/cost"), HttpPost]
        public HttpResponseMessage GetSearchByRentCost(ListingsSearchRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            ItemsResponse<Listing> response = new ItemsResponse<Listing>();

            response.Items = _listingService.GetListingsByRentCost(model);

            return Request.CreateResponse(response);
        }

        [Route("search/availabledate"), HttpPost]
        public HttpResponseMessage GetSearchByAvailabilityDate(ListingsSearchRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            ItemsResponse<Listing> response = new ItemsResponse<Listing>();

            response.Items = _listingService.GetListingsByAvailabilityDate(model);

            return Request.CreateResponse(response);
        }

        [Route("search/hasparking"), HttpPost]
        public HttpResponseMessage GetSearchByHasParking(ListingsSearchRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            ItemsResponse<Listing> response = new ItemsResponse<Listing>();

            response.Items = _listingService.GetListingsByHasParking(model);

            return Request.CreateResponse(response);
        }

        [Route("search/leaseterms"), HttpPost]
        public HttpResponseMessage GetSearchByLeaseTerms(ListingsSearchRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            ItemsResponse<Listing> response = new ItemsResponse<Listing>();

            response.Items = _listingService.GetListingsByLeaseTerms(model);

            return Request.CreateResponse(response);
        }

        [Route("AddressListing"), HttpPost]
        public HttpResponseMessage PLW_insert(PLWAddRequest model)
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ItemResponse<int> response = new ItemResponse<int>();

            response.Item = _listingService.PLW_Insert(model);

            return Request.CreateResponse(response);
        }

        [Route("favorite"), HttpGet]
        public HttpResponseMessage GetFavoritelisting()
        {
            string UserId = _userService.GetCurrentUserId();

            ItemResponse<PagedList<Listing>> response = new ItemResponse<PagedList<Listing>>();

            response.Item = _listingService.GetFavoriteListings(UserId);

            return Request.CreateResponse(response);
        }

        [Route("{PageIndex:int}/{PageSize:int}"), HttpGet]
        public HttpResponseMessage Get(int PageIndex, int PageSize)
        {

            ItemResponse<PagedList<Listing>> response = new ItemResponse<PagedList<Listing>>();

            response.Item = _listingService.GetAllPropListings(PageIndex, PageSize);

            return Request.CreateResponse(response);
        }

        [Route("maplist/{PageIndex:int}/{PageSize:int}"), HttpGet]
        public HttpResponseMessage getMap(int PageIndex, int PageSize)
        {
            ItemResponse<PagedList<Address>> response = new ItemResponse<PagedList<Address>>();

            response.Item = _listingService.GetAllMapListingsAddresses(PageIndex, PageSize);

            return Request.CreateResponse(response);
        }

        [Route("popularlistings/{Latitude:decimal}/{Longitude:decimal}/{Miles:int}"), HttpGet]
        public HttpResponseMessage getPopularListing(decimal Latitude, decimal Longitude, int Miles)
        {
            ItemResponse<List<Address>> response = new ItemResponse<List<Address>>();

            response.Item = _listingService.GetPopularListingsAddressesMap(Latitude, Longitude, Miles);

            return Request.CreateResponse(response);
        }
    }
}
