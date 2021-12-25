using PagedList;
using QLĐRenLuyen.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLĐRenLuyen.Areas.Admin.Controllers
{
    public class LopController : Controller
    {
        private QLĐRenLuyenEntities db = new QLĐRenLuyenEntities();
        // GET: Admin/Lop
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var model = db.tbl_Lop.OrderByDescending(x => x.TenLop).ToPagedList(page, pageSize);
            ViewBag.lstGiaoVien = db.tbl_GiaoVien.ToList();
            ViewBag.lstLop = db.tbl_Lop.OrderByDescending(x => x.TenLop).ToList();
            return View(model);
        }

        public JsonResult Delete(long ID)
        {

            try
            {
                var nh = db.tbl_Lop.Find(ID);
                db.tbl_Lop.Remove(nh);
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
        public ActionResult Add(tbl_Lop entity)
        {
            db.tbl_Lop.Add(entity);
            db.SaveChanges();
            TempData["add_success"] = "Thêm năm học thành công";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(tbl_Lop entity)
        {
            var nh = db.tbl_Lop.Find(entity.ID);
            nh.TenLop = entity.TenLop;
            nh.GiaoVien_ID = entity.GiaoVien_ID;
            db.SaveChanges();
            TempData["add_success"] = "Sửa năm học thành công";
            return RedirectToAction("Index");
        }

        public JsonResult GetLopByID(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var cn = db.tbl_Lop.Find(ID);
            return Json(cn, JsonRequestBehavior.AllowGet);
        }
    }
}