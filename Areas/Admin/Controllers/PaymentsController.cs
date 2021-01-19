using DoAnASP.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAnASP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PaymentsController : Controller
    {
        private readonly DpContext _context;

        public PaymentsController(DpContext context)
        {
            _context = context;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.ListCategory = _context.Category.ToList(); // danh sách product
            ViewBag.ListProduct = _context.Product.ToList();
            ViewBag.Payment = _context.Payment.ToList();
            try
            {
                JObject us = JObject.Parse(HttpContext.Session.GetString("user"));
                ViewBag.Email = us.SelectToken("Email").ToString();
               

            }
            catch { }
            base.OnActionExecuted(context);
        }
        public IActionResult Index()
        {

            if (HttpContext.Session.GetString("admin") == null)
            {
                return Redirect("/Admin/Login");
            }
            JObject us = JObject.Parse(HttpContext.Session.GetString("admin"));
            ViewBag.Name = us.SelectToken("Name").ToString();
            return View();
        }
    }
}
