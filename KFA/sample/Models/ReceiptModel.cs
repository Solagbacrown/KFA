using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sample.Models
{
    public class ReceiptModel
    {

        //public ReceiptModel()
        //{
        //    BillingList = new List<ReceiptModel>();
        //    Trasport = new List<ReceiptModel>();
        //}


        [Display(Name = "NAME OF CLIENT:")]
        public string Name{get; set;}
         [Display(Name = "DATE OF EVENT:")]
        public DateTime eventdate {get; set;}
         [Display(Name = "DATE OF SETUP:")]
        public DateTime SetUpDate{get; set;}
         public int Id { get; set; }
         public string EntryDate{ get; set; }
         public DateTime StartDate { get; set; }
         public DateTime EndDate { get; set; }
         [Display(Name = "ADDRESS OF DELIVERY:")]
        public string FullAddress { get; set; }
         public string description { get; set; }
         public decimal Rate { get; set; }

         [Display(Name = "Amount")]
         public decimal Amount{ get; set; }
         public decimal TransportBill { get; set; }
         public decimal Quantity { get; set; }
         [Display(Name = "QUOTATION NUMBER")]
         public string  InvoiceNumber { get; set; }
        [Display(Name = "Image")]
         public string imagePath { get; set; }
         [Display(Name = "CUSTOMER NAME")]
        public string customerName { get; set; }

         public string EmailAddress { get; set; }
        public List<ReceiptModel> BillingList { get; set; }
        public List<ReceiptModel> Trasport { get; set; }
    }
}