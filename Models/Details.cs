using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceAndGo.Models
{
    public class Details
    {

        public string Location { get; set; } //LName in SQL database
        public int CrowdNow { get; set; }

        public int Confirmed { get; set; }
    }
}
