using QLĐRenLuyen.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLĐRenLuyen.Models.DTO
{
    public class TieuChiDAO
    {
        private QLĐRenLuyenEntities db = new QLĐRenLuyenEntities();

        public string XepLoai(int diem)
        {
            string xeploai = "";
            if(diem >=90 && diem <= 100)
            {
                xeploai += "Xuất sắc";
            }
            else if (diem >= 80 && diem < 90)
            {
                xeploai += "Tốt";
            }
            else if(diem >= 65 && diem < 80)
            {
                xeploai += "Khá";
            }
            else if(diem >=50 && diem < 65)
            {
                xeploai += "Trung bình";
            }
            else if (diem >= 35 && diem < 50)
            {
                xeploai += "Yếu";
            }
            else
            {
                xeploai += "Kém";
            }
            return xeploai;
        }

        public List<TieuChiDTO> getTieuChiByHocKy(long HocKy_ID, long GiaoVien_ID)
        {
            var query = from sv in db.tbl_SinhVien
                        join tc in db.tbl_TieuChi on sv.ID equals tc.SinhVien_ID
                        join hk in db.tbl_HocKy on tc.HocKy_ID equals hk.ID
                        join lp in db.tbl_Lop on sv.Lop_ID equals lp.ID
                        join giaovien in db.tbl_GiaoVien on lp.GiaoVien_ID equals giaovien.ID
                        where lp.GiaoVien_ID == GiaoVien_ID && tc.HocKy_ID == HocKy_ID && tc.LoaiDiem == 3
                        select new TieuChiDTO()
                        {
                            MaSV = sv.MaSV,
                            TenSV = sv.TenSV,
                            TongDiem = tc.TongDiem,
                            XepLoai = tc.XepLoai
                        };
            return query.ToList();
        }
    }
}