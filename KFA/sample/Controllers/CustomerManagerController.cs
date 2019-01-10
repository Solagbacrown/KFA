using sample.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace sample.Controllers
{
    [Authorize]
    public class CustomerManagerController : Controller
    {
        SampleEntities context = new SampleEntities();

       
        // GET: CustomeraManger
        [HttpGet]
        public ActionResult Addcustomers()
        {
            CustomerModel model = new CustomerModel();
            var list = customer();
            model.customerlist = list;
           
            return View(model);
        }
        [HttpPost]
        public ActionResult Addcustomers(CustomerModel mode)
        {
            
            List<CustomerModel> list = new List<CustomerModel>();
            var r = context.Customers.Where(emp => emp.Address.Contains(mode.Search)|| emp.EmailAddress.Contains(mode.Search) || emp.FullName.Contains(mode.Search) || emp.PhoneNumber.Contains(mode.Search) && emp.IsActive==true).ToList();
            foreach (var a in r)
            {
                CustomerModel model = new CustomerModel();
                model.Id = a.Id;
                model.Adress = a.Address.ToUpper();
                model.EmailAdress = a.EmailAddress.ToUpper();
                model.FullName = a.FullName.ToUpper();
                model.PhoneNumber = a.PhoneNumber;
                list.Add(model);

            }
            mode.customerlist = list;
           
            return View(mode);
        }
        [HttpGet]
        public ActionResult Editcustomers(int Id)
        {
            var cust = context.Customers.Where(x => x.Id == Id).SingleOrDefault();
            CustomerModel model = new CustomerModel();
            model.EmailAdress = cust.EmailAddress.ToUpper();
            model.Adress = cust.Address.ToUpper();
            model.FullName = cust.FullName.ToUpper();
            model.PhoneNumber = cust.PhoneNumber;
            return View(model);
        }
        [HttpPost]
        public ActionResult Editcustomers(CustomerModel model)
        {
            if (model!=null)
            {
                Customer client = new Customer();
                client.Id=model.Id;
                client.FullName = model.FullName.ToUpper();
                client.Address = model.Adress.ToUpper();
                client.PhoneNumber = model.PhoneNumber;
                client.EmailAddress = model.EmailAdress.ToUpper();
                client.IsActive = true;
                try
                {
                    context.Entry(client).State = EntityState.Modified;
                    context.SaveChanges();
                    Session["Success"] = "Edited Created Sucessfully";
                }
                catch
                {
                    Session["fail"] = "Customer Not Editted";
                }

            }

            return RedirectToAction("Addcustomers");
        }


        
        public ActionResult Delete(Guid uniqueId)
        {

            var cust = context.Customers.Where(x => x.customenId == uniqueId).SingleOrDefault();
            cust.IsActive = false;
                try
                {
                    context.Entry(cust).State = EntityState.Modified;
                    context.SaveChanges();
                    Session["Success"] = "Edited Created Sucessfully";
                }
                catch
                {
                    Session["fail"] = "Customer Not Editted";
                }
             return RedirectToAction("Addcustomers");
            }

           
       
        public List<CustomerModel> customer ()
        {
            List<CustomerModel> list = new List<CustomerModel>();
            List<Customer> objlist = context.Customers.Where(x=>x.IsActive==true).ToList();
           
            foreach(var a in objlist)
            {
                CustomerModel model = new CustomerModel();
                model.Id = a.Id;
                model.Title = a.Title.ToUpper();
                model.Adress = a.Address.ToUpper();
                model.EmailAdress = a.EmailAddress.ToUpper();
                model.FullName = a.FullName.ToUpper();
                model.PhoneNumber = a.PhoneNumber.ToUpper();
                model.uniqueId = a.customenId;
                model.IsActive = a.IsActive;
                list.Add(model);
                
            }
            return list;
        }

        [HttpGet]
        public ActionResult Createcustomers()
        {
            CustomerModel model = new CustomerModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Createcustomers(CustomerModel model)
        {
            if (model != null)
            {
                Customer client = new Customer();
                client.Title = model.Title.ToUpper();
                client.FullName = model.FullName.ToUpper();
                client.Address = model.Adress.ToUpper();
                client.PhoneNumber = model.PhoneNumber;
                client.EmailAddress = model.EmailAdress.ToUpper();
                client.IsActive = true;
                client.customenId = Guid.NewGuid();

                context.Customers.Add(client);
                try
                {
                    context.SaveChanges();
                    Session["Success"] = "Customer Created Sucessfully";
                }
                catch
                {
                    Session["fail"] = "Customer Created Sucessfully";
                }
                
            }
            
            return RedirectToAction("Addcustomers");
        }
    }
}