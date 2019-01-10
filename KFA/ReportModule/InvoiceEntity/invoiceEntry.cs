using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportModule.InvoiceEntity
{
   public  class invoiceEntry
    {
       public decimal transport { get; set; }
       public decimal AmountSum { get; set; }
       public decimal tenpercent { get; set; }
       public decimal grandTotal { get; set; }
       public string invoiceNumber { get; set; }

       public string Name { get; set; }
       public string eventDate { get; set; }
       public string setupDate { get; set; }
       public string FullAddress { get; set; }

       public string imagepath { get; set; }
       public string description { get; set; }
       public string Quantity { get; set; }
       public string Rate { get; set; }
       public string Amount { get; set; }

       
    }
}
