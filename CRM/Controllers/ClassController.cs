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
    public class ClassController : Controller
    {
        private readonly ILogger<ClassController> _logger;
        private readonly MstClassRepository _classRepo;
        private readonly string _baseUrl;


        public ClassController(ILogger<ClassController> logger,
            IOptions<AppSettings> config,
            MstClassRepository classRepo)
        {
            _classRepo = classRepo;
            _logger = logger;
            _baseUrl = config.Value.BaseUrl;
        }


        public IActionResult Index()
        {
            ViewBag.BaseUrl = _baseUrl;
            var data = _classRepo.GetAll();

            return View(data);
        }



        [HttpPost]
        public IActionResult Create(string className)
        {
            if (string.IsNullOrWhiteSpace(className))
                return Json(new { success = false, message = "Class name is required" });

            var model = new MstClass
            {
                Name = className,
                CreateBy = "Admin",
                CreateDatetime = DateTime.Now
            };

            _classRepo.Add(model);

            return Json(new { success = true });
        }

        public IActionResult Get(int id)
        {
            var data = _classRepo.GetById(id);
            return Json(data);
        }

        [HttpPost]
        public IActionResult Update(int id, string className)
        {
            var data = _classRepo.GetById(id);

            if (data == null)
                return Json(new { success = false, message = "Class not found" });

            data.Name = className;
            data.UpdatedBy = "Admin";
            data.UpdateDatetime = DateTime.Now;

            _classRepo.Update(data);

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _classRepo.Delete(id);
            return Json(new { success = true });
        }





    }
}
