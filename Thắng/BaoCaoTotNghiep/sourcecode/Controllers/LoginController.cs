using QLĐRenLuyen.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLĐRenLuyen.Controllers
{
    public class LoginController : Controller
    {
        private QLĐRenLuyenEntities db = new QLĐRenLuyenEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login_SinhVien(tbl_SinhVien entity)
        {
            if(db.tbl_SinhVien.Count(x => x.MaSV == entity.MaSV && x.MatKhau == entity.MatKhau) > 0)
            {
                Session["sinhvien"] = db.tbl_SinhVien.FirstOrDefault(x => x.MaSV == entity.MaSV && x.MatKhau == entity.MatKhau);
                return Redirect("/");
            }
            else
            {
                TempData["error_login"] = "Mã sinh viên hoặc mật khẩu không đúng, vui lòng đăng nhập lại!!";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Login_GiaoVien(tbl_GiaoVien entity)
        {
            if (db.tbl_GiaoVien.Count(x => x.MaGV == entity.MaGV && x.MatKhau == entity.MatKhau) > 0)
            {
                if(entity.MaGV == "Admin")
                {
                    Session["admin"] = "Administrator";
                    return Redirect("/Admin/GiaoVien");
                }
                else
                {
                    Session["giaovien"] = db.tbl_GiaoVien.FirstOrDefault(x => x.MaGV == entity.MaGV && x.MatKhau == entity.MatKhau);
                    return Redirect("/");
                }
            }
            else
            {
                TempData["error_login"] = "Mã giáo viên hoặc mật khẩu không đúng, vui lòng đăng nhập lại!!";
                return RedirectToAction("Index");
            }
        }

        //Đổi mật khẩu
        public ActionResult ChangePass_SinhVien()
        {
            if (Session["sinhvien"] == null)
            {
                TempData["error_login"] = "Bạn vui lòng đăng nhập!!";
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult ChangePass_GiaoVien()
        {
            if (Session["giaovien"] == null)
            {
                TempData["error_login"] = "Bạn vui lòng đăng nhập!!";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ChangePass_SV(tbl_SinhVien entity, string New_Pass, string Old_Pass)
        {
            var user = db.tbl_SinhVien.Find(entity.ID);
            if (user.MatKhau.Trim() != Old_Pass)
            {
                TempData["error_change"] = "Mật khẩu cũ không đúng, vui lòng kiểm tra lại!";
                return RedirectToAction("ChangePass_SinhVien");
            }
            else
            {
                user.MatKhau = New_Pass;
            }
            db.SaveChanges();
            TempData["error_login"] = "Đổi mật khẩu thành công, vui lòng đăng nhập lại!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ChangePass_GV(tbl_GiaoVien entity, string New_Pass, string Old_Pass)
        {
            var user = db.tbl_GiaoVien.Find(entity.ID);
            if (user.MatKhau.Trim() != Old_Pass)
            {
                TempData["error_change"] = "Mật khẩu cũ không đúng, vui lòng kiểm tra lại!";
                return RedirectToAction("ChangePass_GiaoVien");
            }
            else
            {
                user.MatKhau = New_Pass;
            }
            db.SaveChanges();
            TempData["error_login"] = "Đổi mật khẩu thành công, vui lòng đăng nhập lại!";
            return RedirectToAction("Index");
        }

        public ActionResult Logout_SinhVien()
        {
            Session["sinhvien"] = null;
            return RedirectToAction("Index");
        }

        public ActionResult Logout_GiaoVien()
        {
            Session["giaovien"] = null;
            return RedirectToAction("Index");
        }

        public ActionResult Logout_Admin()
        {
            Session["admin"] = null;
            return RedirectToAction("Index");
        }
    }
}