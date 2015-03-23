using SiteP.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace SiteP.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            FederatedPassiveSecurityTokenServiceOperations.ProcessRequest(
                System.Web.HttpContext.Current.Request,
                User as ClaimsPrincipal,
                CustomSecurityTokenServiceConfiguration.Current.CreateSecurityTokenService(),
                System.Web.HttpContext.Current.Response);
            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}