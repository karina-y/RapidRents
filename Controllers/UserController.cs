using RapidRents.Web.Models.ViewModels;
using System.Web.Mvc;

namespace RapidRents.Web.Controllers
{
    [RoutePrefix("Users")]
    public class UserController : BaseController
    {
        [Route("add")]
        [Route("{id:int}/edit")]
        public ActionResult Index(int? id=null)
        {
            ItemViewModel<int?> model = new ItemViewModel<int?>();
            model.Item = id;

            return View(model);
        }

        [Route]
        public ActionResult List()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        [Route("Dashboard")]
        public ActionResult Dashboard()
        {
            return View();
        }

        [Authorize]
        [Route("CustomUI")]
        public ActionResult CustomUI(int? id = null)
        {
            ItemViewModel<int?> model = new ItemViewModel<int?>();
            model.Item = id;

            return View(model);
        }

        [Authorize(Roles = "SuperAdmin")]
        [Route("adminhome")]
        public ActionResult AdminHome(int? id = null)
        {
            ItemViewModel<int?> model = new ItemViewModel<int?>();
            model.Item = id;

            return View(model);
        }
    }
}
