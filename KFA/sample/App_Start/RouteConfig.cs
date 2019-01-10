using System.Web.Mvc;
using System.Web.Routing;

namespace IdentitySample
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute("GetCategoryByProductId",
                            "quotation/getCategoryByProductId/",
                            new { controller = "Quotation", action = "GetCategoryByProductId" },
                            new[] { "sample.Controllers" });

            routes.MapRoute("GetSubCategoryBycatId",
                          "quotation/getSubCategoryBycatId/",
                          new { controller = "Quotation", action = "GetSubCategoryBycatId" },
                          new[] { "sample.Controllers" });

            routes.MapRoute("",
                        "quotation/getSubCategoryBycatId/",
                        new { controller = "Quotation", action = "GetSubCategoryBycatId" },
                        new[] { "sample.Controllers" });

            routes.MapRoute("GetCustomerInfo",
                         "quotation/getCustomerInfo/",
                         new { controller = "Quotation", action = "GetCustomerInfo" },
                         new[] { "sample.Controllers" });

            routes.MapRoute("CreateQuotation",
                        "quotation/createQuotation/",
                        new { controller = "Quotation", action = "CreateQuotation" },
                        new[] { "sample.Controllers" });

            routes.MapRoute("LoadGrid",
                       "quotation/loadGrid/",
                       new { controller = "Quotation", action = "LoadGrid" },
                       new[] { "sample.Controllers" });


            routes.MapRoute("ConvertoPdf",
                      "quotation/convertoPdf",
                      new { controller = "Quotation", action = "ConvertoPdf", model = UrlParameter.Optional },
                      new[] { "sample.Controllers" });
        }
    }
}