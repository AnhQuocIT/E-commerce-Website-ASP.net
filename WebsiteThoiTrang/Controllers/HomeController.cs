using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteThoiTrang.Models;
using PagedList;
using PagedList.Mvc;

namespace WebsiteThoiTrang.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        dbWebDataContext data = new dbWebDataContext();
        public ActionResult Index(int ? page)
        {
            int pageSize = 9;
            int pageNum = (page ?? 1);
            return View(data.tblProducts.ToList().ToPagedList(pageNum,pageSize));
        }

        private List<tblProduct> Hangmoi(int count)
        {
            return data.tblProducts.OrderByDescending(a => a.created).Take(count).ToList();
        }

        public ActionResult Hangmoi()
        {
            var hm = Hangmoi(9);
            return View(hm);
        }

        private List<tblProduct> Hangchay(int count)
        {
            return data.tblProducts.OrderByDescending(a => a.count_buy).Take(count).ToList();
        }

        public ActionResult Hangchay()
        {
            var hc = Hangchay(9);
            return View(hc);
        }

        public ActionResult Help()
        {
            return View();
        }

        public ActionResult Catalogs()
        {
            var catalog = from cd in data.tblCatalogs select cd;
            return PartialView(catalog);
        }

        public ActionResult TheoCatalog(string id)
        {
            var sp = from s in data.tblProducts where s.catalog_id == id select s;
            return View(sp);
        }

        public ActionResult Details(string id)
        {
            var sp = from s in data.tblProducts
                     where s.id == id
                     select s;
            return View(sp.Single());
        }
    }
}