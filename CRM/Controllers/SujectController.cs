using CRM.Data;
using CRM.Models;
using CRM.ModelsForView;
using CRM.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Controllers
{
    public class SujectController : Controller
    {
        private readonly MstSubjectRepository _subjecctRepo;
        private readonly MstCourseRepository _courseRepo;
        private readonly MstClassRepository _classRepo;
        private readonly ILogger<SujectController> _logger;
        private readonly string _baseUrl;

        public SujectController(ILogger<SujectController> logger,
           IOptions<AppSettings> config,
           MstSubjectRepository subjecctRepo,
           MstClassRepository classRepo,
           MstCourseRepository courseRepo)
        {
            _subjecctRepo = subjecctRepo;
            _courseRepo = courseRepo;
            _classRepo = classRepo;
            _logger = logger;
            _baseUrl = config.Value.BaseUrl;
        }
        public IActionResult Index()
        {
            ViewBag.BaseUrl = _baseUrl;
            var model = new SubjectViewModel();
           
            
            model.ClassList = _classRepo.GetAll()
                        .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
                        .ToList();

            model.CourseList = _courseRepo.GetAll()
                        .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.CourseName })
                        .ToList();

            model.SubjectList = _subjecctRepo.GetAll();
            return View(model);

            //    var list =
            //(from s in _context.MstSubject
            // join c in _context.MstClass on s.ClassId equals c.Id
            // join co in _context.MstCourse on s.CourseId equals co.Id
            // select new SubjectViewModel
            // {
            //     Id = s.Id,
            //     SubjectName = s.Name,
            //     ClassName = c.Name,
            //     CourseName = co.CourseName
            // }).ToList();




        }

        [HttpPost]
        public IActionResult CreateSubject(SubjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            MstSubject subject = new MstSubject
            {
                Name = model.Name,               // Subject Name
                Class = model.SelectedClass ,      // Selected Class
                Course = model.SelectedCourse
            };

            _subjecctRepo.Add(subject);

            return RedirectToAction("index");
        }

        public IActionResult GetCourse(int id)
        {
            var data = _subjecctRepo.GetById(id);
            return Json(data);
        }

        [HttpPost]
        public IActionResult UpdateSubject(int id, string className)
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
        public IActionResult DeleteSubject(int id)
        {
            _subjecctRepo.Delete(id);
            return Json(new { success = true });
        }
    }
}
