using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteThoiTrang.Models;

namespace WebsiteThoiTrang.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        dbWebDataContext data = new dbWebDataContext();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, tblUser kh )
        {
            var hoten = collection["user_name"];
            var email = collection["user_email"];
            var dt = collection["user_phone"];
            var dc = collection["user_address"];
            var pass = collection["user_password"];
            var pass_ap = collection["user_password_accept"];

            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "*Họ tên không được để trống";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi2"] = "*Email không được để trống";
            }
            else if (String.IsNullOrEmpty(dt))
            {
                ViewData["Loi3"] = "*Điện thoại không được để trống";
            }
            else if (String.IsNullOrEmpty(dc))
            {
                ViewData["Loi4"] = "*Địa chỉ không được để trống";
            }
            else if (String.IsNullOrEmpty(pass))
            {
                ViewData["Loi5"] = "*Mật khẩu không được để trống";
            }
            else if (String.IsNullOrEmpty(pass_ap))
            {
                ViewData["Loi6"] = "*Chưa nhập lại mật khẩu";
            }
            else
            {
                kh.name = hoten;
                kh.email = email;
                kh.phone = dt;
                kh.address = dc;
                kh.password = pass;
                kh.created = DateTime.Now;
                data.tblUsers.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("DangNhap");
            }
            return this.DangKy();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var emailDN = collection["email_DN"];
            var passDN = collection["password_DN"];
            if (String.IsNullOrEmpty(emailDN))
            {
                ViewData["Loi1"] = "*Email đăng nhập không được để trống";
            }
            else if (String.IsNullOrEmpty(passDN))
            {
                ViewData["Loi2"] = "*Password không được để trống";
            }
            else
            {
                tblUser kh = data.tblUsers.SingleOrDefault(n => n.email == emailDN && n.password == passDN);
                if (kh != null)
                {
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("GioHang", "GioHang");
                }
                else
                    ViewBag.Thongbao = "Email đăng nhập hoặc password không đúng!";
            }
            return View();
        }
    }
}