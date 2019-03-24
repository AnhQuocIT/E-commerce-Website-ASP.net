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

        [HttpGet]
        public ActionResult ThemAd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemAd(tblAdmin ad)
        {
            if (ModelState.IsValid)
            {
                ad.created = DateTime.Now;
                data.tblAdmins.InsertOnSubmit(ad);
                data.SubmitChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult ProfileAd()
        {
            tblAdmin ad = (tblAdmin)Session["TKadmin"];
            if (ad == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ad);
        }

        [HttpGet]
        public ActionResult EditAd(int id)
        {
            tblAdmin ad = (tblAdmin)Session["TKadmin"];
            ad = data.tblAdmins.SingleOrDefault(n => n.id == id);
            if (ad == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ad);
        }

        [HttpPost]
        [ActionName("EditAD")]
        public ActionResult XNLuuAd(int id)
        {
            tblAdmin ad = (tblAdmin)Session["TKadmin"];
            ad = data.tblAdmins.SingleOrDefault(n => n.id == id);
            if (ModelState.IsValid)
            {
                UpdateModel(ad);
                data.SubmitChanges();
            }
            return RedirectToAction("ProfileAd");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult SignOut()
        {
            Session["TKadmin"] = null;
            return RedirectToAction("Login");
        }

        [HttpPost]

        public ActionResult Login(FormCollection collection)
        {
            var ten = collection["username"];
            var pass = collection["pass"];
            tblAdmin ad = data.tblAdmins.SingleOrDefault(n => n.username == ten && n.userpassword == pass);
            if (ad != null)
            {
                Session["TKadmin"] = ad;
                return RedirectToAction("Index", "Admin");
            }
            else
                ViewBag.Thongbao = "*Tên đăng nhập hoặc mật khẩu không đúng!";
            return View();
        }

                public ActionResult Trans()
        {
            return View(data.tblTrans.ToList());
        }

        [HttpGet]
        public ActionResult Delete(string id)
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

        [HttpPost]
        [ActionName("Delete")]

        public ActionResult XNXoa(string id)
        {
            tblProduct sp = data.tblProducts.SingleOrDefault(n => n.id == id);
            ViewBag.Masp = sp.id;
                if (sp == null)
                    {
                        Response.StatusCode = 404;
                        return null;
                    }

            data.tblProducts.DeleteOnSubmit(sp);
            data.SubmitChanges();
            return RedirectToAction("SanPham");
        }

        public ActionResult DetailsUser(int id)
        {
            tblUser us = data.tblUsers.SingleOrDefault(n => n.usersID == id);
            if(us == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(us);
        }

        public ActionResult DetailsTrans(int id)
        {
            return View(data.tblOrders.Where(a=>a.Trans_id == id).ToList());
        }

        [HttpGet]
        public ActionResult DeleteUser(int id)
        {
            tblUser us = data.tblUsers.SingleOrDefault(n => n.usersID == id);

            if (us == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(us);
        }

        [HttpPost]
        [ActionName("DeleteUser")]
        public ActionResult XNXoaU(int id)
        {
            tblUser us = data.tblUsers.SingleOrDefault(n => n.usersID == id);

            if (us == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.tblUsers.DeleteOnSubmit(us);
            data.SubmitChanges();
            return RedirectToAction("Users");
        }

         [HttpGet]
        public ActionResult DeleteTrans(int id)
        {
            tblTran tr = data.tblTrans.SingleOrDefault(n => n.id == id);

            if (tr == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tr);
        }

        [HttpPost]
        [ActionName("DeleteTrans")]
        public ActionResult XNXoaT(int id)
        {
            tblTran tr = data.tblTrans.SingleOrDefault(n => n.id == id);

            if (tr == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.tblTrans.DeleteOnSubmit(tr);
            
            data.SubmitChanges();
            return RedirectToAction("Trans");
        }

        [HttpGet]
        public ActionResult EditUser(int id)
        {
            tblUser us = data.tblUsers.SingleOrDefault(n => n.usersID == id);
            if (us == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(us);
        }

        [HttpPost]
        [ActionName("EditUser")]
        public ActionResult XNLuuUser(int id)
        {
            tblUser us = data.tblUsers.SingleOrDefault(n => n.usersID == id);
            if (ModelState.IsValid)
            {
                UpdateModel(us);
                data.SubmitChanges();
            }
            return RedirectToAction("Users");
        }

        [HttpGet]
        public ActionResult EditTrans(int id)
        {
            tblTran tr = data.tblTrans.SingleOrDefault(n => n.id == id);
            if (tr == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tr);
        }

        [HttpPost]
        [ActionName("EditTrans")]
        public ActionResult XNLuuT(int id, FormCollection col)
        {
            tblTran tr = data.tblTrans.SingleOrDefault(n => n.id == id);
            if (ModelState.IsValid)
            {
                UpdateModel(tr);
                data.SubmitChanges();
            }
            return RedirectToAction("Trans");
        }

        public ActionResult Users()
        {
            return View(data.tblUsers.ToList());
        }
	
    }
}