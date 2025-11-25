using CRM.Models;
using CRM.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Controllers
{
    public class CourseController : Controller
    {
        private readonly MstCourseRepository _courseRepo;
        private readonly ILogger<MasterController> _logger;
        public CourseController(ILogger<MasterController> logger,
           MstCourseRepository courseRepo)
        {
            _courseRepo = courseRepo;
            _logger = logger;
        }
        public IActionResult Index()
        {
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
