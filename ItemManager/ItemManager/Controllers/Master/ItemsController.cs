using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ItemManager.Controllers.Master
{
    public partial class MasterController : CommonController
    {
        public ActionResult Items()
        {
            return View();
        }
    }
}