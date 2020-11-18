using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpaceAndGo.Models;
using Firebase.Database;
using Firebase.Database.Query;

namespace SpaceAndGo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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

        //Testing

        //Edit to DB 
        //https://developer.okta.com/blog/2019/04/30/store-data-firebase-aspnet-mvc-csharp
        public async Task<ActionResult> About()
        {
            //Simulate test user data and login timestamp
            var userId = "123456";
            var currentLoginTime = DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss");

            //Save non identifying data to Firebase
            var currentUserLogin = new LoginData() { TimestampUtc = currentLoginTime };
            var firebaseClient = new FirebaseClient("https://spaceandscan.firebaseio.com/");
            var result = await firebaseClient
              .Child("Users/" + userId + "/Logins")
              .PostAsync(currentUserLogin);

            //Retrieve data from Firebase
            var dbLogins = await firebaseClient
              .Child("Users")
              .Child(userId)
              .Child("Logins")
              .OnceAsync<LoginData>();

            var timestampList = new List<DateTime>();

            //Convert JSON data to original datatype
            foreach (var login in dbLogins)
            {
                timestampList.Add(Convert.ToDateTime(login.Object.TimestampUtc).ToLocalTime());
            }

            //Pass data to the view
            ViewBag.CurrentUser = userId;
            ViewBag.Logins = timestampList.OrderByDescending(x => x);
            return View();
        }

        public async Task<ActionResult> ViewMap()
        {
            //SEND DATA

            /*var location = "Block 2";
            var crowd = 20;
            var currentUserLogin = new LocationData() { CrowdNow = crowd ,Location = location };
            var firebaseClient = new FirebaseClient("https://spaceandscan.firebaseio.com/");
            var result = await firebaseClient
            .Child("Location1/")
            .PostAsync(currentUserLogin);*/

            var firebaseClient = new FirebaseClient("https://spaceandscan.firebaseio.com/"); //USE FOR LINKING TO DATABASE



            //Retrieve data from Firebase
            var dbLogins = await firebaseClient
              .Child("Location")
              .OnceAsync<LocationData>();
            var locationList = new List<string>(); //DIFFERENT WAYS OF RETRIEVING DATA
            var crowdList = new List<int>();
            var objectList = new List<object>();
            foreach (var login in dbLogins)
            {
                objectList.Add((login.Object));
                //crowdList.Add((login.Object.CrowdNow));
                //locationList.Add((login.Object.Location));
            }
            //Pass data to the view
            ViewBag.Location = locationList;
            ViewBag.Crowd = crowdList;
            ViewBag.Object = objectList;
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }
        public async Task<ActionResult> Counter()
        {
            var firebaseClient = new FirebaseClient("https://spaceandscan.firebaseio.com/"); //USE FOR LINKING TO DATABASE
            
            //Retrieve data from Firebase
            var crowddatanow = await firebaseClient
              .Child("Location")
              .OnceAsync<LocationData>();
            var locationList = new List<int>(); //DIFFERENT WAYS OF RETRIEVING DATA
            var crowdList = new List<int>();
            var objectList = new List<object>();
            foreach (var data in crowddatanow)
            {
                objectList.Add((data.Object));
            }
            //Pass data to the view
            ViewBag.Location = locationList;
            ViewBag.Crowd = crowdList;
            ViewBag.Data = objectList;
            return View();

        }
        public ActionResult News()
        {
            return View();
        }

    }
}
