using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ItemManager.Controllers
{
    public class BaseController : Controller
    {
        protected override void Initialize(RequestContext controllerContext)
        {
            base.Initialize(controllerContext);

            string command = Convert.ToString(controllerContext.RouteData.Values["action"]);

            if (command != "CheckLastAccessServerTime" && command != "UserLogout")
            {
                UserSession.LastAccessServerTime = DateTime.Now;
            }

            if (IsPostBack() == false)
            {
                AddJavaScript();
            }
        }

        protected bool IsPostBack()
        {
            bool isPost = string.Compare(Request.HttpMethod, "POST",
               StringComparison.CurrentCultureIgnoreCase) == 0;
            /*Cannot compare the Url with UrlReferrer, because when the request is from ajax, the result will always false.
            if (Request.UrlReferrer == null) return false;
            bool isSameUrl = string.Compare(Request.Url.AbsolutePath,
               Request.UrlReferrer.AbsolutePath,
               StringComparison.CurrentCultureIgnoreCase) == 0;
            
            return isPost && isSameUrl;
            */
            return isPost;
        }

        private void AddJavaScript()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("//<![CDATA[");

            if (UserSession.User != null)
            {
                builder.AppendLine("var _LoginUserID = \"" + UserSession.User.userid + "\";");
                builder.AppendLine("var _LoginUserName = \"" + UserSession.User.name + "\";");
                //builder.AppendLine("var _LoginUserRole = " + UserSession.User.RoleID + ";");
                builder.AppendLine("var _LoginUserRole = " + UserSession.User.user_role + ";");
                builder.AppendLine("var _LoginUserRoleName = \"" + UserSession.User.user_role_name + "\";");

                builder.AppendLine("var USERROLE_USER = 10;");
                builder.AppendLine("var USERROLE_ADMIN = 20;");
            }

            builder.AppendLine("//]]>");
            ViewBag.DynamicScripts = builder.ToString();
        }

        public ActionResult UserLogout(string SignOutType)
        {
            try
            {
                if (SignOutType == "AutoSignOut")
                {
                    DateTime LastAccessServerTime = UserSession.LastAccessServerTime;
                    TimeSpan ts = DateTime.Now - LastAccessServerTime;
                    int res = Session.Timeout - (int)ts.TotalMinutes;
                    if (res >= 1)
                    {
                        //Continute Login.
                        UserSession.LastAccessServerTime = DateTime.Now;
                        return Json("ContinueLogin");
                    }
                    else if (res < 1)
                    {
                        System.Web.HttpContext.Current.Session.RemoveAll();
                        UserSession.User = null;
                    }
                }
                else
                {
                    System.Web.HttpContext.Current.Session.RemoveAll();
                    UserSession.User = null;
                }
                return Json("SignOut");
            }
            catch (Exception ex)
            {
                ErrorLog.Insert(ex);
                return Json(ex.ToString());
            }
        }

        public ActionResult CheckLastAccessServerTime()
        {
            DateTime LastAccessServerTime = UserSession.LastAccessServerTime;
            TimeSpan ts = DateTime.Now - LastAccessServerTime;
            int rest = Session.Timeout - (int)ts.TotalMinutes;
            if (rest <= 1)
            {
                //popup dialog.
                return Json("ShowDialog");
            }
            return Json("SetTimeout");
        }

        public ActionResult GetSessionTimeout()
        {
            try
            {
                int timeout = Session.Timeout;
                return Json(timeout);
            }
            catch (Exception ex)
            {
                ErrorLog.Insert(ex);
                return Json(ex.ToString());
            }
        }

        public ActionResult CheckUser()
        {
            return View();
        }

        public ActionResult CheckAdmin()
        {
            return View();
        }
    }
}