using sample.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sample.Controllers
{
    [Authorize]
    public class InventoryManagerController : Controller
    {
        SampleEntities context = new SampleEntities();
        // GET: InventoryManager
        public ActionResult AllInventory()
        {
            ProductsetupModel model = new ProductsetupModel();
            var list = ChairsItems();
            var crowlist = CrowdItems();
            var fanlist = FanItems();
            model.Chairlist = list;
            model.Fanlist = fanlist;
            model.Crowdlist = crowlist;
            model.BEACH = BEACH();
            model.IceChest = IceChest();
            model.kiddies = kiddies();
            model.LedFurniture = LedFurniture();
            model.LOUNGEFURNITURE = LOUNGEFURNITURE();
            model.Table = Table();
            model.TENTAccessories = TENTAccessories();
            model.TENTMANIQUESS = TENTMANIQUESS();
            
            return View(model);
        }

        public List<ProductsetupModel> FanItems()
        {
            List<ProductsetupModel> list = new List<ProductsetupModel>();
            List<Subcategory> objlist = context.Subcategories.Where(x => x.ProductId == 6).ToList();

            foreach (var a in objlist)
            {
                ProductsetupModel model = new ProductsetupModel();
                model.subcatId = a.Id;
                model.categoryName = a.Category.CategoryName;
                model.subcategory = a.SubCategory1;
                model.price = a.Price;
                model.quantity = a.Quantity;
                list.Add(model);

            }
            return list;
        }

        public List<ProductsetupModel> ChairsItems()
        {
            List<ProductsetupModel> list = new List<ProductsetupModel>();
            List<Subcategory> objlist = context.Subcategories.Where(x => x.ProductId==1).ToList();

            foreach (var a in objlist)
            {
                ProductsetupModel model = new ProductsetupModel();
                model.subcatId = a.Id;
                model.images = a.imagesPath;
                model.categoryName = a.Category.CategoryName;
                model.subcategory = a.SubCategory1;
                model.price = a.Price;
                model.quantity = a.Quantity;
                list.Add(model);

            }
            return list;
        }


        public List<ProductsetupModel> CrowdItems()
        {
            List<ProductsetupModel> list = new List<ProductsetupModel>();
            List<Subcategory> objlist = context.Subcategories.Where(x => x.ProductId == 2).ToList();

            foreach (var a in objlist)
            {
                ProductsetupModel model = new ProductsetupModel();
                model.subcatId = a.Id;
                model.images = a.imagesPath;
                model.categoryName = a.Category.CategoryName;
                model.subcategory = a.SubCategory1;
                model.price = a.Price;
                model.quantity = a.Quantity;
                list.Add(model);

            }
            return list;
        }

        public List<ProductsetupModel> IceChest()
        {
            List<ProductsetupModel> list = new List<ProductsetupModel>();
            List<Subcategory> objlist = context.Subcategories.Where(x => x.ProductId == 7).ToList();

            foreach (var a in objlist)
            {
                ProductsetupModel model = new ProductsetupModel();
                model.subcatId = a.Id;
                model.images = a.imagesPath;
                model.categoryName = a.Category.CategoryName;
                model.subcategory = a.SubCategory1;
                model.price = a.Price;
                model.quantity = a.Quantity;
                list.Add(model);

            }
            return list;
        }


        public List<ProductsetupModel> kiddies()
        {
            List<ProductsetupModel> list = new List<ProductsetupModel>();
            List<Subcategory> objlist = context.Subcategories.Where(x => x.ProductId == 8).ToList();

            foreach (var a in objlist)
            {
                ProductsetupModel model = new ProductsetupModel();
                model.subcatId = a.Id;
                model.images = a.imagesPath;
                model.categoryName = a.Category.CategoryName;
                model.subcategory = a.SubCategory1;
                model.price = a.Price;
                model.quantity = a.Quantity;
                list.Add(model);

            }
            return list;
        }

        public List<ProductsetupModel> LedFurniture()
        {
            List<ProductsetupModel> list = new List<ProductsetupModel>();
            List<Subcategory> objlist = context.Subcategories.Where(x => x.ProductId == 9).ToList();

            foreach (var a in objlist)
            {
                ProductsetupModel model = new ProductsetupModel();
                model.subcatId = a.Id;
                model.images = a.imagesPath;
                model.categoryName = a.Category.CategoryName;
                model.subcategory = a.SubCategory1;
                model.price = a.Price;
                model.quantity = a.Quantity;
                list.Add(model);

            }
            return list;
        }


        public List<ProductsetupModel> LOUNGEFURNITURE()
        {
            List<ProductsetupModel> list = new List<ProductsetupModel>();
            List<Subcategory> objlist = context.Subcategories.Where(x => x.ProductId == 10).ToList();

            foreach (var a in objlist)
            {
                ProductsetupModel model = new ProductsetupModel();
                model.subcatId = a.Id;
                model.images = a.imagesPath;
                model.categoryName = a.Category.CategoryName;
                model.subcategory = a.SubCategory1;
                model.price = a.Price;
                model.quantity = a.Quantity;
                list.Add(model);

            }
            return list;
        }

        public List<ProductsetupModel> Table()
        {
            List<ProductsetupModel> list = new List<ProductsetupModel>();
            List<Subcategory> objlist = context.Subcategories.Where(x => x.ProductId == 11).ToList();

            foreach (var a in objlist)
            {
                ProductsetupModel model = new ProductsetupModel();
                model.subcatId = a.Id;
                model.images = a.imagesPath;
                model.categoryName = a.Category.CategoryName;
                model.subcategory = a.SubCategory1;
                model.price = a.Price;
                model.quantity = a.Quantity;
                list.Add(model);

            }
            return list;
        }

        public List<ProductsetupModel> BEACH()
        {
            List<ProductsetupModel> list = new List<ProductsetupModel>();
            List<Subcategory> objlist = context.Subcategories.Where(x => x.ProductId == 12).ToList();

            foreach (var a in objlist)
            {
                ProductsetupModel model = new ProductsetupModel();
                model.subcatId = a.Id;
                model.images = a.imagesPath;
                model.categoryName = a.Category.CategoryName;
                model.subcategory = a.SubCategory1;
                model.price = a.Price;
                model.quantity = a.Quantity;
                list.Add(model);

            }
            return list;
        }

        public List<ProductsetupModel> TENTMANIQUESS()
        {
            List<ProductsetupModel> list = new List<ProductsetupModel>();
            List<Subcategory> objlist = context.Subcategories.Where(x => x.ProductId == 13).ToList();

            foreach (var a in objlist)
            {
                ProductsetupModel model = new ProductsetupModel();
                model.subcatId = a.Id;
                model.images = a.imagesPath;
                model.categoryName = a.Category.CategoryName;
                model.subcategory = a.SubCategory1;
                model.price = a.Price;
                model.quantity = a.Quantity;
                list.Add(model);

            }
            return list;
        }

        public List<ProductsetupModel> TENTAccessories()
        {
            List<ProductsetupModel> list = new List<ProductsetupModel>();
            List<Subcategory> objlist = context.Subcategories.Where(x => x.ProductId == 14).ToList();

            foreach (var a in objlist)
            {
                ProductsetupModel model = new ProductsetupModel();
                model.subcatId = a.Id;
                model.images = a.imagesPath;
                model.categoryName = a.Category.CategoryName;
                model.subcategory = a.SubCategory1;
                model.price = a.Price;
                model.quantity = a.Quantity;
                list.Add(model);

            }
            return list;
        }
        [HttpGet]
        public ActionResult DetailsItem(int Id)
        {
            var cust = context.Subcategories.Where(x => x.Id == Id).SingleOrDefault();
            ProductsetupModel model = new ProductsetupModel();
            model.productName = cust.Product.PoductName;
            model.categoryName = cust.Category.CategoryName;
            model.subcategory = cust.SubCategory1;
            model.price = cust.Price;
            model.quantity = cust.Quantity;
            model.ProductId = cust.ProductId;
            model.catgoryId = cust.CategoryId;
            model.subcatId = cust.Id;
            return View(model);

        }
       [HttpGet]

        public ActionResult Report()
        {
            ReceiptModel model = new ReceiptModel();
            var list = Reciept();
            model.BillingList = list;
            model.StartDate = DateTime.Today;
            model.EndDate = DateTime.Today;

            return View(model);
            
        }

        
        public List<ReceiptModel> Reciept()
        {
            List<ReceiptModel> list = new List<ReceiptModel>();
            List<BillItem> objlist = context.BillItems.OrderByDescending(x => x.EntryDate).ToList();
            foreach (var a in objlist)
            {
                ReceiptModel model = new ReceiptModel();
                model.Id = a.Id;
                model.InvoiceNumber = a.InvoiceNumber;
                model.description = a.Description.ToUpper();
                model.EntryDate= a.EntryDate.ToShortDateString();
                model.Quantity = a.Quantity;
                model.Amount = a.Amount;
                model.customerName = a.CustomerName;
                model.Rate = a.Rate;
                list.Add(model);

            }
            return list;
        }
        
        [HttpPost]
        public ActionResult Report(ReceiptModel mode)
        {

            List<ReceiptModel> list = new List<ReceiptModel>();
            if (mode.StartDate != null && mode.EndDate != null);
            var query = context.BillItems.Where(emp => emp.EntryDate >= mode.StartDate && emp.EntryDate <= mode.EndDate);
            if(!string.IsNullOrEmpty(mode.InvoiceNumber))
                query =query.Where(emp=> emp.InvoiceNumber==mode.InvoiceNumber);
               
            //var u = context.BillItems.Where(emp => emp.InvoiceNumber == mode.InvoiceNumber);
            foreach (var a in query)
            {
                ReceiptModel model = new ReceiptModel();

                model.EntryDate = a.EntryDate.ToShortDateString();
                model.Id = a.Id;
                model.InvoiceNumber = a.InvoiceNumber.ToUpper();
                model.description = a.Description.ToUpper();
                model.Rate = a.Rate;
                model.Quantity = a.Quantity;
                model.customerName = a.CustomerName;
                model.Amount = a.Amount;
                list.Add(model);

            }
            mode.BillingList = list;

            return View(mode);
        }
        [HttpGet]
        public ActionResult EditItem(int Id)
        {
            var cust = context.Subcategories.Where(x => x.Id == Id).SingleOrDefault();
            ProductsetupModel model = new ProductsetupModel();
            model.productName = cust.Product.PoductName;
            model.categoryName = cust.Category.CategoryName;
            model.subcategory = cust.SubCategory1;
            model.price = cust.Price;
            model.quantity = cust.Quantity;
            model.ProductId = cust.ProductId;
            model.catgoryId = cust.CategoryId;
            model.subcatId = cust.Id;
            return View(model);
          
        }
        [HttpPost]
        public ActionResult EditItem(ProductsetupModel model, HttpPostedFileBase uploadedFile)
        {
            if (model != null)
            {
               
                var client = context.Subcategories.Where(x => x.Id == model.subcatId).SingleOrDefault();
                client.Id = model.subcatId;
                client.SubCategory1 = model.subcategory;
                client.Price = model.price;
                client.Quantity = model.quantity;
                if (client.imagesPath != null)
                {
                    client.imagesPath = client.imagesPath;
                }
                else
                {
                    string ImageName = System.IO.Path.GetFileName(uploadedFile.FileName);
                    string physicalPath = Server.MapPath("~/Images/" + ImageName);
                    //save image in folder
                    uploadedFile.SaveAs(physicalPath);
                    client.imagesPath = ImageName;
                }
                try
                {
                    context.Entry(client).State = EntityState.Modified;
                    context.SaveChanges();
                    Session["Success"] = "Edited Sucessfully";
                }
                catch
                {
                    Session["fail"] = "Customer Not Editted";
                }

            }

            return RedirectToAction("AllInventory");

        }
    }
}