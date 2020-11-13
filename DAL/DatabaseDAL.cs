﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpaceAndGo.Models;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;

namespace SpaceAndGo.DAL
{
    public class DatabaseDAL
    {

        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor     
        public DatabaseDAL()
        {
            //Read ConnectionString from appsettings.json file       
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
                "Air_Flights_Management_System_ConnectionString");
            //Instantiate a SqlConnection object with the         
            //Connection String read.          
            conn = new SqlConnection(strConn);   //conn is like the key to database
        }
        
        // Return number of row updated
        public int UpdateCount(LocationData locationData)
        {
            int location = locationData.Location;
            int crowdNow = locationData.CrowdNow;
            
            LocationData result = (from l in Context.Locations
        }
    }
}
