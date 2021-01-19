 using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DoAnASP.Areas.Admin.Models
{
    [Area("Admin")]
    public class Product
    {
        [Key]
        public int IDProduct { get; set; }

        [Display(Name = "Tên Sản Phẩm")]
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Tên sản phẩm có ít nhất 5 ký tự và nhiều nhất 100 ký tự!")]
        public string ProductName { get; set; }

        [Display(Name = "Giá Sản Phẩm")]
        [Range(0, 200000000, ErrorMessage = "Giá thấp nhất là 0 VND và cao nhất là 200.000.000 VND")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "Hình ảnh")]
        [Column(TypeName = "nvarchar(255)")]
        [StringLength(255)]
        public string Image { get; set; }

        [Display(Name = "Phân loại")]
        public string Classify { get; set; }

        [Display(Name = "Chi tiết")]
        public string Description { get; set; }

        public int Status { get; set; }
        [Required]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }
        [Required]
        [Display(Name = "Nhà sản xuất")]

        public string Producer { get; set; } //NSX
        [Required]
        [Display(Name = "Nguồn góc xuất xứ")]

        public string Origin { get; set; }//Nguon goc
        [Required]
        [Display(Name = "Thời gian bảo hành")]
        public string WarrantyPeriod { get; set; }
        [Required]
        public string CPU { get; set; }
        [Required]
        public string Ram { get; set; }
        [Required]
        public string VGA { get; set; }
        [Required]
        [Display(Name = "Lưu trử")]
        public string Hard_drive { get; set; }
        [Required]
        [Display(Name = "Màn hình")]

        public string Display { get; set; }
        [Required]
        [Display(Name = "Kết nối")]

        public string Connector { get; set; } //Cong ket noi
        [Required]
        [Display(Name = "Âm thanh")]

        public string Audio { get; set; }
        [Required]

        public string Wifi { get; set; }
        [Required]
        public string Bluetooth { get; set; }
        [Required]
        [Display(Name = "Hệ điều hành")]

        public string OperatingSystem { get; set; }
        [Required]
        [Display(Name = "Pin")]

        public string Battery { get; set; }
        [Required]
        [Display(Name = "Cân nặng")]

        public string Weight { get; set; }
        [Required]
        [Display(Name = "Màu sắc")]

        public string Color { get; set; }
        [Required]
        [Display(Name = "Kích thước")]

        public string Size { get; set; }
        public int IdCategory { get; set; }
        [ForeignKey("IdCategory")]
        public Category category { get; set; }
    }
}
