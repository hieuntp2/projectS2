using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectS3.Models
{
    public class ThongTinNguoiDungMuaHang
    {       
        [Required]
        [Display(Name = "Địa chỉ giao hàng")]
        public string DiaChiGiao { get; set; }

        [Required]
        [Display(Name = "Thời gian giao hàng")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [validateTimeInWork]
        [validateMinDay]
        public System.DateTime ThoiGianGiao { get; set; }

        [Required]
        [Display(Name = "Điện thoại liên lạc")]
        public string SoDienThoai { get; set; }

        [Required]
        [Display(Name = "Họ và Tên")]
        public string HoTen { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public class SanPhamTrongGioHang
    {
        public int id { get; set; }
        public int soluong { get; set; }
        public int idbosanpham { get; set; }
        public string size { get; set; }
        public string color { get; set; }
        public float dongia { get; set; }
    }

    public class validateMinDay : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dt = DateTime.Parse(value.ToString());
        
            if ((dt - DateTime.Now).TotalDays <= 1)
            {              
                ErrorMessage = "Ngày giao hàng phải kể sau hôm nay ít nhất 1 ngày!";
                return false;
            }
               

            // eliminate other invalid values, etc
            // if contains valid hour for your business logic, etc

            return true;
        }
    }

    public class validateTimeInWork : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dt = DateTime.Parse(value.ToString());

            if (dt.Hour <= 7 || dt.Hour >= 21)
            {
                ErrorMessage = "Giờ giao hàng từ 7h sáng đến 9h tối!";
                return false;                
            }               

            // eliminate other invalid values, etc
            // if contains valid hour for your business logic, etc

            return true;
        }
    }
}
