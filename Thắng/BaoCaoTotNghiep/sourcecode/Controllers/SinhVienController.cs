using QLĐRenLuyen.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLĐRenLuyen.Controllers
{
    public class SinhVienController : Controller
    {
        private QLĐRenLuyenEntities db = new QLĐRenLuyenEntities();
        // GET: SinhVien
        //Xem điểm sinh viên, k là lớp trưởng
        public ActionResult Index()
        {
            var sv = Session["sinhvien"] as tbl_SinhVien;
            ViewBag.DiemSV = db.tbl_TieuChi.Where(x => x.SinhVien_ID == sv.ID && x.LoaiDiem == 3).ToList();
            return View();
        }

        public ActionResult ChamDiem()
        {
            if(Session["giaovien"] != null)
            {
                TempData["error_login"] = "Bạn vui lòng đăng nhập tài khoản sinh viên để tự chấm điểm!!";
                return Redirect("/Login");
            }
            var sv = Session["sinhvien"] as tbl_SinhVien;
            var hocky = db.tbl_HocKy.FirstOrDefault(x => x.Status == true);
            if(hocky != null)
            {
                ViewBag.HocKy = hocky;
                //check xem sinh viên đã tự chấm điểm chưa, nếu chưa thì cho chấm điểm, còn đã chấm rồi thì k cho
                var check = db.tbl_TieuChi.Count(x => x.SinhVien_ID == sv.ID && x.LoaiDiem == 1 && x.HocKy_ID == hocky.ID);
                if (check > 0)
                {
                    return Redirect("/Home/Error");
                }
            }
            
            return View();
        }

        public ActionResult ChamDiem_LT()
        {
            if (Session["giaovien"] != null)
            {
                TempData["error_login"] = "Bạn vui lòng đăng nhập tài khoản sinh viên là lớp trưởng để chấm điểm cho lớp!!";
                return Redirect("/Login");
            }
            var sv = Session["sinhvien"] as tbl_SinhVien;
            if(sv.LopTruong == false)
            {
                TempData["error_login"] = "Bạn vui lòng đăng nhập tài khoản sinh viên là lớp trưởng để chấm điểm cho lớp!!";
                return Redirect("/Login");
            }
            ViewBag.lstSinhVien = db.tbl_SinhVien.Where(x => x.Lop_ID == sv.Lop_ID).ToList();
            var hocky = db.tbl_HocKy.FirstOrDefault(x => x.Status == true);
            if(hocky != null)
            {
                ViewBag.HocKy = hocky;

                ViewBag.DiemTuCham = db.tbl_TieuChi.Where(x => x.tbl_SinhVien.Lop_ID == sv.Lop_ID && x.LoaiDiem == 1 && x.HocKy_ID == hocky.ID).ToList();
                ViewBag.DiemLopCham = db.tbl_TieuChi.Where(x => x.tbl_SinhVien.Lop_ID == sv.Lop_ID && x.LoaiDiem == 2 && x.HocKy_ID == hocky.ID).ToList();
            }
            return View();
        }

        public ActionResult Lop_ChamDiem(long ID)
        {
            if (Session["giaovien"] != null)
            {
                TempData["error_login"] = "Bạn vui lòng đăng nhập tài khoản sinh viên để tự chấm điểm!!";
                return Redirect("/Login");
            }
            var sv = Session["sinhvien"] as tbl_SinhVien;
            if (sv.LopTruong == false)
            {
                TempData["error_login"] = "Bạn vui lòng đăng nhập tài khoản sinh viên là lớp trưởng để chấm điểm cho lớp!!";
                return Redirect("/Login");
            }

            
            var hocky = db.tbl_HocKy.FirstOrDefault(x => x.Status == true);
            ViewBag.HocKy = hocky;
            ViewBag.SinhVien = db.tbl_TieuChi.FirstOrDefault(x => x.SinhVien_ID == ID && x.LoaiDiem == 1 && x.HocKy_ID == hocky.ID);
            return View();
        }

        //Lớp duyệt điểm tự chấm của sinh viên
        public ActionResult DuyetDiem(long ID)
        {
            var hocky = db.tbl_HocKy.FirstOrDefault(x => x.Status == true);
            var sv = db.tbl_TieuChi.FirstOrDefault(x => x.SinhVien_ID == ID && x.HocKy_ID == hocky.ID && x.LoaiDiem == 1);
            sv.LoaiDiem = 2;
            sv.NgayLap = DateTime.Now;
            db.tbl_TieuChi.Add(sv);
            db.SaveChanges();
            TempData["notify_success"] = "Duyệt điểm thành công";
            return RedirectToAction("ChamDiem_LT");
        }
    }
}