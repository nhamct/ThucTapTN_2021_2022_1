using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLĐRenLuyen.Models.DTO
{
    public class TieuChiDTO
    {
        public string TenLop { get; set; }
        public string MaSV { get; set; }
        public string TenSV { get; set; }
        public string HocKy { get; set; }
        public string NamHoc { get; set; }
        public string XepLoai { get; set; }
        public long HocKy_ID { get; set; }
        public Nullable<bool> LopTruong { get; set; }

        public Nullable<int> TongDiem { get; set; }
    }
}