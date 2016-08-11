using System.Web.Mvc;
using RapidRents.Web.Models.ViewModels;

namespace RapidRents.Web.Controllers
{
    [RoutePrefix("Contact")]
    public class MessagingController : BaseController
    {
        [Route]
        public ActionResult Messages(int? id = null)
        {
            ItemViewModel<int?> model = new ItemViewModel<int?>();
            model.Item = id;

            return View(model);
        }
    }
}
