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
        //Create Location stored procedure Inputs
        //uspCreateLocation (@LName, @LAddress, @PostalCode, @ContactNo)

        //Update Crowd stored procedure Inputs
        //uspUpdateCrowd (@LName, @LCount)
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

        public List<LocationData> GetPastData()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM locationdata"; //might need to add ORDER BY LocationID
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<LocationData> locationdataList = new List<LocationData>();
            while (reader.Read())
            {
                locationdataList.Add(
                new LocationData
                {
                    LocationID= reader.GetInt32(0), //0: 1st column
                    Time = reader.GetSqlDateTime(1), 
                    CrowdNow = reader.GetInt32(2)
                }
            );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return locationdataList;
        }
        public List<NearbyData> GetNearbyData()
        {
        //Create a SqlCommand object from connection object
        SqlCommand cmd = conn.CreateCommand();
        //Specify the SELECT SQL statement
        cmd.CommandText = @"SELECT* FROM nearby_location";    //might need to add ORDER BY LocationID
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<NearbyData> nearbydataList = new List<NearbyData>();
            while (reader.Read())
            {
                nearbydataList.Add(
                new NearbyData
                {
                    LocationID = reader.GetInt32(0), //0: 1st column
                    NearbyID = reader.GetInt32(1),
                    NearbyName = reader.GetString(2)
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return nearbydataList;
        }
    }
}
