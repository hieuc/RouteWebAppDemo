using RoutesWebAppDemo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RoutesWebAppDemo.Controllers
{
    public class GenerateController : Controller
    {
        // GET: Generate
        private static List<Coordinates> cors = new List<Coordinates>();
        private static string url = "";
        private static string json = "";
        private static Route route = new Route();
        private int start = 0;
        private int destination = 1;

        public ActionResult Index()
        {
            var data = new DataPack {url = url, json = json, route = route };
            return View(data);
        }

        public ActionResult List()
        {
            return View(cors); 
        }
        
        public async Task<ActionResult> Regenerate()
        {
            cors = Coordinates.GenerateRandCor(new Coordinates(), 4, 20);
            url = Route.GenerateRequestUrl(cors[destination], cors[start]);
            json = await Route.GetJsonResponse(url);
            route = new Route(json);

            return RedirectToAction("Index");
        }
        
        public async Task<ActionResult> RegenerateCustom(int start = 0, int destination = 1)
        {
            this.start = start;
            this.destination = destination;

            if (cors.Count == 0)
                return RedirectToAction("Regenerate");

            url = Route.GenerateRequestUrl(cors[destination], cors[start]);
            json = await Route.GetJsonResponse(url);
            route = new Route(json);

            return RedirectToAction("Index");
        }
    }
}