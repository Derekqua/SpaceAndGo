using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using System.Net;
using System.Net.Mail;
using SpaceAndGo.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Firebase.Database;
using Firebase.Database.Query;


namespace ScheduledTask.Models
{
    public class Jobclass : IJob
    {
        public async Task Execute(IJobExecutionContext context)
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

            //retrieving email from firebase
            var firebaseClient = new FirebaseClient("https://spaceandgo-938a9.firebaseio.com/"); //USE FOR LINKING TO DATABASE

            //Retrieve data from Firebase
            var dbLogins = await firebaseClient
              .Child("Email")
              .OnceAsync<Form>();
            var emailList = new List<string>();
            foreach (var login in dbLogins)
            {
                emailList.Add((login.Object.FromEmail));
            }

            //sending email
            var message = new MailMessage();
            foreach (string i in emailList) {
                message.To.Add(new MailAddress(i));
            }
            
            message.From = new MailAddress("spancengo198@gmail.com");  // replace with valid value
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
            }
                
        }
    }

}