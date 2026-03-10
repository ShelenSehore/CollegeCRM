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
        private readonly MstFeeRepository _feeRepo;
        private readonly StudentFeeRepository _studentFeeRepo;

        public AdmissionController(ILogger<AdmissionController> logger,
            IOptions<AppSettings> config,
            MstClassRepository classRepo,
           MstCourseRepository courseRepo,
            MstSubjectRepository subjecctRepo,
            StudentRepository repoStudent,
             AcademicRepository repoAcademic,
              MstSessionRepository sessionRepo,
               MstYearRepository yearRepo,
                MstFeeRepository feeRepo,
                StudentFeeRepository studentFeeRepo,
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
            _feeRepo = feeRepo;
            _studentFeeRepo = studentFeeRepo;
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
            obj.Year = student.Year.ToUpper();    //-----Year
            obj.Subject = student.Class.ToUpper();  //--------Class
            obj.Course = student.Course.ToUpper();  //-----Course
            obj.Sem = student.Sem.ToUpper();
            obj.RegPvt = student.RegPvt.ToUpper();
            obj.Status = student.Status;
            obj.Name = student.Name.ToUpper();
            obj.FatherName = student.FatherName.ToUpper();
            obj.MotherName = student.MotherName.ToUpper();
            obj.DOB = student.DOB;
            obj.Caste = student.Caste;
            obj.Gender = student.Gender;
            obj.MobileNo = student.MobileNo;
            obj.Scholership = student.Scholership.ToString();
            obj.CreateBy = "Admin";
            obj.CreateDate = DateTime.Now;
           
            try
            {
              var RegiSaveId =   _repoStudentRegi.SaveAndGetId(obj);

                //-------------Save Into Student-----table--------------
                Student stuObj = new Student();
                stuObj.AdmissionFormNo = student.FormNo;
                stuObj.Session = student.Session;
                stuObj.Year = student.Year.ToUpper();
                stuObj.Course = student.Course.ToUpper();
                stuObj.Class = student.Class.ToUpper();
                stuObj.StudentName = student.Name.ToUpper();
                stuObj.FatherName = student.FatherName.ToUpper();
                stuObj.MotherName = student.MotherName.ToUpper();
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
                objAcademy.SchoolName = student.SchoolName.ToUpper();
                objAcademy.PassingYear = student.PassingYear.ToUpper();
                objAcademy.Board = student.Board.ToUpper();
                objAcademy.MaxMark = student.MaxMark;
                objAcademy.ObtMark = student.ObtMark;
                objAcademy.Result = student.Result.ToUpper();
                objAcademy.Parcent = student.Parcent;
                objAcademy.CreatedBy = "Admin";
                objAcademy.CreatedDate = DateTime.Now;
                objAcademy.UpdatedDate = DateTime.Now;
                objAcademy.UpdatedBy = "Admin";

                var AcedemicId = _repoAcademic.SaveAndGetId(objAcademy);

                //-------------Student Fee---------------
                StudentFee studentFee = new StudentFee();
                studentFee.StudentId = StudentID;
                studentFee.Year = student.Year.ToUpper();
                studentFee.Course = student.Course.ToUpper();
                studentFee.Class = student.Class.ToUpper();
                studentFee.Session = student.Session.ToUpper();
                studentFee.NewOld = "NEW";
                studentFee.NewStudentFee = student.NewStudentFee;
                studentFee.CMoney = student.CMoney;
                studentFee.TutionFee = student.TutionFee;
                studentFee.OtherFee = student.OtherFee;
                studentFee.TotalFee = student.TotalFee;
                studentFee.TotalFeeCM = student.TotalFeeCM;
                studentFee.Scholership = student.Scholership;
                studentFee.TotalFeeAfterDiscount = student.TotalFeeAfterDiscount;
                studentFee.CMoneyPaidOrNot = "No";
                studentFee.DisBy = student.DisBy;
                studentFee.DisResion = student.DisResion;
                studentFee.CreatedBy = "Admin";
                studentFee.CreatedDateTime = DateTime.Now;

                 studentFee.UpdateDateTime = DateTime.Now;
                 studentFee.UpdateBy = "";
                 _studentFeeRepo.Add(studentFee);


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

        //------------Get Fee Detail---------
        public IActionResult StudentPaymentDetail(string classname, string course, string session, string year)
        {
            StudentPaymentDetailView returnObj = new StudentPaymentDetailView();
            var FeeDetail = _feeRepo.GetFeeByClasssCouseSessionYearNewOld(classname, course, session, year, "New");
            returnObj.feeMasterDetail = FeeDetail;

            return Json(new { success = true, data = returnObj
    });
        }
    }
}
