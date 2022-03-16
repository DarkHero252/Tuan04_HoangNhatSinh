using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tuan04_HoangNhatSinh.Models;

namespace Tuan04_HoangNhatSinh.Controllers
{
    public class GioHangController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();
        // GET: GioHang
        public List<Giohang> Laygiohang()
        {
            List<Giohang> LstGiohang = Session["Giohang"] as List<Giohang>;
            if (LstGiohang == null)
            {
                LstGiohang = new List<Giohang>();
                Session["Giohang"] = LstGiohang;
            }
            return LstGiohang;
        }

        public List<Sach> ListSach()
        {
            var all = data.Saches.ToList();
            return all;
        }

        public ActionResult ThemGioHang(int id, string strURL)
        {
            List<Giohang> LstGiohang = Laygiohang();
            Giohang sanpham = LstGiohang.Find(n => n.masach == id);
            if (sanpham == null)
            {
                sanpham = new Giohang(id);
                LstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }

        private int TongSotuong()
        {
            int tsl = 0;
            List<Giohang> LstGiohang = Session["GioHang"] as List<Giohang>;
            if (LstGiohang != null)
            {
                tsl = LstGiohang.Sum(n => n.iSoluong);
            }
            return tsl;
        }

        private int TongSoluongSanPham()
        {
            int tsl = 0;
            List<Giohang> LstGiohang = Session["GicHang"] as List<Giohang>;
            if (LstGiohang != null)
            {
                tsl = LstGiohang.Count;
            }
            return tsl;
        }

        private double TongTien()
        {
            double tt = 0;
            List<Giohang> LstGiohang = Session["Giohang"] as List<Giohang>;
            if (LstGiohang != null)
            {
                tt = LstGiohang.Sum(n => n.dthanhtien);
            }
            return tt;
        }

        public ActionResult Giohang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSotuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoluongSanPham();
            return View(lstGiohang);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSotuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoluongSanPham();
            return PartialView();
        }

        public ActionResult XoaGiohang(int id)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.masach == id);
            if(sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.masach == id);
                return RedirectToAction("Giohang");
            }
            return RedirectToAction("Giohang");
        }

        public ActionResult CapNhapGioHang(int id, FormCollection collection)
        {
            List<Giohang> listGioHang = Laygiohang();
            Giohang sanpham = listGioHang.SingleOrDefault(n => n.masach == id);
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(collection["txtSoLg"].ToString());
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaTatCaGioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("Giohang");
        }
    }
}