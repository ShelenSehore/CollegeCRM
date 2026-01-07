using ClosedXML.Excel;
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
using System.IO;
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
        public IActionResult Index(int? id)
        {
            var model = new StudentViewModel();
            if (id == null)
                return View("List");

            var varStudentDetail = _repoStudentRegi.GetById(id.Value);
            if (varStudentDetail != null)
            {
                model.StudentName = varStudentDetail.Name;
                model.FatherName = varStudentDetail.FatherName;
                model.MotherName = varStudentDetail.MotherName;
                model.DOB = varStudentDetail.DOB;
                model.AdmissionFormNo = varStudentDetail.FormNo;
                model.SchoolarNo = varStudentDetail.SchoNo.ToString();
                //model.EnRollNo = varStudentDetail.Session;
                model.Year = varStudentDetail.Year;
                model.Subject = varStudentDetail.Subject;
                model.Course = varStudentDetail.Course;
                model.Caste = varStudentDetail.Caste;
                model.Gender = varStudentDetail.Gender;
                model.MobileNoOne = varStudentDetail.MobileNo;
            }

            model.ClassList = _classRepo.GetAll()
                        .Select(x => new SelectListItem {
                            Value = x.Name.ToString(), 
                            Text = x.Name ,
                            Selected = x.Name == model.Year
                        })
                        .ToList();

          
            model.Class = model.Year;

            model.SubjectList = _courseRepo.GetAll()
                        .Select(x => new SelectListItem {
                            Value = x.CourseName.ToString(), 
                            Text = x.CourseName ,
                            Selected = x.CourseName == model.Subject
                        })
                        .ToList();
            model.Subject = model.Subject;

            model.CourseList = _subjecctRepo.GetAll()
                       .Select(x => new SelectListItem { 
                           Value = x.Id.ToString(), 
                           Text = x.Course ,
                           Selected = x.Course == model.Course
                       })
                       .ToList();
            model.Course = model.Course;



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
            return RedirectToAction("List");
        }





        public IActionResult List()
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



            //--------Get List
            var data = _repoStudentRegi.GetAll();

            model.StudentList = data;

            return View(model);
        }

        public IActionResult SearchList(string name, string classes, string subject, string course, string regPvt)
        {
           

            //--------Get List
            var data = _repoStudentRegi.FilterList(name, classes, subject, course, regPvt);



            return Json(new { success = true, data = data });
        }


        public IActionResult Registration()
        {
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

        [HttpPost]
        public IActionResult RegistrationPost(StudentRegistration student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }
            
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


        public IActionResult RegistrationUpdate(int Id)
        {

           var getValue =  _repoStudentRegi.GetById(Id);

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


        [HttpPost]
        public IActionResult DownloadExcel(string name, string classes, string subject, string course, string regPvt)
        {
            try {

                var students = _repoStudentRegi.GetAll();
                // var students = _repoStudentRegi.FilterList(name, classes, subject, course, regPvt);
                using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Student List");

                // Header
                worksheet.Cell(1, 1).Value = "Id";
                worksheet.Cell(1, 2).Value = "Student Name";
                worksheet.Cell(1, 3).Value = "Mobile";
                worksheet.Cell(1, 4).Value = "Course";
                worksheet.Cell(1, 5).Value = "Mobile";

                worksheet.Row(1).Style.Font.Bold = true;

                int row = 2;
                foreach (var item in students)
                {
                    worksheet.Cell(row, 1).Value = item.Id;
                    worksheet.Cell(row, 2).Value = item.Name;
                    worksheet.Cell(row, 3).Value = item.MobileNo;
                    worksheet.Cell(row, 4).Value = item.Course;
                    worksheet.Cell(row, 5).Value = item.MobileNo;
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return File(
                        stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "StudentList.xlsx"
                    );
                }


            }

            }
            catch (Exception ex)
            {
                return null;
            }
            
        }



    }
}
