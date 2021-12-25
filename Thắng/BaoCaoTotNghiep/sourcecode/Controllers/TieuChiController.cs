using QLĐRenLuyen.Models.DTO;
using QLĐRenLuyen.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLĐRenLuyen.Controllers
{
    public class TieuChiController : Controller
    {
        private QLĐRenLuyenEntities db = new QLĐRenLuyenEntities();
        // GET: TieuChi
        public ActionResult ThanhCong()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChamDiem_SV(tbl_TieuChi entity)
        {
            entity.XepLoai = new TieuChiDAO().XepLoai((int)entity.TongDiem);
            entity.NgayLap = DateTime.Now;
            entity.LoaiDiem = 1;//Sinh viên tự chấm
            db.tbl_TieuChi.Add(entity);
            db.SaveChanges();

            return RedirectToAction("ThanhCong");
        }

        [HttpPost]
        public ActionResult ChamDiem_LT(tbl_TieuChi entity)
        {
            entity.XepLoai = new TieuChiDAO().XepLoai((int)entity.TongDiem);
            entity.NgayLap = DateTime.Now;
            entity.LoaiDiem = 2;//lớp trưởng chấm
            db.tbl_TieuChi.Add(entity);
            db.SaveChanges();
            TempData["notify_success"] = "Chấm điểm thành công";
            return Redirect("/SinhVien/ChamDiem_LT");
        }
    }
}