using System.Web.Mvc;

namespace Mvc.Jsonp.Examples.Controllers
{
    using System.Collections.Generic;
    using Models;

    public class HomeController : JsonpControllerBase
    {
        public JsonpResult Index(string callback = "processStarships")
        {
            var starships = new List<Starship>
            {
                new Starship { Name = "Enterprise", Registry = "NCC-1701" },
                new Starship { Name = "Galaxy", Registry = "NCC-70637" },
                new Starship { Name = "Tikopai", Registry = "NCC-1800" }
            };

            return this.Jsonp(starships, callback, JsonRequestBehavior.AllowGet);
        }
    }
}
