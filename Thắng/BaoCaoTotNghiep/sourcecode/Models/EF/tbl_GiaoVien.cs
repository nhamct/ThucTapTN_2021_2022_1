//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QLĐRenLuyen.Models.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_GiaoVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_GiaoVien()
        {
            this.tbl_Lop = new HashSet<tbl_Lop>();
            this.tbl_TieuChi = new HashSet<tbl_TieuChi>();
        }
    
        public long ID { get; set; }
        public string MaGV { get; set; }
        public string MatKhau { get; set; }
        public string TenGV { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Lop> tbl_Lop { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_TieuChi> tbl_TieuChi { get; set; }
    }
}
