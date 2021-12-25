using PagedList;
using QLĐRenLuyen.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLĐRenLuyen.Areas.Admin.Controllers
{
    public class HocVuController : Controller
    {
        private QLĐRenLuyenEntities db = new QLĐRenLuyenEntities();
        // GET: Admin/HocVu
        public ActionResult NamHoc(int page = 1, int pageSize = 10)
        {
            var model = db.tbl_NamHoc.OrderByDescending(x => x.NamHoc).ToPagedList(page, pageSize);
            return View(model);
        }

        public JsonResult Delete_NamHoc(long ID)
        {

            try
            {
                var nh = db.tbl_NamHoc.Find(ID);
                db.tbl_NamHoc.Remove(nh);
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

        public JsonResult ChangeStatus(long ID)
        {
            try
            {
                var hk = db.tbl_HocKy.Find(ID);
                if (hk.Status == true)
                    hk.Status = false;
                else
                {
                    hk.Status = true;
                    var lstHk = db.tbl_HocKy.Where(x => x.ID != ID).ToList();
                    foreach (var item in lstHk)
                    {
                        item.Status = false;
                    }
                }
                    
                
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
        public ActionResult Add_NamHoc(tbl_NamHoc entity)
        {
            db.tbl_NamHoc.Add(entity);
            db.SaveChanges();
            TempData["add_success"] = "Thêm năm học thành công";
            return RedirectToAction("NamHoc");
        }

        [HttpPost]
        public ActionResult Edit_NamHoc(tbl_NamHoc entity)
        {
            var nh = db.tbl_NamHoc.Find(entity.ID);
            nh.NamHoc = entity.NamHoc;
            db.SaveChanges();
            TempData["add_success"] = "Sửa năm học thành công";
            return RedirectToAction("NamHoc");
        }

        public JsonResult GetNamHocByID(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var cn = db.tbl_NamHoc.Find(ID);
            return Json(cn, JsonRequestBehavior.AllowGet);
        }

        public ActionResult HocKy(int page = 1, int pageSize = 10)
        {
            var model = db.tbl_HocKy.OrderByDescending(x => x.NamHoc_ID).ToPagedList(page, pageSize);
            ViewBag.lstNamHoc = db.tbl_NamHoc.ToList();
            return View(model);
        }

        public JsonResult Delete_HocKy(long ID)
        {

            try
            {
                var hk = db.tbl_HocKy.Find(ID);
                db.tbl_HocKy.Remove(hk);
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
        public ActionResult Add_HocKy(tbl_HocKy entity)
        {
            db.tbl_HocKy.Add(entity);
            db.SaveChanges();
            TempData["add_success"] = "Thêm học kỳ thành công";
            return RedirectToAction("HocKy");
        }

        [HttpPost]
        public ActionResult Edit_HocKy(tbl_HocKy entity)
        {
            var nh = db.tbl_HocKy.Find(entity.ID);
            nh.HocKy = entity.HocKy;
            nh.NamHoc_ID = entity.NamHoc_ID;
            db.SaveChanges();
            TempData["add_success"] = "Sửa học kỳ thành công";
            return RedirectToAction("HocKy");
        }

        public JsonResult GetHocKyByID(long ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var cn = db.tbl_HocKy.Find(ID);
            return Json(cn, JsonRequestBehavior.AllowGet);
        }

    }
}