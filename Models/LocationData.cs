﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceAndGo.Models
{
    //public class DataContext : IDisposable
    public /*partial*/ class LocationData //: DataContext
    {
        public string Location { get; set; } //LName in SQL database
        public int LocationID { get; set; }
        public int CrowdNow { get; set; }
        public SqlDateTime Time { get; set; }
        public int Confirmed { get; set; }

        //public List<int> Data { get; set; }


        /*public Table<Locations> Locations;
        public Table<LCount> LCount;
        public LocationData(string connection) : base(connection) { }*/
    }
}
