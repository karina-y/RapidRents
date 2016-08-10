using System.Web.Mvc;
using RapidRents.Web.Models.ViewModels;

namespace RapidRents.Web.Controllers
{
    [RoutePrefix("Company")]
    public class CompanyController : BaseController
    {
        [Route("add")]
        [Route("{id:int}/edit")]
        public ActionResult Add(int? id = null)
        {
            ItemViewModel<int?> model = new ItemViewModel<int?>();
            model.Item = id;

            return View(model);
        }

        [Route("list")]
        public ActionResult List()
        {
            return View();
        }
    }
}
