using RoutesWebAppDemo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RoutesWebAppDemo.Controllers
{
    public class GenerateController : Controller
    {
        // GET: Generate
        // global fields to display 
        private static List<Coordinates> cors = new List<Coordinates>();
        private static string url = "";
        private static string json = "";
        private static Route route = new Route();
        // indexes for route assessment
        private int start = 0;
        private int destination = 1;

        /*
         * Action for index page.
         */
        public ActionResult Index()
        {
            // display available data.
            var data = new DataPack {url = url, json = json, route = route };
            return View(data);
        }

        /*
         * Action for list of generated coordinates page.
         */
        public ActionResult List()
        {
            return View(cors); 
        }
        
        /*
         * The Regenerate "page" is used to initialize/reinitialize data.
         */
        public async Task<ActionResult> Regenerate()
        {
            cors = Coordinates.GenerateRandCor(new Coordinates(), 4, 20);
            url = Route.GenerateRequestUrl(cors[destination], cors[start]);
            json = await Route.GetJsonResponse(url);
            route = new Route(json);

            return RedirectToAction("Index");
        }
        
        /*
         * Same function with Regenerate, but allow custom points.
         */
        public async Task<ActionResult> RegenerateCustom(int start = 0, int destination = 1)
        {
            this.start = start;
            this.destination = destination;

            // if not initialized
            if (cors.Count == 0)
                return RedirectToAction("Regenerate");

            // reinitialize fields
            url = Route.GenerateRequestUrl(cors[destination], cors[start]);
            json = await Route.GetJsonResponse(url);
            route = new Route(json);

            return RedirectToAction("Index");
        }
    }
}