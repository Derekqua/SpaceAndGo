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
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;

using Quartz;
using Quartz.Impl;
using ScheduledTask.Models;

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
            var firebaseClient = new FirebaseClient("https://spaceandgo-938a9.firebaseio.com/");
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

            var firebaseClient = new FirebaseClient("https://spaceandgo-938a9.firebaseio.com/"); //USE FOR LINKING TO DATABASE



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

        public async Task<ActionResult> Details(int i)
        {
            var firebaseClient = new FirebaseClient("https://spaceandgo-938a9.firebaseio.com/"); //USE FOR LINKING TO DATABASE
            var dbLogins = await firebaseClient
              .Child("Location")
              .OnceAsync<LocationData>();
            var objectList = new List<object>();
            foreach (var login in dbLogins)
            {
                objectList.Add((login.Object));
            }
            ViewBag.Object = objectList;
            ViewBag.selected = objectList[i];
            return View();
        }


        public ActionResult AboutUs()
        {
            return View();
        }
        public async Task<ActionResult> Counter()
        {
            var firebaseClient = new FirebaseClient("https://spaceandgo-938a9.firebaseio.com/"); //USE FOR LINKING TO DATABASE
            
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
        public async Task<ActionResult> News()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://covid-19-news.p.rapidapi.com/v1/covid?q=covid&lang=en&media=True"),
                Headers =
                {
                    { "x-rapidapi-key", "c947943dd5mshd3da70c50ce7bb9p175bd6jsndb5705073769" },
                    { "x-rapidapi-host", "covid-19-news.p.rapidapi.com" },
                },
            };
            
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Rootobject rootdata = JsonConvert.DeserializeObject<Rootobject>(body);
                Article[] articles = rootdata.articles;

                
                return View(articles);

            }
            
        }
        [HttpGet]
        public ActionResult Email()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Email(Form email)
        {

            if (ModelState.IsValid)
            {
                Article[] articles = { };
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://covid-19-news.p.rapidapi.com/v1/covid?q=covid&lang=en&media=True"),
                    Headers =
                {
                    { "x-rapidapi-key", "c947943dd5mshd3da70c50ce7bb9p175bd6jsndb5705073769" },
                    { "x-rapidapi-host", "covid-19-news.p.rapidapi.com" },
                },
                };

                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    Rootobject rootdata = JsonConvert.DeserializeObject<Rootobject>(body);
                    articles = rootdata.articles;
                }

                //setting content in email
                var content = "";
                var num = 1;
                foreach (var i in articles)
                {
                    content += "<h1>" + num + ". " + i.title + "</h1><br>";
                    content += "<a href=" + i.link + "><img src=" + i.media + " /></a>";
                    content += i.summary;
                    num += 1;
                }

                //sending email
                var message = new MailMessage();
                message.To.Add(new MailAddress(email.FromEmail));  // replace with valid value 
                message.From = new MailAddress("derekqua8@gmail.com");  // replace with valid value
                message.Subject = "Space & Go";
                message.Body = content;
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "spacengo198@gmail.com",  // replace with valid value
                        Password = "Space198!Go"  // replace with valid value
                    };
                 
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    TempData["errorMessage"] = "Email Sent Successfuly. You will receive news daily.";
                    return View();
                }
            }
            else
            {
                // Store an error message in TempData for display at the CreateRoute view     
                TempData["errorMessage"] = "Failed to send an Email, Please Try Again!";

                //Input validation fails, return to the Create view
                //to display error message
                return View(email);
            }
        }

        public static void Start()
        {
            IScheduler scheduler = (IScheduler)StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<Jobclass>().Build();

            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("trigger1", "group1")
            .StartNow()
            .WithSimpleSchedule(x => x
            .WithIntervalInSeconds(10)
            .RepeatForever())
            .Build();

            scheduler.ScheduleJob(job, trigger);
        }



    }
}
