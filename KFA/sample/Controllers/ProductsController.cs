using sample.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sample.Controllers
{

   
    public class ProductsController : Controller
    {

        SampleEntities context = new SampleEntities();

        [HttpPost]
        public JsonResult ProductExist(string product)
        {
            var productexist = context.Products.Where(x => x.PoductName == product).SingleOrDefault();

            return Json(productexist.PoductName == null);
        }
        private IEnumerable<SelectListItem> Products(ProductsetupModel model)
        {
            
            List<Product> products = new List<Product>();
            products = context.Products.ToList();
            var list = from s in products
                       select new SelectListItem
                       {
                           Selected = s.Id.ToString() == model.ProductId.ToString(),
                           Text = s.PoductName,
                           Value = s.Id.ToString()
                       };

            return list;
        }
        private IEnumerable<SelectListItem> Category(ProductsetupModel model)
        {

            List<Category> products = new List<Category>();
            products = context.Categories.ToList();
            var list = from s in products
                       select new SelectListItem
                       {
                           Selected = s.Id.ToString() == model.catgoryId.ToString(),
                           Text = s.CategoryName,
                           Value = s.Id.ToString()
                       };

            return list;
        }

        private IEnumerable<SelectListItem> SubCategory(ProductsetupModel model)
        {

            List<Subcategory> products = new List<Subcategory>();
            products = context.Subcategories.ToList();
            var list = from s in products
                       select new SelectListItem
                       {
                           Selected = s.Id.ToString() == model.catgoryId.ToString(),
                           Text = s.SubCategory1,
                           Value = s.Id.ToString()
                       };

            return list;
        }
        // GET: Products
        [HttpGet]
        public ActionResult SetupProducts()
        {
            ProductsetupModel model = new ProductsetupModel();
            List<Product> product = new List<Product>();
            List<Category> category = new List<Category>();
            product = context.Products.ToList();
            category = context.Categories.ToList();
            model.Products = Products(model);
            model.Categories = Category(model);
            return View(model);
        }
        
        [HttpPost]
        public ActionResult SetupProduct(ProductsetupModel model)
        {
            Product product = new Product();
        
            if (!string.IsNullOrEmpty(model.productName))
            {
                var result = context.Products.Where(x => x.PoductName == model.productName).SingleOrDefault();
                if (result == null)
                {
                    product.PoductName = model.productName;
                    context.Products.Add(product);
                    context.SaveChanges();
                    Session["exist"] = "Product created Successfully";
                }
                else { Session["fail"] = "This Product Already Exist please pick a new one"; }
            }
          
            
            return RedirectToAction("SetupProducts");
        }

        [HttpPost]
        public ActionResult SetupCategories(ProductsetupModel model)
        {
            Category product = new Category();
            if (!string.IsNullOrEmpty(model.categoryName))
            {
             var result = context.Categories.Where(x => x.CategoryName == model.categoryName).SingleOrDefault();
                 if (result == null)
                {
            product.PoductId = model.ProductId;
            product.CategoryName = model.categoryName;
            context.Categories.Add(product);
            context.SaveChanges();
                 Session["exist"] = "Category created Successfully";
                 }
          
             else { Session["fail"] = "Category Item Already Exist please pick a new one"; }
            }
            return RedirectToAction("SetupProducts"); 
            }

        public IList<Category> GetAllCategoriesByProductId(int productId)
        {
            var query = from category in context.Categories
                        where category.PoductId == productId
                        select category;
            var content = query.ToList<Category>();
            return content;
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetCategoryByProductId(string productId)
        {
            if (String.IsNullOrEmpty(productId))
            {
                throw new ArgumentNullException("productId");
            }
            int id = 0;
            bool isValid = Int32.TryParse(productId, out id);
            var states = GetAllCategoriesByProductId(id);

            var result = (from s in states
                          select new
                          {
                              id = s.Id,
                              name = s.CategoryName
                          }).ToList();


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetupSubCategories(ProductsetupModel model, HttpPostedFileBase uploadedFile)
        {

            if (uploadedFile != null)
            {
                string ImageName = System.IO.Path.GetFileName(uploadedFile.FileName);
                string physicalPath = Server.MapPath("~/images/" + ImageName);
                // save image in folder
                uploadedFile.SaveAs(physicalPath);
                Subcategory product = new Subcategory();
                if (!string.IsNullOrEmpty(model.subcategory))
                {
                    var result = context.Subcategories.Where(x => x.SubCategory1 == model.subcategory).SingleOrDefault();
                    if (result == null)
                    {

                        product.ProductId = model.ProductId;
                        product.CategoryId = model.catgoryId;
                        product.SubCategory1 = model.subcategory;
                        product.Price = model.price;
                        product.Quantity = model.quantity;
                        product.imagesPath =ImageName;
                        context.Subcategories.Add(product);
                        context.SaveChanges();
                        Session["exist"] = "SubCategory created Successfully";
                    }
                    else { Session["fail"] = "Subcategory item Already Exist please pick a new one"; }
                }
            }
            return RedirectToAction("SetupProducts");
        }

     }
    
}