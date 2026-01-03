using CRM.Data;
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
    public class PaymentController : Controller
    {

        private readonly StudentRepository _repoStudent;
        private readonly StudentRegistrationRepository _repoStudentRegi;
        private readonly ILogger<PaymentController> _logger;
        private readonly MstCourseRepository _courseRepo;
        private readonly MstClassRepository _classRepo;
        private readonly string _baseUrl;
        private readonly MstSubjectRepository _subjecctRepo;

        public PaymentController(ILogger<PaymentController> logger,
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
            ViewBag.BaseUrl = _baseUrl;
            var model = new StudentListView();

            //----  MstClass = Year
            model.ClassList = _classRepo.GetAll()
                       .Select(x => new SelectListItem { Value = x.Name.ToString(), Text = x.Name })
                       .ToList();

            // --- MstSubject  = Subject
            model.CourseList = _courseRepo.GetAll()
                        .Select(x => new SelectListItem { Value = x.CourseName.ToString(), Text = x.CourseName })
                        .ToList();

            //---MstCourse = Class
            model.SubjectList = _subjecctRepo.GetAll()
                     .Select(x => new SelectListItem { Value = x.Course.ToString(), Text = x.Course })
                     .ToList();


            return View(model);
        }


        public IActionResult SearchList(string name, string classes, string subject, string course, string regPvt)
        {
            var data = _repoStudent.FilterList(name, classes, subject, course, regPvt);
            return Json(new { success = true, data = data });
        }



    }
}
