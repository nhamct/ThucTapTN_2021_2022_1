using QLĐRenLuyen.Models.DTO;
using QLĐRenLuyen.Models.EF;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLĐRenLuyen.Controllers
{
    public class GiaoVienController : Controller
    {
        private QLĐRenLuyenEntities db = new QLĐRenLuyenEntities();
        // GET: GiaoVien
        public ActionResult Index()
        {
            if (Session["sinhvien"] != null)
            {
                TempData["error_login"] = "Bạn vui lòng đăng nhập tài khoản giáo viên để chấm điểm lớp!!";
                return Redirect("/Login");
            }
            var gv = Session["giaovien"] as tbl_GiaoVien;
            var lop = db.tbl_Lop.FirstOrDefault(x => x.GiaoVien_ID == gv.ID);
            ViewBag.lop = lop;
            ViewBag.lstSinhVien = db.tbl_SinhVien.Where(x => x.Lop_ID == lop.ID).ToList();

            var query = from sv in db.tbl_SinhVien
                        join tc in db.tbl_TieuChi on sv.ID equals tc.SinhVien_ID
                        join hk in db.tbl_HocKy on tc.HocKy_ID equals hk.ID
                        join lp in db.tbl_Lop on sv.Lop_ID equals lp.ID
                        join giaovien in db.tbl_GiaoVien on lp.GiaoVien_ID equals giaovien.ID
                        where lp.GiaoVien_ID == gv.ID && tc.LoaiDiem == 3
                        select new TieuChiDTO()
                        {
                            TenLop = sv.tbl_Lop.TenLop,
                            MaSV = sv.MaSV,
                            TenSV = sv.TenSV,
                            TongDiem = tc.TongDiem,
                            XepLoai = tc.XepLoai,
                            HocKy_ID = hk.ID,
                            HocKy = hk.HocKy,
                            NamHoc = hk.tbl_NamHoc.NamHoc
                        };
            ViewBag.LstDiemRL = query.OrderByDescending(x => x.HocKy_ID).ToList();
            ViewBag.LstHocKy = db.tbl_HocKy.OrderByDescending(x => x.ID).ToList();
            var hocky = db.tbl_HocKy.FirstOrDefault(x => x.Status == true);
            if(hocky != null)
            {
                ViewBag.HocKy = hocky;

                ViewBag.DiemTuCham = db.tbl_TieuChi.Where(x => x.tbl_SinhVien.Lop_ID == lop.ID && x.LoaiDiem == 1 && x.HocKy_ID == hocky.ID).ToList();
                ViewBag.DiemLopCham = db.tbl_TieuChi.Where(x => x.tbl_SinhVien.Lop_ID == lop.ID && x.LoaiDiem == 2 && x.HocKy_ID == hocky.ID).ToList();
                ViewBag.DiemGVCham = db.tbl_TieuChi.Where(x => x.tbl_SinhVien.Lop_ID == lop.ID && x.LoaiDiem == 3 && x.HocKy_ID == hocky.ID).ToList();
            }
            return View();
        }

        public ActionResult ChamDiem(long ID)
        {
            if (Session["sinhvien"] != null)
            {
                TempData["error_login"] = "Bạn vui lòng đăng nhập tài khoản giáo viên để chấm điểm lớp!!";
                return Redirect("/Login");
            }
            var hocky = db.tbl_HocKy.FirstOrDefault(x => x.Status == true);
            ViewBag.HocKy = hocky;

            ViewBag.TuChamDiem = db.tbl_TieuChi.FirstOrDefault(x => x.SinhVien_ID == ID && x.LoaiDiem == 1 && x.HocKy_ID == hocky.ID);
            ViewBag.LopChamDiem = db.tbl_TieuChi.FirstOrDefault(x => x.SinhVien_ID == ID && x.LoaiDiem == 2 && x.HocKy_ID == hocky.ID);
            return View();
        }

        [HttpPost]
        public ActionResult ChamDiem_GVCN(tbl_TieuChi entity)
        {
            entity.XepLoai = new TieuChiDAO().XepLoai((int)entity.TongDiem);
            entity.NgayLap = DateTime.Now;
            entity.LoaiDiem = 3;//Giáo viên tự chấm
            db.tbl_TieuChi.Add(entity);
            db.SaveChanges();
            TempData["notify_success"] = "Chấm điểm thành công";
            return Redirect("/GiaoVien");
        }

        public ActionResult Export_Pdf(long ID, long GiaoVien_ID)//ID : id học kỳ
        {
            ViewBag.LstDiemRL = new TieuChiDAO().getTieuChiByHocKy(ID, GiaoVien_ID);
            ViewBag.Lop = db.tbl_Lop.FirstOrDefault(x => x.GiaoVien_ID == GiaoVien_ID);
            ViewBag.HocKy = db.tbl_HocKy.Find(ID);
            return View();
        }

        //In pdf
        public ActionResult Print_Pdf(long ID)
        {
            var gv = Session["giaovien"] as tbl_GiaoVien;
            var report = new ActionAsPdf("Export_Pdf", new { ID = ID, GiaoVien_ID = gv.ID });
            return report;
        }

        //giáo viên duyệt điểm tự chấm của sinh viên
        public ActionResult DuyetDiem(long ID)
        {
            var hocky = db.tbl_HocKy.FirstOrDefault(x => x.Status == true);
            var sv = db.tbl_TieuChi.FirstOrDefault(x => x.SinhVien_ID == ID && x.HocKy_ID == hocky.ID && x.LoaiDiem == 2);
            sv.LoaiDiem = 3;
            sv.NgayLap = DateTime.Now;
            db.tbl_TieuChi.Add(sv);
            db.SaveChanges();
            TempData["notify_success"] = "Duyệt điểm thành công";
            return RedirectToAction("Index");
        }
    }
}