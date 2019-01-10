using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace sample.Models
{
    public class QuotationModel
    {

        public QuotationModel()
        {
            ProductList = new List<SelectListItem>();
            CategoryList = new List<SelectListItem>();
            SubCategoryList = new List<SelectListItem>();
        }
        public IEnumerable<SelectListItem> cutomerList { get; set; }
       
        public List<QuotationModel> BillingList { get; set; }
        public IList<SelectListItem> CategoryList { get; set; }
        public  IList<SelectListItem> SubCategoryList { get; set; }
        public IEnumerable<SelectListItem> ProductList { get; set; }


        public MultiSelectList Locations { get; set; }
        [Display(Name = "Customers")]
        public int cutomerId { get; set; }
        [Display(Name = "Delivery Address")]

        public string DeliveryAddress { get; set; }

          [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
         [Display(Name = "Product")]
        public int ProductId { get; set; }
         [Display(Name = "Category")]
        public int categoryId { get; set; }
          [Display(Name = "SubCategory")]
        public int subcategoryId { get; set; }
        public string description { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        public int BillId { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
         [Display(Name = "Home Address")]
        public string HomeAddress { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public DateTime EntryDate { get; set; }
        [Display(Name = "Event Date")]
        public DateTime EventDate { get; set; }
        [Display(Name = "Setup Date")]
        public DateTime SetupDate { get; set; }
       
     
        
    }
}