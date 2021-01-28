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

namespace ScheduledTask.Models
{
    public class Jobclass : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            using (var message = new MailMessage("testuser@gmail.com", "testdestinationmail@gmail.com"))
            {
                message.Subject = "Message Subject test";
                message.Body = "Message body test at " + DateTime.Now;
                using (SmtpClient client = new SmtpClient
                {
                    EnableSsl = true,
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Credentials = new NetworkCredential("testuser@gmail.com", "123546")
                })
                {
                    client.Send(message);
                }
            }
        }
    }

}