using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sample.Models
{
    public class LocationModel
    {
        public int Id{ get; set; }
        
        [Required]
       
        [Display(Name = "Location")]
      
        public string Location { get; set; }
        [Required]
     
        [Display(Name = "Transport Fee (To-Fro)")]
    
        public string Amount { get; set; }

        public List<LocationModel> LocationList { get; set; }
         
    }
}