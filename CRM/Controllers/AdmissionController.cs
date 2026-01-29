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
    public class AdmissionController : Controller
    {
        private readonly StudentRepository _repoStudent;
        private readonly StudentRegistrationRepository _repoStudentRegi;
        private readonly ILogger<AdmissionController> _logger;
        private readonly MstCourseRepository _courseRepo;
        private readonly MstClassRepository _classRepo;
        private readonly string _baseUrl;
        private readonly MstSubjectRepository _subjecctRepo;
        private readonly AcademicRepository _repoAcademic;
        private readonly MstSessionRepository _sessionRepo;
        private readonly MstYearRepository _yearRepo;

        public AdmissionController(ILogger<AdmissionController> logger,
            IOptions<AppSettings> config,
            MstClassRepository classRepo,
           MstCourseRepository courseRepo,
            MstSubjectRepository subjecctRepo,
            StudentRepository repoStudent,
             AcademicRepository repoAcademic,
              MstSessionRepository sessionRepo,
               MstYearRepository yearRepo,
             StudentRegistrationRepository repoStudentRegi)
        {
            _repoStudent = repoStudent;
            _repoStudentRegi = repoStudentRegi;
            _courseRepo = courseRepo;
            _classRepo = classRepo;
            _logger = logger;
            _baseUrl = config.Value.BaseUrl;
            _subjecctRepo = subjecctRepo;
            _repoAcademic = repoAcademic;
            _yearRepo = yearRepo;
            _sessionRepo = sessionRepo;
        }
        public IActionResult Index()
        {
            ViewBag.BaseUrl = _baseUrl;
            var model = new StudentListView();

            model.ClassList = _classRepo.GetAll()
                      .Select(x => new SelectListItem { Value = x.Name.ToString(), Text = x.Name })
                      .ToList();

            //----Year----
            model.YearList = _yearRepo.GetAll()
                       .Select(x => new SelectListItem
                       {
                           Value = x.Name.ToString(),
                           Text = x.Name
                       })
                       .ToList();

            ////----Session----
            //model.SessionList = _sessionRepo.GetAll()
            //           .Select(x => new SelectListItem
            //           {
            //               Value = x.Name.ToString(),
            //               Text = x.Name
            //           })
            //           .ToList();


            // --- MstSubject  = Subject
            model.CourseList = _courseRepo.GetAll()
                        .Select(x => new SelectListItem { Value = x.CourseName.ToString(), Text = x.CourseName })
                        .ToList();

            //---MstCourse = Class
            var varAllSubject = _subjecctRepo.GetAll();

            model.SubjectList = varAllSubject
                .GroupBy(x => x.Name)
                .Select(g => g.First())
                .Select(x => new SelectListItem
                {
                    Value = x.Name,
                    Text = x.Name
                })
                .ToList();



            //--------Get List
            var data = _repoStudentRegi.GetAll();

            model.StudentRegistrationssList = data;

            return View(model);
        }




        public IActionResult SearchList(string name, string classes, string year, string course, string regPvt)
        {


            //--------Get List
            var data = _repoStudentRegi.FilterList(name, classes, year, course, regPvt);



            return Json(new { success = true, data = data });
        }



        public IActionResult NewAdmission()
        {
            ViewBag.BaseUrl = _baseUrl;
            var model = new StudentRegistrationForView();

            model.ClassList = _classRepo.GetAll()
                       .Select(x => new SelectListItem { Value = x.Name.ToString(), Text = x.Name })
                       .ToList();

            //----Year----
            model.YearList = _yearRepo.GetAll()
                       .Select(x => new SelectListItem
                       {
                           Value = x.Name.ToString(),
                           Text = x.Name
                       })
                       .ToList();

            //----Session----
            model.SessionList = _sessionRepo.GetAll()
                       .Select(x => new SelectListItem
                       {
                           Value = x.Name.ToString(),
                           Text = x.Name
                       })
                       .ToList();


            // --- MstSubject  = Subject
            model.CourseList = _courseRepo.GetAll()
                        .Select(x => new SelectListItem { Value = x.CourseName.ToString(), Text = x.CourseName })
                        .ToList();

            //---MstCourse = Class
            var varAllSubject = _subjecctRepo.GetAll();

            model.SubjectList = varAllSubject
                .GroupBy(x => x.Name)
                .Select(g => g.First())
                .Select(x => new SelectListItem
                {
                    Value = x.Name,
                    Text = x.Name
                })
                .ToList();

            ViewBag.BaseUrl = _baseUrl;
            //var data = _repoStudent.GetAll();
            return View(model);
        }

        [HttpPost]
        public IActionResult NewAdmissionPost(StudentRegistrationForView student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }
            StudentRegistration obj = new StudentRegistration();

            obj.RegNo = student.RegNo;
            obj.FormNo = student.FormNo;
            obj.SchoNo = student.SchoNo;
            obj.Session = student.Session;
            obj.Year = student.Year;    //-----Year
            obj.Subject = student.Class;  //--------Class
            obj.Course = student.Course;  //-----Course
            obj.Sem = student.Sem;
            obj.RegPvt = student.RegPvt;
            obj.Status = student.Status;
            obj.Name = student.Name;
            obj.FatherName = student.FatherName;
            obj.MotherName = student.MotherName;
            obj.DOB = student.DOB;
            obj.Caste = student.Caste;
            obj.Gender = student.Gender;
            obj.MobileNo = student.MobileNo;
            obj.Scholership = student.Scholership;
            obj.CreateBy = "Admin";
            obj.CreateDate = DateTime.Now;
           
            try
            {
              var RegiSaveId =   _repoStudentRegi.SaveAndGetId(obj);

                //-------------Save Into Student-----table--------------
                Student stuObj = new Student();
                stuObj.AdmissionFormNo = student.FormNo;
                stuObj.Session = student.Session;
                stuObj.Year = student.Year;
                stuObj.Course = student.Course;
                stuObj.Class = student.Class;
                stuObj.StudentName = student.Name;
                stuObj.FatherName = student.FatherName;
                stuObj.MotherName = student.MotherName;
                stuObj.DOB = student.DOB;
                stuObj.Caste = student.Caste;
                stuObj.Gender = student.Gender;
                stuObj.MobileNoOne = student.MobileNo;
                stuObj.CreateBy = "Admin";
                stuObj.CreateDatetime = DateTime.Now;



                var StudentID =   _repoStudent.SaveAndGetId(stuObj);

                //-------------Acedemic Detail save-----------

                Academy objAcademy = new Academy();
                objAcademy.RegStudentId = RegiSaveId;
                objAcademy.StudentId = StudentID;
                objAcademy.SchoolName = student.SchoolName;
                objAcademy.PassingYear = student.PassingYear;
                objAcademy.Board = student.Board;
                objAcademy.MaxMark = student.MaxMark;
                objAcademy.ObtMark = student.ObtMark;
                objAcademy.Result = student.Result;
                objAcademy.Parcent = student.Parcent;
                objAcademy.CreatedBy = "Admin";
                objAcademy.CreatedDate = DateTime.Now;
                objAcademy.UpdatedDate = DateTime.Now;
                objAcademy.UpdatedBy = "Admin";

                var AcedemicId = _repoAcademic.SaveAndGetId(objAcademy);




            }
            catch (Exception ex)
            {
            }




            TempData["msg"] = "Student Added Successfully!";
            return RedirectToAction("Index");
        }


        public IActionResult RegistrationUpdate(int Id)
        {

            var getValue = _repoStudentRegi.GetById(Id);

            var model = new StudentRegistrationForView();



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

            ViewBag.BaseUrl = _baseUrl;
            //var data = _repoStudent.GetAll();
            return View(model);
        }


    }
}
