using Drones.Entities;
using Drones.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Drones.Controllers
{
    public class WeatherAPIController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _applicationDbContext;

        const string apiKey = "514f076158bda071bc7bd357e5ee4819";

        public WeatherAPIController()
        {

        }
        public WeatherAPIController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationDbContext applicationDbContext)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            DbContext = _applicationDbContext;
        }

        private const string COUNTRY_CODE = "za";
        private const string MEASUREMENT_UNITS = "metric";

        // GET: WeatherAPI
        public ViewResult WeatherWidget()
        {
            ResponseWeatherViewModel weatherView = GetWeatherAPI();
            return View(weatherView);
        }

     
        //TODO Farmer can have multiple farms, we have to redo this method in the future
        private string GetFarmerZipCode() {
            string userId = User.Identity.GetUserId();
            var farmerDetail = DbContext.Farmer.Include("Farms").Where(x => x.UserId.Equals(userId)).FirstOrDefault();
            var farm = DbContext.Farms.Where(x => x.FarmId.Equals(farmerDetail.Farms.FarmId)).FirstOrDefault();
         
            return farm.PostalCode;
        }

        public ResponseWeatherViewModel GetWeatherAPI()
        {
         
                HttpWebRequest apiRequest =WebRequest.Create("https://api.openweathermap.org/data/2.5/weather?zip=" + GetFarmerZipCode() + "," + COUNTRY_CODE + "&appid="+apiKey+"&units=" + MEASUREMENT_UNITS) as HttpWebRequest;

                string apiResponse = "";
                using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    apiResponse = reader.ReadToEnd();
                }
                ResponseWeatherViewModel responseWeatherViewModel = JsonConvert.DeserializeObject<ResponseWeatherViewModel>(apiResponse);

            return responseWeatherViewModel;
           
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
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

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
    }
}