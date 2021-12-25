using PagedList;
using QLĐRenLuyen.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLĐRenLuyen.Areas.Admin.Controllers
{
    public class GiaoVienController : Controller
    {
        private QLĐRenLuyenEntities db = new QLĐRenLuyenEntities();

        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var model = db.tbl_GiaoVien.OrderByDescending(x => x.TenGV).ToPagedList(page, pageSize);
            return View(model);
        }

        public JsonResult Delete(long ID)
        {

            try
            {
                var nh = db.tbl_GiaoVien.Find(ID);
                db.tbl_GiaoVien.Remove(nh);
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
        public ActionResult Add(tbl_GiaoVien entity)
        {
            if(db.tbl_GiaoVien.Count(x => x.MaGV == entity.MaGV) > 0)
            {
                TempData["add_success"] = "Thêm giáo viên không thành công. Mã giáo viên bị trùng.";
                return RedirectToAction("Index");
            }

            db.tbl_GiaoVien.Add(entity);
            db.SaveChanges();
            TempData["add_success"] = "Thêm giáo viên thành công";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(tbl_GiaoVien entity)
        {
            var nh = db.tbl_GiaoVien.Find(entity.ID);
            nh.TenGV = entity.TenGV;
            nh.MaGV = entity.MaGV;
            db.SaveChanges();
            TempData["add_success"] = "Sửa giáo viên thành công";
            return RedirectToAction("Index");
        }

        public JsonResult GetGiaoVienByID(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var cn = db.tbl_GiaoVien.Find(ID);
            return Json(cn, JsonRequestBehavior.AllowGet);
        }
    }
}