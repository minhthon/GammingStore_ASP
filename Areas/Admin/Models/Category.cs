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
    public class Category
    {
        [Key]
        public int IDCategory { get; set; }
        [Display(Name = "Tên loại sản phẩm")]
        [StringLength(50, MinimumLength = 5)]
        [Required]
        public string CategoryName { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        [StringLength(255)]
        public string Image { get; set; }
        public bool Status { get; set; }
    }
}
