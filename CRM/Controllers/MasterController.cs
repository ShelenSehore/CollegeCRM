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
    public class MasterController : Controller
    {
        private readonly ILogger<MasterController> _logger;
        private readonly MstClassRepository _classRepo;
        private readonly MstCourseRepository _courseRepo;

        public MasterController(ILogger<MasterController> logger, 
            MstClassRepository classRepo,
            MstCourseRepository courseRepo)
        {
            _classRepo = classRepo;
            _courseRepo = courseRepo;
            _logger = logger;
        }

        #region Class Master
        public IActionResult Index()
        {
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

        #endregion


        #region Course Master
        public IActionResult Course()
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

        #endregion

    }
}
