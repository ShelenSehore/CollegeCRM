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
        private readonly StudentRegistrationRepository _repoStudentRegi;
        private readonly ILogger<StudentController> _logger;
        private readonly MstCourseRepository _courseRepo;
        private readonly MstClassRepository _classRepo;
        private readonly string _baseUrl;
        private readonly MstSubjectRepository _subjecctRepo;
        public StudentController(ILogger<StudentController> logger,
            IOptions<AppSettings> config,
            MstClassRepository classRepo,
           MstCourseRepository courseRepo,
            MstSubjectRepository subjecctRepo,
            StudentRepository repoStudent,
             StudentRegistrationRepository repoStudentRegi)
        {
            _repoStudent = repoStudent;
            _repoStudentRegi = repoStudentRegi;
            _courseRepo = courseRepo;
            _classRepo = classRepo;
            _logger = logger;
            _baseUrl = config.Value.BaseUrl;
            _subjecctRepo = subjecctRepo;
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

            model.SubjectList = _subjecctRepo.GetAll()
                       .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Course })
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


        public IActionResult Registration()
        {
            var model = new StudentRegistrationForView();


            model.ClassList = _classRepo.GetAll()
                       .Select(x => new SelectListItem { Value = x.Name.ToString(), Text = x.Name })
                       .ToList();
            model.CourseList = _courseRepo.GetAll()
                        .Select(x => new SelectListItem { Value = x.CourseName.ToString(), Text = x.CourseName })
                        .ToList();

           
            model.SubjectList = _subjecctRepo.GetAll()
                     .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Class+ "/ " + x.Name + " / " + x.Course })
                     .ToList();

            ViewBag.BaseUrl = _baseUrl;
            //var data = _repoStudent.GetAll();
            return View(model);
        }

        [HttpPost]
        public IActionResult RegistrationPost(StudentRegistration student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }
            student.RegNo = 1;
            student.FormNo = 1;
            student.SchoNo = 1;
            student.Session = "tet";
            student.Subject = "tet";
            student.Course = "tet";
            student.CreateBy = "Admin";
            student.CreateDate = DateTime.Now.Date;
            student.IsMove = false;

            try {
                _repoStudentRegi.Add(student);
            }
            catch (Exception ex) 
            { 
            }


          

            TempData["msg"] = "Student Added Successfully!";
            return RedirectToAction("Index");
        }



    }
}
