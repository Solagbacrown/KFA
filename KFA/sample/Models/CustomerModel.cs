using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sample.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "FULL NAME")]
        [RegularExpression(@"^([a-zA-Z])[a-zA-Z_-]*[\w_-]*[\S]$|^([a-zA-Z])[0-9_-]*[\S]$|^[a-zA-Z]*[\S]$")]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        [Display(Name = "PHONE NUMBER")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "ADDRESS")]
        public string Adress { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "EMAIL")]
        public string EmailAdress { get; set; }

        [Display(Name = "SEARCH")]
        public string Search { get; set; }
        public bool IsActive { get; set; }
        public Guid  uniqueId { get; set; }
        [Required]
        [Display(Name = "TITLE")]
        
        public string Title { get; set; } 
        public List<CustomerModel> customerlist { get; set; }
    }
}