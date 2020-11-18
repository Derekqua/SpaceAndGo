using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceAndGo.Models
{
    //public class DataContext : IDisposable
    public /*partial*/ class LocationData //: DataContext
    {
        public string Location { get; set; } //LName in SQL database
        public int CrowdNow { get; set; }

        /*public Table<Locations> Locations;
        public Table<LCount> LCount;
        public LocationData(string connection) : base(connection) { }*/
    }
}
