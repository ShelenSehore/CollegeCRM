using CRM.Data;
using CRM.Models;
using CRM.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Controllers
{
    public class YearController : Controller
    {
        private readonly ILogger<YearController> _logger;
        private readonly MstYearRepository _yearRepo;
        private readonly string _baseUrl;

        public YearController(ILogger<YearController> logger,
            IOptions<AppSettings> config,
            MstYearRepository yearRepo)
        {
            _yearRepo = yearRepo;
            _logger = logger;
            _baseUrl = config.Value.BaseUrl;
        }


        #region Year Master 
        public IActionResult Index()   //----------Year
        {
            ViewBag.BaseUrl = _baseUrl;
            var data = _yearRepo.GetAll();

            return View(data);
        }


        [HttpPost]
        public IActionResult Create(string className)
        {
            if (string.IsNullOrWhiteSpace(className))
                return Json(new { success = false, message = "Class name is required" });

            var model = new MstYear
            {
                Name = className.ToUpper(),
                CreatedBy = "Admin",
                CreatedDateTime = DateTime.Now
            };

            _yearRepo.Add(model);

            return Json(new { success = true });
        }

        public IActionResult Get(int id)
        {
            var data = _yearRepo.GetById(id);
            return Json(data);
        }

        [HttpPost]
        public IActionResult Update(int id, string className)
        {
            var data = _yearRepo.GetById(id);

            if (data == null)
                return Json(new { success = false, message = "Year not found" });

            data.Name = className;
            data.CreatedBy = "Admin";
            data.CreatedDateTime = DateTime.Now;

            _yearRepo.Update(data);

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _yearRepo.Delete(id);
            return Json(new { success = true });
        }

        #endregion
    }
}
