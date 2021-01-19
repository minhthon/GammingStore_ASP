using DoAnASP.Data;
using DoAnASP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DoAnASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly DpContext _context;

        public HomeController(DpContext context)
        {
            _context = context;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.ListCategory = _context.Category.ToList(); // danh sách product
            try
            {
                JObject us = JObject.Parse(HttpContext.Session.GetString("user"));
                ViewBag.UserName = us.SelectToken("Name").ToString();
            }
            catch
            {

            }
            base.OnActionExecuted(context);
        }
        public IActionResult Index()
        {
            ViewBag.ListProduct = _context.Product.ToList();

            if (HttpContext.Session.GetString("user") == null)
            {
                return View();
            }

            return View();
        }

        [HttpGet]
        public IActionResult GetProductByCate(int id)
        {
            var product = from p in _context.Product where p.IdCategory == id select p;
            ViewBag.ListProduct = product;
            return View();
        }
        [HttpGet]
        public IActionResult GetProductByClassify(string id)
        {
            var product = from p in _context.Product where p.Classify == id select p;
            ViewBag.ListProduct = product;
            return View();
        }
        [HttpPost]
        public IActionResult GetProductByName(string key_word)
        {
            var product = from p in _context.Product where p.ProductName.Contains(key_word) select p;
            ViewBag.ListProduct = product;
            return View();
        }
    }
}
