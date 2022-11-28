using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace YiAim.Cms.Web.Controllers
{
    public class HomeController : Controller
    {
        [Route("/category", Order = 1)]
        [Route("/category/{cid:long}", Order = 2)]
        [Route("/category/{cid:long}/{page:int:min(1)}", Order = 3)]
        public IActionResult Category(long? cid, int page = 1)
        {
            ViewBag.Cid = cid;
            ViewBag.Page = page;
            return View();
        }

        [Route("/news/{id:long}")]
        public async Task<IActionResult> Detail(long id)
        {
           ViewBag.Id = id;
            return View();
        }
    }

}
