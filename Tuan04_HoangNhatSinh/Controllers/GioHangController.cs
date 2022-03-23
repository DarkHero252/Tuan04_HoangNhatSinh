using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;
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
            List<Giohang> lstGioHang = Laygiohang();
            var sach = data.Saches.FirstOrDefault(p => p.masach == id);
            Giohang sanpham = lstGioHang.Find(p => p.masach == id);
            if (sanpham == null)
            {
                sanpham = new Giohang(id);
                lstGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                if (sanpham.iSoluong < sach.soluongton)
                {
                    sanpham.iSoluong++;
                    return Redirect(strURL);
                }
                else
                {
                    MessageBox.Show("Không có đủ sách đẻ bán");
                    return RedirectToAction("Index", "Home");
                }
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
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.masach == id);
                return RedirectToAction("Giohang");
            }
            return RedirectToAction("Giohang");
        }

        public ActionResult CapNhapGioHang(int id, FormCollection collection)
        {
            List<Giohang> listGioHang = Laygiohang();
            var sach = data.Saches.FirstOrDefault(p => p.masach == id);
            Giohang sanpham = listGioHang.SingleOrDefault(n => n.masach == id);
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(collection["txtSoLg"].ToString().Trim());
                if (sanpham.iSoluong > sach.soluongton)
                {
                    MessageBox.Show("Không còn đủ sách để bán");
                    sanpham.iSoluong = 1;
                }
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaTatCaGioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("Giohang");
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap","NguoiDung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "Sach");
            }
            List<Giohang> lstGioHang = Laygiohang();
            ViewBag.Tongsoluong = TongSotuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoluongSanPham();
            return View(lstGioHang);
        }
        public ActionResult DatHang(System.Web.Mvc.FormCollection collection)
        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            Sach s = new Sach();

            List<Giohang> gh = Laygiohang();
            var ngaygiao = string.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
            if (ngaygiao == "")
            {
                MessageBox.Show("Chưa có ngày giao");
            }
            else
            {
                dh.makh = kh.makh;
                dh.ngaydat = DateTime.Now;
                dh.ngaygiao = DateTime.Parse(ngaygiao);
                dh.giaohang = false;
                dh.thanhtoan = false;

                data.DonHangs.InsertOnSubmit(dh);
                data.SubmitChanges();
                foreach (var ele in gh)
                {
                    ChiTietDonHang ctdh = new ChiTietDonHang();
                    ctdh.madon = dh.madon;
                    ctdh.masach = ele.masach;
                    ctdh.soluong = ele.iSoluong;
                    ctdh.gia = (decimal)ele.giaban;
                    s = data.Saches.Single(n => n.masach == ele.masach);
                    s.soluongton -= ctdh.soluong;
                    data.SubmitChanges();
                    data.ChiTietDonHangs.InsertOnSubmit(ctdh);
                }
                data.SubmitChanges();
                Session["Giohang"] = null;
                return RedirectToAction("XacNhanDonHang", "GioHang");
            }
            return RedirectToAction("DatHang", "GioHang");
        }

        public ActionResult XacnhanDonhang()
        {
            return View();
        }
    }
}