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
    public class StudentController : Controller
    {
        private readonly StudentRepository _repoStudent;
        private readonly ILogger<StudentController> _logger;
        private readonly MstCourseRepository _courseRepo;
        private readonly MstClassRepository _classRepo;
        private readonly string _baseUrl;
        public StudentController(ILogger<StudentController> logger,
            IOptions<AppSettings> config,
            MstClassRepository classRepo,
           MstCourseRepository courseRepo,
            StudentRepository repoStudent)
        {
            _repoStudent = repoStudent;
            _courseRepo = courseRepo;
            _classRepo = classRepo;
            _logger = logger;
            _baseUrl = config.Value.BaseUrl;
        }
        public IActionResult Index()
        {
            var model = new StudentViewModel();


            model.ClassList = _classRepo.GetAll()
                        .Select(x => new SelectListItem { Value = x.Name.ToString(), Text = x.Name })
                        .ToList();
            model.CourseList = _courseRepo.GetAll()
                        .Select(x => new SelectListItem { Value = x.CourseName.ToString(), Text = x.CourseName })
                        .ToList();

            ViewBag.BaseUrl = _baseUrl;
            //var data = _repoStudent.GetAll();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }

            //student.AdmissionDate = DateTime.Now;

            _repoStudent.Add(student);

            TempData["msg"] = "Student Added Successfully!";
            return RedirectToAction("Index");
        }


        public IActionResult List()
        {
            ViewBag.BaseUrl = _baseUrl;
            var data = _repoStudent.GetAll();


            return View(data);
        }

    }
}
