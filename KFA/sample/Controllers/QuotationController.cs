using sample.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;
using System.Diagnostics;
using ReportModule;
using ReportModule.InvoiceEntity;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Runtime.Serialization.Json;
using iTextSharp.text.pdf.draw;
 

namespace sample.Controllers
{

    [Authorize]
    public class QuotationController : Controller
    {

        SampleEntities context = new SampleEntities();

        // GET: Quotation


       
      
        [HttpGet]
        public ActionResult CreateQuotation()
        {
            QuotationModel model = new QuotationModel();
           
            model.CategoryList.Add(new SelectListItem { Text = "Please Select Category", Value = "-1" });
            var categories =GetAllCategory();
            foreach (var category in categories)
            {
                model.CategoryList.Add(new SelectListItem()
                {
                    Text = category.CategoryName,
                    Value = category.Id.ToString()
                });
            }

            var list = CustomersList(model);
            model.cutomerList = CustomersList(model);
            model.ProductList = ProductList(model);
            if (Session["customerkey"] != null)
            {
                var mybill = bills(Session["customerkey"].ToString());
                model.BillingList = mybill;
            }
            model.SetupDate = DateTime.Today;
            model.EventDate = DateTime.Today;

            List<Location> Reasons = new List<Location>();
            Reasons = context.Locations.OrderBy(x => x.Location1).ToList();
            model.Locations = new MultiSelectList(Reasons, "Id", "Location1", Reasons.Select(x => x.Id));
            //model.CategoryList = CategoryList(model);
            //model.SubCategoryList= SubCategoryList(model.categoryId);
            return View(model);
          
        }
        [HttpPost]
        public ActionResult GrandQuotation(int[] groups, QuotationModel model)
        {
            if (groups!=null)
            {
                //List<string> amountlist = new List<string>();
                foreach (var group in groups)
                {      
                     var location = context.Locations.Where(x => x.Id == group).SingleOrDefault();
                     TransportBill bill = new TransportBill();
                     if (Session["customerkey"] != null)
                     {
                         bill.SessionKey = Session["customerkey"].ToString();
                         bill.TransportBill1 = location.Value;
                         bill.EventDate = model.EventDate;
                         bill.SetupDate = model.SetupDate;
                         bill.DeliveryAddress = model.DeliveryAddress;
                         try
                         {
                             context.TransportBills.Add(bill);
                             context.SaveChanges();

                         }
                         catch
                         {
                             TempData["customerempty"] = "This customer session as expire please restart the process";
                             return RedirectToAction("CreateQuotation");
                         }
                         Session["EventDate"] = model.EventDate;
                         Session["SetupDate"] = model.SetupDate;
                         Session["Address"] = model.DeliveryAddress;
                      
                     }
                     else
                     {
                         TempData["customerempty"] = "This customer session as expire please restart the process";
                     }
                }
               

            }
            else
            {
                TempData["customerempty"] = "Atleast you must select a location";
                return RedirectToAction("CreateQuotation");
            }
           

            return RedirectToAction("QuotationView");
        }

        public float PixelsToPoints(float value)
        {
            var result = value / 110 * 72;
            return result;
        }

     

        public void ToAdminEmail(string to,string message)
        {

            var invoice = "KFA" + Session["Number"].ToString();
            string path= HttpContext.Server.MapPath(invoice+".pdf");
            var myMessage = new MailMessage();
            myMessage.To.Add(new MailAddress(to));  // replace with valid value 
            myMessage.Bcc.Add(new MailAddress("gbengaoluyide@gmail.com"));
            myMessage.CC.Add(new MailAddress("kfarentals@gmail.com"));
            myMessage.Bcc.Add(new MailAddress("henry.mba@venturegardengroup.com"));
            myMessage.From = new MailAddress("kfaportal@gmail.com");  // replace with valid value
            myMessage.Subject = "KFA RENTALS INVOICE";
            myMessage.Body = message;
            myMessage.Attachments.Add(new System.Net.Mail.Attachment(path));
            myMessage.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "kfaportal@gmail.com",  // replace with valid value
                    Password = "kemiadeleke"  // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(myMessage);
            }
        }

        public  void ConvertoPdf(ReceiptModel  model)
        {
            //invoiceEntry inv = new invoiceEntry();
            decimal transport = model.Trasport.Sum(m => m.TransportBill);
            decimal Amountsum = model.BillingList.Sum(m => m.Amount);
            decimal fivepercent = Math.Round(Decimal.Multiply(Amountsum, decimal.Parse("0.05")),2);
            decimal grandTotal = Amountsum + fivepercent + transport;

            var invoice = "KFA" + Session["Number"].ToString();

            PdfPTable tableLayout = new PdfPTable(4);
            string url = HttpContext.Server.MapPath("logo.png");
            ReportBC bc = new ReportBC();
            //bc.GetFreshReportPdfDocument(url, inv, "");
            iTextSharp.text.Font textFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 8, iTextSharp.text.Font.NORMAL);//5e85ab
            iTextSharp.text.Font textFont3 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);//5e85ab
             iTextSharp.text.Font textFont4 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);//5e85ab
            Document doc = new Document();
            string logo = HttpContext.Server.MapPath("logo.png");
            //doc.Add(new Paragraph("GIF"));
            iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(logo + "");
            gif.ScaleToFit(PixelsToPoints(900), PixelsToPoints(132));
            gif.SetAbsolutePosition(250f, 740f);
            gif.Alignment = Element.ALIGN_CENTER;

            PdfPTable tab = new PdfPTable(2);
            tab.DefaultCell.Border = 0;
            //table.SetWidthPercentage(new float[] { 100, 120 }, PageSize.A4);
            tab.SetWidths(new float[] { 20, 80 });
            tab.HorizontalAlignment = Element.ALIGN_CENTER;
            //doc.Add(gif);
            //Create PDF Table
            PdfPTable table0 = new PdfPTable(2);
            table0.DefaultCell.Border = 0;
            //table.SetWidthPercentage(new float[] { 100, 120 }, PageSize.A4);
            table0.SetWidths(new float[] { 20, 80 });
            table0.HorizontalAlignment = Element.ALIGN_CENTER;


            PdfPTable table = new PdfPTable(2);
            table.DefaultCell.Border = 0;
            //table.SetWidthPercentage(new float[] { 100, 120 }, PageSize.A4);
            table.SetWidths(new float[] { 20, 80 });
            table.HorizontalAlignment = Element.ALIGN_LEFT;

            PdfPTable table2 = new PdfPTable(2);
            table2.DefaultCell.Border = 0;
            //table.SetWidthPercentage(new float[] { 100, 120 }, PageSize.A4);
            table2.SetWidths(new float[] { 20, 80 });
            table2.HorizontalAlignment = Element.ALIGN_LEFT;


            //Create a PDF file in specific path
            PdfWriter.GetInstance(doc, new FileStream(Server.MapPath(invoice+".pdf"), FileMode.Create));
           
            //Open the PDF document
            doc.Open();

            //Add Content to PDF
            
            
            doc.Add(gif);

            tab = new PdfPTable(2);
            tab.DefaultCell.Border = 0;
            tab.SetWidths(new float[] { 12, 30 });
            tab.HorizontalAlignment = Element.ALIGN_LEFT;
            tab.AddCell(new Phrase(" ", textFont3));
            tab.AddCell(new Phrase(" ", textFont));
            tab.AddCell(new Phrase(" ", textFont3));
            tab.AddCell(new Phrase(" ", textFont));
            tab.AddCell(new Phrase(" ", textFont3));
            tab.AddCell(new Phrase(" ", textFont));
            tab.AddCell(new Phrase(" ", textFont3));
            tab.AddCell(new Phrase(" ", textFont));
            tab.AddCell(new Phrase(" ", textFont3));
            tab.AddCell(new Phrase(" ", textFont));
            //tab.AddCell(new Phrase(" ", textFont3));
            //tab.AddCell(new Phrase(" ", textFont));
            //tab.AddCell(new Phrase(" ", textFont));
            //tab.AddCell(new Phrase(" ", textFont3));
            //tab.AddCell(new Phrase(" ", textFont));
            doc.Add(tab);

            Paragraph paragraph0 = new Paragraph();
            paragraph0 = new Paragraph { Alignment = Element.ALIGN_CENTER };
            string term = "RENTALS  TERMS AND CONDITIONS.";
            var phrase0 = new Phrase(term, textFont3);

          
            List list = new List(List.UNORDERED);
            list.Add(new ListItem("PLEASE READ OUR TERMS & CONDITIONS CAREFULLY. IF YOU ORDER FROM US, IT WILL BE ASSUMED YOU HAVE READ AND UNDERSTAND THE ITEMS OUTLINED ON THIS PAGE, AND AGREE TO ABIDE BY THEM. PLEASE REVIEW THE ABOVE QUOTATION." ,textFont));
            
            Paragraph paragraph = new Paragraph();
            string text = "REVIEW";
            

            List list1 = new List(List.UNORDERED);
            list1.Add(new ListItem("FULL PAYMENT INTO OUR BANK ACCOUNT CONFIRMS THE ORDER/ BOOKING . KINDLY NOTIFY US WHEN PAYMENT IS MADE INTO OUR ACCOUNT TO ENABLE US PROCESS THE ORDER.",textFont));

            Paragraph paragraph1 = new Paragraph();
            string text1 = "PAYMENT";
           

            List list2 = new List(List.UNORDERED);
            list2.Add(new ListItem("FULL PAYMENT INTO OUR BANK ACCOUNT CONFIRMS THE ORDER AT LEAST ONE WEEK TO YOUR EVENT.  KINDLY NOTIFY US VIA EMAIL WHEN PAYMENT IS MADE INTO OUR ACCOUNT TO ENABLE US PROCESS THE ORDER. KFA WILL NOT COLLECT CASH FOR ANY TRANSACTION. PAYMENT IS NOT ALLOWED AT THE POINT OF DELIVERY. PLEASE USE NAME ON QUOTATION TO REFERENCE PAYMENT.", textFont));

            Paragraph paragraph2 = new Paragraph();
            string text2 = "DELIVERY AND RETRIEVAL";
            

            List list3 = new List(List.UNORDERED);
            list3.Add(new ListItem("WE WILL DELIVER YOUR ORDER THE EVENING BEFORE YOUR EVENT DEPENDING ON THE BOOKINGS WE HAVE. WE WILL RETRIEVE THE ITEMS IMMEDIATELY AFTER THE EVENT, IN THE EVENT THAT THIS ISN’T DONE, KINDLY PUT A CALL THROUGH TO MRS. ADELEKE ON 08062795680. RENTAL ITEMS THAT HAVE BEEN CHECKED OUT OF KFA WILL NOT BE REFUNDED IF NOT USED AT CLIENT’S LOCATION. WE ADVISE OUR CLIENTS TO TAKE GOOD CARE OF ALL THE RENTAL ITEMS FROM KFA. WE VALUE OUR PRODUCTS",textFont));

            Paragraph paragraph7 = new Paragraph();
            string text7 = "TABLE COVER";


            List list7 = new List(List.UNORDERED);
            list7.Add(new ListItem("KFA WILL CHARGE A REFUNDABLE DEPOSIT ON TABLE LINENS BASED ON THE QUANTITY BEING RENTED.  PLEASE CHECK THE WRAPPED LINENS GIVEN TO YOU IF YOU ARE DOING A PICKUP AND ALSO IF THE LINENS ARE DELIVERED TO YOU. PLEASE ENSURE THE LINENS ARE NOT BURNT OR TORN. ", textFont));



            Paragraph paragraph3 = new Paragraph();
            string text3 = "WEATHER";

            List listW = new List(List.UNORDERED);
            listW.Add(new ListItem("CLIENT UNDERSTANDS THAT TENTS ARE TEMPORARY STRUCTURES DESIGNED TO PROVIDE LIMITED PROTECTION FROM WEATHER CONDITIONS, PRIMARILY SUN AND RAIN; HOWEVER, THERE MAY BE SITUATIONS, PARTICULARLY INVOLVING STRONG WINDS, IN WHICH THE TENTS WILL NOT PROVIDE PROTECTION AND MAY EVEN BE DAMAGED OR BLOWN OVER. EVACUATION OF TENTS TO AVOID POSSIBLE INJURY IS RECOMMENDED WHEN SEVERE WEATHER THREATENS THE AREA WHERE TENT IS ERECTED. PEOPLE MUST LEAVE TENTS AND NOT SEEK SHELTER IN TENTS DURING SUCH CONDITIONS.  ", textFont));


            Paragraph paragraphG = new Paragraph();
            string textG = "GENERAL INFORMATION";

            List listG = new List(List.UNORDERED);
            listG.Add(new ListItem("PLEASE NOTE THAT KFA DOES NOT MIX THE RENTAL ITEMS WITH OTHER COMPANIES. THIS IS TO PREVENT ITEMS FROM BEING MIXED UP. KINDLY LET THE SETUP CREW KNOW THE EXACT LOCATION YOU WANT YOUR ORDER TO BE SETUP. RENTAL ORDERS ARE FOR A DAY ONLY. PLEASE LET US KNOW IF THE ITEMS ARE GOING TO BE USED FOR ADDITIONAL DAYS. PLEASE ADVISE YOUR DECORATORS NOT TO USE PINS OR TEAR OUR CANOPIES.", textFont));


            Paragraph paragraph4 = new Paragraph();
            paragraph4 = new Paragraph { Alignment = Element.ALIGN_CENTER };
            string r = "KM 15, LEKKI EPE EXPRESSWAY, OPPOSITE NICON ESTATES, LEKKI TEL: 0806 279 5680, 0704 043 4341 WWW.KFARENTAL.COM, KFARENTALS@GMAIL.COM 27 NOVEMBER 2015";
            var phrase = new Phrase(r, textFont3);


            Paragraph paragraph5 = new Paragraph();
            paragraph5 = new Paragraph { Alignment = Element.ALIGN_CENTER };
            string bank = "BANK DETAILS";
            var phrase2 = new Phrase(bank, textFont3);

            paragraph0.Add(phrase0);
            doc.Add(paragraph0);

            Chunk lin = new Chunk(new LineSeparator(1f, 100f, BaseColor.DARK_GRAY, Element.ALIGN_CENTER, -1));
            doc.Add(lin);

            paragraph.Add(text);
            doc.Add(paragraph);
            doc.Add(list);

            paragraph1.Add(text1);
            doc.Add(paragraph1);
            doc.Add(list1);

            paragraph2.Add(text2);
            doc.Add(paragraph2);
            doc.Add(list2);
           

            paragraph3.Add(text3);
            doc.Add(paragraph3);
            doc.Add(listW);

            paragraph7.Add(text7);
            doc.Add(paragraph7);
            doc.Add(list7);

            paragraphG.Add(textG);
            doc.Add(paragraphG);
            doc.Add(listG);

            Chunk linbreaker = new Chunk(new LineSeparator(1f, 100f, BaseColor.DARK_GRAY, Element.ALIGN_CENTER, -1));
            doc.Add(linbreaker);

            paragraph4.Add(phrase);
            doc.Add(paragraph4);

            Chunk linbreak0 = new Chunk(new LineSeparator(1f, 100f, BaseColor.DARK_GRAY, Element.ALIGN_CENTER, -1));
            doc.Add(linbreak0);

            paragraph5.Add(phrase2);
            doc.Add(paragraph5);
            table0 = new PdfPTable(2);
            table0.DefaultCell.Border = 0;
            table0.SetWidths(new float[] { 12, 30 });
            table0.HorizontalAlignment = Element.ALIGN_LEFT;
            table0.AddCell(new Phrase("Bank Name:", textFont3));
            table0.AddCell(new Phrase("Zenith", textFont));
            table0.AddCell(new Phrase("Name of account:", textFont3));
            table0.AddCell(new Phrase("kfarentals", textFont));
            table0.AddCell(new Phrase("Account Number", textFont3));
            table0.AddCell(new Phrase("1012657767", textFont));
            table0.AddCell(new Phrase("Sort Code:", textFont3));
            table0.AddCell(new Phrase(" 05715059", textFont));
           
            doc.Add(table0);

            Chunk linbreak = new Chunk(new LineSeparator(1f, 100f, BaseColor.DARK_GRAY, Element.ALIGN_CENTER, -1));
            doc.Add(linbreak);





            table = new PdfPTable(2);
            table.DefaultCell.Border = 0;
            table.SetWidths(new float[] { 12, 30 });
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(new Phrase("Name of Client:", textFont3));
            table.AddCell(new Phrase(model.Name.ToString(), textFont));
            table.AddCell(new Phrase("Date of Event:", textFont3));
            table.AddCell(new Phrase(model.eventdate.ToString(), textFont));
            table.AddCell(new Phrase("Date of Setup:", textFont3));
            table.AddCell(new Phrase(model.SetUpDate.ToString(), textFont));
            table.AddCell(new Phrase("Address of Delivery:", textFont3));
            table.AddCell(new Phrase(model.FullAddress, textFont));
            table.AddCell(new Phrase("Invoice Number:", textFont3));
            table.AddCell(new Phrase(invoice, textFont));
            doc.Add(table);

            Chunk linbreak2 = new Chunk(new LineSeparator(1f, 100f, BaseColor.DARK_GRAY, Element.ALIGN_CENTER, -1));
            doc.Add(linbreak2);

            table2 = new PdfPTable(4);
            table2.DefaultCell.Border = 0;
            table2.AddCell(new Phrase("Description", textFont3));
            table2.AddCell(new Phrase("Quantity", textFont3));
            table2.AddCell(new Phrase("Rate", textFont3));
            table2.AddCell(new Phrase("Amount", textFont3));
            foreach (var item in model.BillingList)
            {
                //table2.SetWidths(new float[] { 12, 30 });
                table2.HorizontalAlignment = Element.ALIGN_CENTER;
                table2.AddCell(new Phrase(item.description, textFont));
                table2.AddCell(new Phrase(item.Quantity.ToString(), textFont));
                table2.AddCell(new Phrase(item.Rate.ToString(), textFont));
                table2.AddCell(new Phrase(item.Amount.ToString(), textFont));
                //table.AddCell(new Phrase(msg, textFont));
            }
            Chunk linbreak3 = new Chunk(new LineSeparator(1f, 100f, BaseColor.DARK_GRAY, Element.ALIGN_CENTER, -1));
            doc.Add(linbreak3);
            table2.AddCell(new Phrase("SubTotal", textFont3));
            table2.AddCell("");
            table2.AddCell("");
            table2.AddCell(new Phrase(model.BillingList.Sum(m => m.Amount).ToString(), textFont3));
            Chunk linbreak4 = new Chunk(new LineSeparator(1f, 100f, BaseColor.DARK_GRAY, Element.ALIGN_CENTER, -1));
            doc.Add(linbreak4);
            table2.AddCell(new Phrase("5% VAT", textFont3));
            table2.AddCell("");
            table2.AddCell("");
            table2.AddCell(new Phrase(fivepercent.ToString(), textFont3));
            Chunk linbreak5 = new Chunk(new LineSeparator(1f, 100f, BaseColor.DARK_GRAY, Element.ALIGN_CENTER, -1));
            doc.Add(linbreak5);
            table2.AddCell(new Phrase("TRANSPORTATION", textFont3));
            table2.AddCell("");
            table2.AddCell("");
            table2.AddCell(new Phrase(model.Trasport.Sum(m => m.TransportBill).ToString(), textFont3));

            Chunk linbreak6 = new Chunk(new LineSeparator(1f, 100f, BaseColor.DARK_GRAY, Element.ALIGN_CENTER, -1));
            doc.Add(linbreak6);

            table2.AddCell(new Phrase("GRAND TOTAL", textFont3));
            table2.AddCell("");
            table2.AddCell("");
            table2.AddCell(new Phrase(grandTotal.ToString(), textFont4));
            Chunk linbreak7 = new Chunk(new LineSeparator(1f, 100f, BaseColor.DARK_GRAY, Element.ALIGN_CENTER, -1));
            doc.Add(linbreak7);
            doc.Add(table2);
            // Closing the document
            doc.Close();
            //return Json(new { status = "Success" });
        }

        public ActionResult QuotationView()
        {
            ReceiptModel model = new ReceiptModel();
            try
            {
                model.Name = Session["Name"].ToString();
                model.eventdate = DateTime.Parse(Session["EventDate"].ToString()).Date;
                model.SetUpDate = DateTime.Parse(Session["SetupDate"].ToString()).Date;
                model.FullAddress = Session["Address"].ToString();
                TempData["month"] = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month).ToUpper();
                var mybill = Recieptbills(Session["customerkey"].ToString());
                ViewBag.data = mybill;
                model.BillingList = mybill;
                var mytrasport = Trasportbills(Session["customerkey"].ToString());
                model.Trasport = mytrasport;

                ConvertoPdf(model);

              string message ="Please find the attached invoice of your Trasactions with KFA Rentals";
              ToAdminEmail(Session["EmailAddress"].ToString(), message);
              //Session["EmailAddress"] = states.EmailAddress;

            }
            catch
            {
                TempData["customerempty"] = "Please Select at least one location";
                return RedirectToAction("CreateQuotation");
            }

           
            
            return View(model);
        }

       
        
        [HttpPost]
        public ActionResult CreateQuotation(QuotationModel model)
        {
            if (model.cutomerId==0 && Session["customerkey"]!=null)
            {
            var subcategory = context.Subcategories.Where(x=>x.Id==model.subcategoryId).SingleOrDefault();
            var product = context.Products.Where(x => x.Id == model.ProductId).SingleOrDefault();
            var category = context.Categories.Where(x => x.Id == model.categoryId).SingleOrDefault();
            BillItem items = new BillItem();
            items.Amount = model.Quantity * subcategory.Price;
            items.Description = product.PoductName +" "+category.CategoryName+" "+subcategory.SubCategory1;
            items.EntryDate = DateTime.Now.Date;
            items.Quantity = model.Quantity;
            items.Rate = subcategory.Price;
            items.Imagepath = subcategory.imagesPath;
            items.CustomerName = Session["customer"].ToString();
            items.SessionKey = Session["customerkey"].ToString();
            items.InvoiceNumber = string.Format("KFA"+Session["Number"].ToString());
            TempData["number"] = items.InvoiceNumber;
            context.BillItems.Add(items);
            context.SaveChanges();


            subcategory.Quantity = subcategory.Quantity - model.Quantity;
              try
                {
                    context.Entry(subcategory).State = EntityState.Modified;
                    context.SaveChanges();
                    //Session["Success"] = "Edited Created Sucessfully";
                }
              catch
              {
                  Session["fail"] = "Quantity Not Editted";
              }

            var list = CustomersList(model);
            model.cutomerList = CustomersList(model);
            model.ProductList = ProductList(model);
            List<Location> Reasons = new List<Location>();
            Reasons = context.Locations.ToList();
            model.Locations = new MultiSelectList(Reasons, "Value", "Location1", Reasons.Select(x => x.Value));
           

            }
            else
            {
                TempData["customerempty"] = "Please Select Customer";
                var list = CustomersList(model);
                model.cutomerList = CustomersList(model);
                model.ProductList = ProductList(model);
                return View(model);
            }
          
            return View(model);
        }
        [HttpGet]
        public ActionResult LoadGrid(QuotationModel model)
        {
                var mybill = bills(Session["customerkey"].ToString());
                ViewBag.data = mybill;
                model.BillingList = mybill;
                return Json(model.BillingList, JsonRequestBehavior.AllowGet);
        }
        public List<QuotationModel> bills(string sessionkey)
        {
            List<QuotationModel> list = new List<QuotationModel>();
            List<BillItem> objlist = context.BillItems.Where(x => x.SessionKey==sessionkey).ToList();

            foreach (var a in objlist)
            {
                QuotationModel model = new QuotationModel();
                model.BillId = a.Id;
                model.description= a.Description;
                model.EntryDate = a.EntryDate;
                model.Amount = a.Amount;
                model.Quantity = a.Quantity;
                model.Rate = a.Rate;
                list.Add(model);

            }
            return list;
        }

        public List<ReceiptModel> Recieptbills(string sessionkey)
        {
            List<ReceiptModel> list = new List<ReceiptModel>();
            List<BillItem> objlist = context.BillItems.Where(x => x.SessionKey == sessionkey).ToList();

            foreach (var a in objlist)
            {
                ReceiptModel model = new ReceiptModel();
             
                model.description = a.Description;
                model.Amount = a.Amount;
                model.Quantity = a.Quantity;
                model.Rate = a.Rate;
                model.imagePath = a.Imagepath;
                model.customerName = a.CustomerName;
                list.Add(model);

            }
            return list;
        }

        public List<ReceiptModel> Trasportbills(string sessionkey)
        {
            List<ReceiptModel> list = new List<ReceiptModel>();
            List<TransportBill> objlist = context.TransportBills.Where(x => x.SessionKey == sessionkey).ToList();

            foreach (var a in objlist)
            {
                ReceiptModel model = new ReceiptModel();

                model.TransportBill = a.TransportBill1;
               
                list.Add(model);

            }
            return list;
        }
        public void CleanController()
        {
            Session.Clear();
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetCustomerInfo(string customerId)
        {

            CleanController();
            Session["Number"] = String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);
            if (String.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException("");
            }
            int id = 0;
            bool isValid = Int32.TryParse(customerId, out id);
            var states = GetCustomerinfoById(id);
            Session["customer"] = states.FullName;
            Session["customerkey"] = Guid.NewGuid().ToString();
            var r = Session["customerkey"];
            Session["Name"] = states.FullName;
            Session["EmailAddress"] = states.EmailAddress;
            return Json(states,JsonRequestBehavior.AllowGet);
            
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


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSubCategoryBycatId(string categoryId)
        {
            if (String.IsNullOrEmpty(categoryId))
            {
                throw new ArgumentNullException("categoryId");
            }
            int id = 0;
            bool isValid = Int32.TryParse(categoryId, out id);
            var states = GetSubCatByCatId(id);
            var result = (from s in states
                          select new
                          {
                              id = s.Id,
                              name = s.SubCategory1
                          }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        } 
        private IEnumerable<SelectListItem> CustomersList(QuotationModel model)
        {

            List<Customer> products = new List<Customer>();
            products = context.Customers.Where(x=>x.IsActive==true).ToList();
            var list = from s in products
                       select new SelectListItem
                       {
                           Selected = s.Id.ToString()==model.cutomerId.ToString(),
                           Text = s.FullName,
                           Value = s.Id.ToString()
                       };

            return list;
        }
        private IEnumerable<SelectListItem> CategoryList(QuotationModel model)
        {

            List<Category> products = new List<Category>();
            products = context.Categories.ToList();
            var list = from s in products
                       select new SelectListItem
                       {
                           Selected = s.Id.ToString() == model.categoryId.ToString(),
                           Text = s.CategoryName,
                           Value = s.Id.ToString()
                       };
            SubCategoryList(model.categoryId);
            return list;
        }
        private IEnumerable<SelectListItem> ProductList(QuotationModel model)
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
            SubCategoryList(model.categoryId);
            return list;
        }

        public IList<Product> GetAllProducts()
        {
            var query = from products in context.Products
                        select products;
            var content = query.ToList<Product>();
            return content;
        }

        public IList<Category> GetAllCategory()
        {
            var query = from category in context.Categories
                        select category;
            var content = query.ToList<Category>();
            return content;
        }
        public IList<Category> GetAllCategoriesByProductId(int productId)
        {
            var query = from category in context.Categories
                        where category.PoductId==productId
                        select category;
            var content = query.ToList<Category>();
            return content;
        }
        public Customer GetCustomerinfoById(int customerId)
        {
            var r= context.Customers.Where(x => x.Id == customerId).ToList().SingleOrDefault();
            return r;
        }
        public IList<Subcategory> GetSubCatByCatId(int catId)
        {
            var query = from subcat in context.Subcategories
                        where subcat.CategoryId == catId
                        select subcat;
            var content = query.ToList<Subcategory>();
            return content;
        }
        private IEnumerable<SelectListItem> SubCategoryList(int Id)
        {

            
            List<Subcategory> products = new List<Subcategory>();
            products = context.Subcategories.Where(x=>x.CategoryId==Id).ToList();
            var list = from s in products
                       select new SelectListItem
                       {
                           Selected = s.Id.ToString() == Id.ToString(),
                           Text = s.SubCategory1,
                           Value = s.Id.ToString()
                       };

            return list;
        }
        [HttpGet]
        public ActionResult Location()
        {
            LocationModel model = new LocationModel();
            var list = LocationList();
            
            model.LocationList = list;
            return View(model);
        }
        [HttpPost]
        public ActionResult Location(LocationModel model)
        {
            try
            {
                Location loc = new Location();
                loc.Location1 = model.Location;
                loc.Value = decimal.Parse(model.Amount.ToString());
                context.Locations.Add(loc);
            }
            catch
            {
                TempData["locationd"] = "Transportation Fee cannot be empty";
                return RedirectToAction("Location");
            }
            context.SaveChanges();
            TempData["location"] = "Location Successfully Created";
            var list = LocationList();

            model.LocationList = list;
         
            return View(model);
        }


        public List<LocationModel> LocationList()
        {
            List<LocationModel> list = new List<LocationModel>();
            List<Location> objlist = context.Locations.ToList();

            foreach (var a in objlist)
            {
                LocationModel model = new LocationModel();
                model.Id = a.Id;
                model.Amount = a.Value.ToString();
                model.Location = a.Location1;
                list.Add(model);

            }
            return list;
        }



        [HttpGet]
        public ActionResult EditLocation(int Id)
        {
            var Loc = context.Locations.Where(x => x.Id == Id).SingleOrDefault();
           LocationModel model = new LocationModel();
           model.Amount = Loc.Value.ToString();
            model.Location = Loc.Location1;

            return View(model);
        }
        [HttpPost]
        public ActionResult EditLocation(LocationModel model)
        {
            if (model != null)
            {
                Location client = new Location();
                client.Id = model.Id;
                client.Location1 = model.Location;
                client.Value = decimal.Parse(model.Amount.ToString());
               
                try
                {
                    context.Entry(client).State = EntityState.Modified;
                    context.SaveChanges();
                    Session["Success"] = "Edited  Sucessfully";
                }
                catch
                {
                    Session["fail"] = "Location Not Editted";
                }

            }
            var list = LocationList();
            model.LocationList = list;
            return RedirectToAction("Location");
        }

    }
}