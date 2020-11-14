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
            string strConn = Configuration.GetConnectionString("SpaceAndGo_ConnectionString");
            //Instantiate a SqlConnection object with the         
            //Connection String read.          
            conn = new SqlConnection(strConn);   //conn is like the key to database
        }
        
        // Return number of row updated
        public int UpdateCount(LocationData locationData)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE LCount SET LCount = @LCount
                                WHERE LID = @LID";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@LID", locationData.Location);
            cmd.Parameters.AddWithValue("@LCount", locationData.CrowdNow);

            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();

            return count;
        }
    }
}
