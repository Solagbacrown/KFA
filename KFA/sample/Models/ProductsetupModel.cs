using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sample.Models
{
    public class ProductsetupModel
    {

        public IEnumerable<SelectListItem> Products { get; set; }
        public IEnumerable<SelectListItem> Categories{ get; set; }
        public IEnumerable<SelectListItem> SubCategories { get; set; }
      
        public int ProductId { get; set; }
        [Required]
        [RegularExpression(@"([A-Z][a-zA-Z\-\']*\s*)*", ErrorMessage = "Please it must start with a capital letter")]
        [Display(Name = "Product Name")]
        [Remote("Product", "Products", HttpMethod = "POST", ErrorMessage = "Product already exists. Please enter a different Product name.")]
        public string productName { get; set; }
       
        public int catgoryId { get; set; }
        [Required]
        [RegularExpression(@"([A-Z][a-zA-Z\-\']*\s*)*", ErrorMessage = "Please it must start with a capital letter")]
        [Display(Name = "Category")]
        public string categoryName { get; set; }

        public int subcatId { get; set; }
        
        [Display(Name = "Quantity")]
        public int quantity { get; set; }
        [Required]

        [Display(Name = "SubCategory")]
        [RegularExpression(@"([A-Z][a-zA-Z\-\']*\s*)*", ErrorMessage = "Please it must start with a capital letter")]
        public string subcategory { get; set; }
        [Required]

        [Display(Name = "Rate")]
        public decimal price { get; set; }
        [Required]

        //[Display(Name = "Upload Images")]
        public string images { get; set; }
        public List<ProductsetupModel> Chairlist { get; set; }
        public List<ProductsetupModel> Crowdlist { get; set; }
        public List<ProductsetupModel> Fanlist { get; set; }

        public List<ProductsetupModel> IceChest { get; set; }
        public List<ProductsetupModel> kiddies { get; set; }
        public List<ProductsetupModel> LedFurniture { get; set; }

        public List<ProductsetupModel> LOUNGEFURNITURE { get; set; }
        public List<ProductsetupModel> Table { get; set; }
        public List<ProductsetupModel> BEACH { get; set; }

        public List<ProductsetupModel> TENTMANIQUESS { get; set; }
        public List<ProductsetupModel> TENTAccessories { get; set; }
        //public List<ProductsetupModel> Fanlist { get; set; }


    }
}