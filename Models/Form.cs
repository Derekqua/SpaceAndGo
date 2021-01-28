using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SpaceAndGo.Models
{
    public class Form
    {
        [Required, Display(Name = "Your email"), EmailAddress]
        public string FromEmail { get; set; }
    }
}
