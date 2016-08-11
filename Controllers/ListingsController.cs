using RapidRents.Web.Models.ViewModels;
using System.Web.Mvc;

namespace RapidRents.Web.Controllers
{

    [RoutePrefix("listings")]
    public class ListingsController : BaseController
    {
        [Authorize]
        [Route("create")]
        [Route("{id:int}/edit")]
        public ActionResult AddListings(int? id = null)
        {
            ItemViewModel<int?> model = new ItemViewModel<int?>();

            model.Item = id;
            return View(model);
        }

        [Route ("{listingId:int}")]
        [Route("address/{addressId:int}")]
        public ActionResult ListingDetails(int addressId = 0, int listingId = 0)
        {
            ListingDetailViewModel model = GetViewModel<ListingDetailViewModel>();

            model.AddressId = addressId;
            model.ListingId = listingId;
            return View(model);
        }

        public ActionResult CompareListings()
        {
            return View();
        }

        public ActionResult CookieTester()
        {
            return View();
        }

        [Route("Reports")] 
        public ActionResult ListingReports()
        {
            return View();
        }
    }
}
