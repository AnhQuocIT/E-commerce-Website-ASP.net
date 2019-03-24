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

        public ActionResult SanPham(int? page)
        {
            int numpage = (page ?? 1);
            int pagesize = 5;
            //return View(data.tblProducts.ToList());
            return View(data.tblProducts.ToList().OrderBy(n => n.id).ToPagedList(numpage, pagesize));
        }

        public ActionResult Catalog()
        {
            return View(data.tblCatalogs.ToList());
        }

        [HttpGet]
        public ActionResult ThemSP()
        {
            ViewBag.catalog_id = new SelectList(data.tblCatalogs.ToList().OrderBy(n => n.name), "id", "name");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]

        public ActionResult ThemSP(tblProduct sp, HttpPostedFileBase fileup)
        {
            ViewBag.catalog_id = new SelectList(data.tblCatalogs.ToList().OrderBy(n => n.name), "id", "name");
            if (fileup == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh sản phẩm";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {

                    var fileName = Path.GetFileName(fileup.FileName);
                    var path = Path.Combine(Server.MapPath("~/imagesSP"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại!";
                    }
                    else
                    {
                        fileup.SaveAs(path);
                    }
                    sp.image_link = fileName;
                    sp.created = DateTime.Now;
                    data.tblProducts.InsertOnSubmit(sp);
                    data.SubmitChanges();
                }
                return RedirectToAction("SanPham");
            }
        }

        [HttpGet]
        public ActionResult ThemCalg()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemCalg(tblCatalog cal)
        {
            if (ModelState.IsValid)
            {
                data.tblCatalogs.InsertOnSubmit(cal);
                data.SubmitChanges();
            }
            return RedirectToAction("Catalog");
        }

        public ActionResult Details(string id)
        {
            tblProduct sp = data.tblProducts.SingleOrDefault(n => n.id == id);
            ViewBag.Masp = sp.id;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        [HttpGet]
        public ActionResult DeleteCalg(string id)
        {
            tblCatalog cal = data.tblCatalogs.SingleOrDefault(n => n.id == id);

            if (cal == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(cal);
        }

        [HttpPost]
        [ActionName("DeleteCalg")]
        public ActionResult XNXoaC(string id)
        {
            tblCatalog cal = data.tblCatalogs.SingleOrDefault(n => n.id == id);

            if (cal == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.tblCatalogs.DeleteOnSubmit(cal);
            data.SubmitChanges();
            return RedirectToAction("Catalog");
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            tblProduct sp = data.tblProducts.SingleOrDefault(n => n.id == id);
            ViewBag.Masp = sp.id;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.catalog_id = new SelectList(data.tblCatalogs.ToList().OrderBy(n => n.name), "id", "name", sp.catalog_id);
            return View(sp);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]

        public ActionResult XNLuu(string id, HttpPostedFileBase fileup)
        {

            tblProduct sp = data.tblProducts.SingleOrDefault(n => n.id == id);
            ViewBag.catalog_id = new SelectList(data.tblCatalogs.ToList().OrderBy(n => n.name), "id", "name");
            //if (fileup == null)
            //{
            //    ViewBag.Thongbao = "Vui lòng chọn ảnh bìa!";
            //    return View();
            //}
            //else { 
            if (ModelState.IsValid)
            {
                var fileName = Path.GetFileName(fileup.FileName);
                var path = Path.Combine(Server.MapPath("~/imagesSP"), fileName);
                if (System.IO.File.Exists(path))
                {
                    ViewBag.Thongbao = "Hình ảnh đã tồn tại!";
                }
                else
                {
                    fileup.SaveAs(path);
                }
                sp.image_link = fileName;
                UpdateModel(sp);
                data.SubmitChanges();
            }
            //}
            return RedirectToAction("SanPham");

        }

        [HttpGet]
        public ActionResult EditCalg(string id)
        {
            tblCatalog cal = data.tblCatalogs.SingleOrDefault(n => n.id == id);
            if (cal == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(cal);
        }

        [HttpPost]
        [ActionName("EditCalg")]
        public ActionResult XNLuuCalg(string id)
        {
            tblCatalog cal = data.tblCatalogs.SingleOrDefault(n => n.id == id);
            if (ModelState.IsValid)
            {
                UpdateModel(cal);
                data.SubmitChanges();
            }
            return RedirectToAction("Catalog");
        }

	
    }
}