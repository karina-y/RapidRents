using System.Web.Mvc;

namespace RapidRents.Web.Controllers
{
    [Authorize]
    [RoutePrefix("files")]
    public class FilesController : BaseController
    {
        [Route("gallery")]
        public ActionResult Files()
        {
            return View();
        }
    }
}
