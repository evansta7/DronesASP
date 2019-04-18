using Drones.Entities;
using Drones.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Drones.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {

        }
        private ApplicationDbContext _applicationDbContext;
        public HomeController(ApplicationDbContext applicationDbContext)
        {
            DbContext = _applicationDbContext;
        }
        public ApplicationDbContext DbContext
        {
            get
            {
                return _applicationDbContext ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            }
            private set
            {
                _applicationDbContext = value;
            }
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            Farm farm = new Farm();
            var farmDetails = DbContext.Farms;
            string markers = "[";

            foreach (Farm item in farmDetails)
            {
                if (item.FarmId == 3)
                {
                    markers += "{";
                    markers += string.Format("'lat': '{0}',", item.Latitude);
                    markers += string.Format("'lng': '{0}',", item.Longitude);
                    markers += "},";
                }
            }
            markers += "];";
            ViewBag.Markers = markers;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}