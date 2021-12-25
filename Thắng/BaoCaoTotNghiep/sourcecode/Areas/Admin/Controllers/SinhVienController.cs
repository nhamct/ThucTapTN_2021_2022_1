using PagedList;
using QLĐRenLuyen.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLĐRenLuyen.Areas.Admin.Controllers
{
    public class SinhVienController : Controller
    {
        private QLĐRenLuyenEntities db = new QLĐRenLuyenEntities();

        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var model = db.tbl_SinhVien.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
            ViewBag.lstLop = db.tbl_Lop.ToList();
            return View(model);
        }

        public JsonResult Delete(long ID)
        {

            try
            {
                var nh = db.tbl_SinhVien.Find(ID);
                db.tbl_SinhVien.Remove(nh);
                db.SaveChanges();
                return Json(new
                {
                    status = true
                });
            }
            catch
            {
                return Json(new
                {
                    status = false
                });
            }

        }

        [HttpPost]
        public ActionResult Add(tbl_SinhVien entity)
        {
            if (db.tbl_SinhVien.Count(x => x.MaSV == entity.MaSV) > 0)
            {
                TempData["add_success"] = "Thêm sinh viên viên KHÔNG thành công. MÃ SINH VIÊN bị trùng.";
                return RedirectToAction("Index");
            }
            db.tbl_SinhVien.Add(entity);
            db.SaveChanges();
            TempData["add_success"] = "Thêm sinh viên thành công";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(tbl_SinhVien entity)
        {
            var nh = db.tbl_SinhVien.Find(entity.ID);
            nh.TenSV = entity.TenSV;
            nh.MaSV = entity.MaSV;
            nh.Lop_ID = entity.Lop_ID;
            nh.LopTruong = entity.LopTruong;
            db.SaveChanges();
            TempData["add_success"] = "Sửa sinh viên thành công";
            return RedirectToAction("Index");
        }

        public JsonResult GetSinhVienByID(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var cn = db.tbl_SinhVien.Find(ID);
            return Json(cn, JsonRequestBehavior.AllowGet);
        }
    }
}