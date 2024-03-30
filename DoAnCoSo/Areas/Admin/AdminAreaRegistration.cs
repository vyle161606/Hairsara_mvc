using System.Web.Mvc;

namespace DoAnCoSo.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Barbers", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "DoAnCoSo.Areas.Admin.Controllers" }
                );
        }
    }
}