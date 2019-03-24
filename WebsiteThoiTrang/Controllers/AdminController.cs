using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteThoiTrang.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Data.Entity;

namespace WebsiteThoiTrang.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        dbWebDataContext data = new dbWebDataContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TenAdmin()
        {
            return PartialView();
        }

        //Tiếp theo ở đây
	
    }
}