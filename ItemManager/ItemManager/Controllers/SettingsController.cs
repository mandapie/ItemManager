﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ItemManager.Controllers
{
    public class SettingsController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}