using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteThoiTrang.Models
{
        public class Giohang
        {
            dbWebDataContext data = new dbWebDataContext();
            public string MaSP { set; get; }
            public string TenSP { set; get; }
            public string AnhSP { set; get; }
            public Double DonGia { set; get; }
            public int SoLuong { set; get; }

            public Double ThanhTien
            {
                get { return SoLuong * DonGia; }
            }

            public Giohang(string masp)
            {
                MaSP = masp;
                tblProduct sp = data.tblProducts.Single(n => n.id == MaSP);
                TenSP = sp.name;
                AnhSP = sp.image_link;
                DonGia = double.Parse(sp.price_after.ToString());
                SoLuong = 1;
            }
     }
}