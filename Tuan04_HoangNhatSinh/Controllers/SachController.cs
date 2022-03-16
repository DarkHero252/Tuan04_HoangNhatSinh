using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tuan04_HoangNhatSinh.Models;

namespace Tuan04_HoangNhatSinh.Controllers
{

    public class SachController : Controller
    {
        // GET: Sach
        MyDataDataContext db = new MyDataDataContext();

        public ActionResult ListSach()
        {
            var all = db.Saches.ToList();
            return View(all);
        }

        public ActionResult Detail(int id)
        {
            var D_sach = db.Saches.Where(m => m.masach == id).First(); return View(D_sach);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, Sach s)
        {
            var E_tensach = collection["tensach"];
            var E_hinh = collection["hinh"];
            var E_giaban = Convert.ToDecimal(collection["giaban"]); var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycapnhat"]); var E_soluongton = Convert.ToInt32(collection["soluongton"]); if (string.IsNullOrEmpty(E_tensach))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                s.tensach = E_tensach.ToString();
                s.hinh = E_hinh.ToString();
                s.giaban = E_giaban;
                s.ngaycapnhat = E_ngaycapnhat;
                s.soluongton = E_soluongton;
                db.Saches.InsertOnSubmit(s);
                db.SubmitChanges();
                return RedirectToAction("ListSach");
            }
            return this.Create();
        }
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }

        public ActionResult Edit(int id)
        {
            var E_sach = db.Saches.First(m => m.masach == id); return View(E_sach);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var E_sach = db.Saches.First(m => m.masach == id);
            var E_tensach = collection["tensach"];
            var E_hinh = collection["hinh"];
            var E_giaban = Convert.ToDecimal(collection["giaban"]); var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycatnhat"]); var E_soluongton = Convert.ToInt32(collection["soluongton"]); E_sach.masach = id;
            if (string.IsNullOrEmpty(E_tensach))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                E_sach.tensach = E_tensach;
                E_sach.hinh = E_hinh;
                E_sach.giaban = E_giaban;
                E_sach.ngaycapnhat = E_ngaycapnhat;
                E_sach.soluongton = E_soluongton;
                UpdateModel(E_sach);
                db.SubmitChanges();
                return RedirectToAction("ListSach");
            }
            return this.Edit(id);
        }
        //----------------------------------------- 
        public ActionResult Delete(int id)
        {
            var D_sach = db.Saches.First(m => m.masach == id);
            return View(D_sach);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_sach = db.Saches.Where(m => m.masach == id).First(); db.Saches.DeleteOnSubmit(D_sach);
            db.SubmitChanges();
            return RedirectToAction("ListSach");
        }
    }

}