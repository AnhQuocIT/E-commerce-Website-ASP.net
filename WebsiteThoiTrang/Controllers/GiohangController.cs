using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteThoiTrang.Models;

namespace WebsiteThoiTrang.Controllers
{
    public class GiohangController : Controller
    {
        // GET: Giohang
        dbWebDataContext data = new dbWebDataContext();

        public List<Giohang> Laygiohang ()
        {
            List<Giohang> lgh = Session["Giohang"] as List<Giohang>;
            if(lgh==null)
            {
                lgh = new List<Giohang>();
                Session["Giohang"] = lgh;
            }
            return lgh;
        }

        public ActionResult Themgiohang(string MaSP, string URL)
        {
            List<Giohang> lgh = Laygiohang();
            Giohang sp = lgh.Find(n => n.MaSP == MaSP);
            if(sp == null)
            {
                sp = new Giohang(MaSP);
                lgh.Add(sp);
                return Redirect(URL);
            }
            else
            {
                sp.SoLuong++;
                return Redirect(URL);
            }
        }

        private int Tongsoluong()
        {
            int TongSL = 0;
            List<Giohang> lgh = Session["Giohang"] as List<Giohang>;
            if (lgh != null)
            {
                TongSL = lgh.Sum(n => n.SoLuong);
            }
            return TongSL;
        }

        private double TongTien()
        {
            double TT = 0;
            List<Giohang> lgh = Session["Giohang"] as List<Giohang>;
            if (lgh != null)
            {
                TT = lgh.Sum(n => n.ThanhTien);
            }
            return TT;
        }
        public ActionResult GioHang()
        {
            List<Giohang> lgh = Laygiohang();
            if (lgh.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Tongsoluong = Tongsoluong();
            ViewBag.Tongtien = TongTien();
            return View(lgh);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = Tongsoluong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }

        public ActionResult XoaGiohang(string MaSP)
        {
            List<Giohang> lgh = Laygiohang();
            Giohang sp = lgh.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp != null)
            {
                lgh.RemoveAll(n => n.MaSP == MaSP);
                return RedirectToAction("GioHang");
            }
            if (lgh.Count == 0)
            {
                return RedirectToAction("Index","Home");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CNGioHang(string MaSP, FormCollection collection)
        {
            List<Giohang> lgh = Laygiohang();
            Giohang sp = lgh.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp != null)
            {
                sp.SoLuong = int.Parse(collection["txtSoluong"].ToString());
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaAll()
        {
            List<Giohang> lgh = Laygiohang();
            lgh.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            if(Session["Taikhoan"]==null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap","Users");
            }

            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Giohang> lgh = Laygiohang();
            ViewBag.Tongsoluong = Tongsoluong();
            ViewBag.Tongtien = TongTien();
            return View(lgh);
        }

        public ActionResult TenUsers()
        {
            return PartialView();
        }

        public ActionResult ProfileUs()
        {
            tblUser us = (tblUser)Session["Taikhoan"];
            if (us == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(us);
        }

        [HttpGet]
        public ActionResult EditUs(int id)
        {
            tblUser us = (tblUser)Session["Taikhoan"];
            us = data.tblUsers.SingleOrDefault(n => n.usersID == id);
            if (us == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(us);
        }

        [HttpPost]
        [ActionName("EditUs")]
        public ActionResult XNLuuAd(int id)
        {
            tblUser us = (tblUser)Session["Taikhoan"];
            us = data.tblUsers.SingleOrDefault(n => n.usersID == id);
            if (ModelState.IsValid)
            {
                UpdateModel(us);
                data.SubmitChanges();
            }
            return RedirectToAction("ProfileUs");
        }

        public ActionResult SignOut()
        {
            Session["Taikhoan"] = null;
            Session["GioHang"] = null;
            return Redirect("/");
        }

        public ActionResult DatHang(FormCollection collection)
        {
            tblTran ddh = new tblTran();
            tblUser kh = (tblUser)Session["Taikhoan"];
            List<Giohang> lgh = Laygiohang();
            ddh.user_id = kh.usersID;
            ddh.created = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.delivery_date = DateTime.Parse(ngaygiao);
            ddh.amount = Decimal.Parse(collection["tien"]);
            ddh.payment = " ";
            ddh.payment_info = " ";
            ddh.note = collection["Ghichu"].ToString();
            ddh.security = " ";
            ddh.status = " ";
            data.tblTrans.InsertOnSubmit(ddh);
            data.SubmitChanges();

            foreach(var item in lgh)
            {
                tblOrder cthd = new tblOrder();
                cthd.Trans_id = ddh.id;
                cthd.product_id = item.MaSP;
                cthd.number = item.SoLuong;
                cthd.amount = (decimal)item.DonGia;
                cthd.status = " ";
                data.tblOrders.InsertOnSubmit(cthd);
            }
            data.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacnhanDH", "GioHang");
        }

        public ActionResult XacnhanDH()
        {
            return View();
        }
    }
}