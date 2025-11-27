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
    public class CourseController : Controller
    {
        private readonly MstCourseRepository _courseRepo;
        private readonly ILogger<CourseController> _logger;
        private readonly string _baseUrl;
        public CourseController(ILogger<CourseController> logger,
             IOptions<AppSettings> config,
           MstCourseRepository courseRepo)
        {
            _courseRepo = courseRepo;
            _logger = logger;
            _baseUrl = config.Value.BaseUrl;
        }
        public IActionResult Index()
        {
            ViewBag.BaseUrl = _baseUrl;
            var data = _courseRepo.GetAll();


            return View(data);
        }

        [HttpPost]
        public IActionResult CoursePost(string className)
        {
            if (string.IsNullOrWhiteSpace(className))
                return Json(new { success = false, message = "Course name is required" });

            var model = new MstCourse
            {
                CourseName = className,
                CreatedBy = "Admin",
                CreatedDatetime = DateTime.Now
            };

            _courseRepo.Add(model);

            return Json(new { success = true });
        }

        public IActionResult GetCourse(int id)
        {
            var data = _courseRepo.GetById(id);
            return Json(data);
        }

        [HttpPost]
        public IActionResult UpdateCourse(int id, string className)
        {
            var data = _courseRepo.GetById(id);

            if (data == null)
                return Json(new { success = false, message = "Class not found" });

            data.CourseName = className;
            data.UpdatedBy = "Admin";
            data.UpdatedDatetime = DateTime.Now;

            _courseRepo.Update(data);

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult DeleteCourse(int id)
        {
            _courseRepo.Delete(id);
            return Json(new { success = true });
        }


    }
}
