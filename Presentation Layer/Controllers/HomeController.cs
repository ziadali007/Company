using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.Models;
using Presentation_Layer.Services;

namespace Presentation_Layer.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IScopedService serviceScope01;
        private readonly IScopedService serviceScope02;
        private readonly ITransientService transientService01;
        private readonly ITransientService transientService02;
        private readonly ISingletonService singletonService01;
        private readonly ISingletonService singletonService02;

        public HomeController(ILogger<HomeController> logger,
            IScopedService serviceScope01,
            IScopedService serviceScope02,
            ITransientService transientService01,
            ITransientService transientService02,
            ISingletonService singletonService01,
            ISingletonService singletonService02)
        {
            _logger = logger;
            this.serviceScope01 = serviceScope01;
            this.serviceScope02 = serviceScope02;
            this.transientService01 = transientService01;
            this.transientService02 = transientService02;
            this.singletonService01 = singletonService01;
            this.singletonService02 = singletonService02;
        }

        public string TestLifeTime()
        {
            StringBuilder result = new StringBuilder();
            result.Append($"ServiceScope01: {serviceScope01.GetGuid()}\n");
            result.Append($"ServiceScope02: {serviceScope02.GetGuid()}\n\n");
            result.Append($"Transient01: {transientService01.GetGuid()}\n");
            result.Append($"Transient02: {transientService02.GetGuid()}\n\n");
            result.Append($"Singleton01: {singletonService01.GetGuid()}\n");
            result.Append($"Singleton02: {singletonService02.GetGuid()}\n\n");
           

            return result.ToString();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
