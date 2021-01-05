using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceAndGo.Models
{


        public class Rootobject
        {
            public string status { get; set; }
            public int total_hits { get; set; }
            public int page { get; set; }
            public int total_pages { get; set; }
            public int page_size { get; set; }
            public Article[] articles { get; set; }
            public User_Input user_input { get; set; }
        }

        public class User_Input
        {
            public string q { get; set; }
            public string search_in { get; set; }
            public string lang { get; set; }
            public object country { get; set; }
            public string from { get; set; }
            public object to { get; set; }
            public string ranked_only { get; set; }
            public object from_rank { get; set; }
            public object to_rank { get; set; }
            public string sort_by { get; set; }
            public int page { get; set; }
            public int size { get; set; }
            public object sources { get; set; }
            public object not_sources { get; set; }
            public object topic { get; set; }
            public string media { get; set; }
        }

        public class Article
        {
            public string summary { get; set; }
            public string country { get; set; }
            public string author { get; set; }
            public string link { get; set; }
            public string language { get; set; }
            public string media { get; set; }
            public string title { get; set; }
            public string media_content { get; set; }
            public string clean_url { get; set; }
            public string rights { get; set; }
            public string rank { get; set; }
            public string topic { get; set; }
            public string published_date { get; set; }
            public string _id { get; set; }
            public float _score { get; set; }
        }

    
}
