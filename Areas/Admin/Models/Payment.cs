using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoAnASP.Areas.Admin.Models
{
    [Area("Admin")]
    public class Payment
    {
        [Key]
        public int IDPayment { get; set; }
        public string IDUser { get; set; }
        [Display(Name = "Phương thức thanh toán")]

        public string PaymentMethod { get; set; }
        [Display(Name = "Đơn vị vận chuyển")]
        [Required]

        public string ShippingUnit { get; set; }

        [Display(Name = "Ghi chú")]
        [Required]
        public string Note { get; set; }

        [Display(Name = "Địa chỉ giao hàng")]
        [Required]
        public string Address { get; set; }

        [Display(Name = "Giỏ hàng")]
        public string Cart { get; set; }
        public int Status { get; set; }
    }
}
