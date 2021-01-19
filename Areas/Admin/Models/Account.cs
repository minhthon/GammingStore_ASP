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
    public class Account
    {
        [Key]
        [Column(TypeName = "varchar(50)")]
        public string Email { get; set; }
        [StringLength(maximumLength: 50, MinimumLength = 5, ErrorMessage = "Tên người dùng có ít nhất 5 ký tự và nhiều nhất 50 ký tự")]
        [Required]
        [Column(TypeName = "varchar(255)")]
        [Display(Name = "Tên người dùng")]
        public string Name { get; set; }
        [Column(TypeName = "varchar(255)")]
        [Required]
        public string Password { get; set; }
        [Display(Name = "Hình ảnh")]
        [Column(TypeName = "nvarchar(255)")]
        [StringLength(255)]
        public string Image { get; set; }
        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }
        [Display(Name = "Vai trò")]
        public string Role { get; set; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessage = "Địa chỉ người dùng có ít nhất 5 ký tự và nhiều nhất 100 ký tự")]
        public string Address { get; set; }
    }
}
