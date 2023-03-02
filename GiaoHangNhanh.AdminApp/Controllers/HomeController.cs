
using GiaoHangNhanh.AdminApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace GiaoHangNhanh.AdminApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel();

            model.PageTitle = "Tổng quan";
            model.Breadcrumbs = new List<string>() { "Tổng quan" };

            return View(model);
        }
    }
}
