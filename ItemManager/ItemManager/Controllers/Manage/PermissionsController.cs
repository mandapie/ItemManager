using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ItemManager.Controllers.Manage
{
    public partial class ManageController : CommonController
    {
        public ActionResult Permissions()
        {
            return View();
        }
    }
}