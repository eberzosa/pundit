using System.Web.Mvc;
using System.Web.Security;

namespace Pundit.Web.Controllers
{
   public class AccountController : Controller
   {
      //
      // GET: /Account/

      public ActionResult Index()
      {
         //return View();
         return null;
      }

      public ActionResult Login(FormCollection form)
      {
         string login = form["Login"];
         string password = form["Password"];

         if (Membership.ValidateUser(login, password))
         {
            FormsAuthentication.RedirectFromLoginPage(login, false);
         }
         else
         {
            ViewBag.ErrorMessage = "todo";
         }
         return null;
      }
   }
}
