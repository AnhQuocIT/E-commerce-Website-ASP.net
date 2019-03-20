using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteThoiTrang.Models;
using PagedList.Mvc;
using PagedList;

namespace WebsiteThoiTrang.Controllers
{
    public class TimKiemController : Controller
    {
        dbWebDataContext data = new dbWebDataContext();
        // GET: TimKiem
        [HttpPost]
        public ActionResult KQTimKiem(FormCollection collection, int? page)
        {
            String chuoi = collection["txtsearch"].ToString();
            List<tblProduct> lsp = data.tblProducts.Where(n => n.name.Contains(chuoi)).ToList();
            ViewBag.Chuoi = chuoi;
            int pageNum = (page ?? 1);
            int pageSize = 9;
            if (lsp.Count == 0)
            {
                ViewBag.Thongbao = "Không tìm thấy sản phẩm nào!";
                return View(data.tblProducts.OrderBy(n => n.name).ToPagedList(pageNum,pageSize));
            }
            ViewBag.Thongbao = "Đã tìm thấy "+ lsp.Count + " kết quả!";
            return View(lsp.OrderBy(n=>n.name).ToPagedList(pageNum,pageSize));
        }

        [HttpGet]
        public ActionResult KQTimKiem(String chuoi, int? page)
        {
            List<tblProduct> lsp = data.tblProducts.Where(n => n.name.Contains(chuoi)).ToList();
            ViewBag.Chuoi = chuoi;
            int pageNum = (page ?? 1);
            int pageSize = 9;
            if (lsp.Count == 0)
            {
                ViewBag.Thongbao = "Không tìm thấy sản phẩm nào!";
                return View(data.tblProducts.OrderBy(n => n.name).ToPagedList(pageNum, pageSize));
            }
            ViewBag.Thongbao = "Đã tìm thấy " + lsp.Count + " kết quả!";
            return View(lsp.OrderBy(n => n.name).ToPagedList(pageNum, pageSize));
        }
        //Tìm kiếm sản phẩm trong Admin
        [HttpPost]
        public ActionResult TimKiem(FormCollection collection, int? page)
        {
            String chuoi = collection["searchAD"].ToString();
            List<tblProduct> lsp = data.tblProducts.Where(n => n.name.Contains(chuoi)).ToList();
            ViewBag.Chuoi = chuoi;
            int pageNum = (page ?? 1);
            int pageSize = 6;
            if (lsp.Count == 0)
            {
                ViewBag.Thongbao = "Không tìm thấy sản phẩm nào!";
                return View(data.tblProducts.OrderBy(n => n.name).ToPagedList(pageNum, pageSize));
            }
            ViewBag.Thongbao = "Đã tìm thấy " + lsp.Count + " kết quả!";
            return View(lsp.OrderBy(n => n.name).ToPagedList(pageNum, pageSize));
        }

        [HttpGet]
        public ActionResult TimKiem(String chuoi, int? page)
        {
            List<tblProduct> lsp = data.tblProducts.Where(n => n.name.Contains(chuoi)).ToList();
            ViewBag.Chuoi = chuoi;
            int pageNum = (page ?? 1);
            int pageSize = 9;
            if (lsp.Count == 0)
            {
                ViewBag.Thongbao = "Không tìm thấy sản phẩm nào!";
                return View(data.tblProducts.OrderBy(n => n.name).ToPagedList(pageNum, pageSize));
            }
            ViewBag.Thongbao = "Đã tìm thấy " + lsp.Count + " kết quả!";
            return View(lsp.OrderBy(n => n.name).ToPagedList(pageNum, pageSize));
        }
    }
}