﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityProject.Controllers
{
    public class MemberController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
