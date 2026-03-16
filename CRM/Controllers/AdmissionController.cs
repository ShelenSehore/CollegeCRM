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
        private readonly StudentRegitrationFeeRepository _studentRegiFee;

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
                StudentRegitrationFeeRepository studentRegiFee,
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
            _studentRegiFee = studentRegiFee;
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
            //obj.Sem = student.Sem.ToUpper();
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

                //-------------Student Fee---------------
                StudentRegitrationFee studentFee = new StudentRegitrationFee();
                studentFee.StudentId = RegiSaveId;
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
                _studentRegiFee.Add(studentFee);

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


        //----------------Move Registration to Student Table AdmissionDone-----
        public IActionResult AdmissionDone(string studentList)
        {
            if (!string.IsNullOrEmpty(studentList)) {
                string[] studentArray = studentList.Split(',');
                if (studentArray.Count() > 0) 
                {
                    foreach (var RegistudentId in studentArray) 
                    {
                       var student = _repoStudentRegi.GetById(Convert.ToInt32(RegistudentId));
                        if (student != null) 
                        {
                            //-------------Save Into Student-----table--------------
                            Student stuObj = new Student();
                            stuObj.AdmissionFormNo = student.FormNo;

                            if(student.Session !=null)
                            stuObj.Session = student.Session.ToUpper();

                            if (student.Year != null)
                                stuObj.Year = student.Year.ToUpper();

                            if (student.Course != null)
                                stuObj.Course = student.Course.ToUpper();

                            if (student.Subject != null)
                                stuObj.Class = student.Subject.ToUpper();

                            if (student.Name != null)
                                stuObj.StudentName = student.Name.ToUpper();

                            if (student.FatherName != null)
                                stuObj.FatherName = student.FatherName.ToUpper();

                            if (student.MotherName != null)
                                stuObj.MotherName = student.MotherName.ToUpper();

                            stuObj.DOB = student.DOB;
                            stuObj.Caste = student.Caste;
                            stuObj.Gender = student.Gender;
                            stuObj.MobileNoOne = student.MobileNo;
                            stuObj.CreateBy = "Admin";
                            stuObj.CreateDatetime = DateTime.Now;

                            var StudentTableID = _repoStudent.SaveAndGetId(stuObj);

                            //---------Get Fee detail from FeeReigtration table----------
                            var varFeeDetail = _studentRegiFee.GetById(Convert.ToInt32(RegistudentId));
                            if (RegistudentId != null) 
                            {
                                StudentFee studentFee = new StudentFee();
                                studentFee.StudentId = StudentTableID;
                                studentFee.Year = stuObj.Year.ToUpper();
                                studentFee.Course = stuObj.Course.ToUpper();
                                studentFee.Class = stuObj.Class.ToUpper();
                                studentFee.Session = stuObj.Session.ToUpper();
                                studentFee.NewOld = "NEW";
                                studentFee.NewStudentFee = varFeeDetail.NewStudentFee;
                                studentFee.CMoney = varFeeDetail.CMoney;
                                studentFee.TutionFee = varFeeDetail.TutionFee;
                                studentFee.OtherFee = varFeeDetail.OtherFee;
                                studentFee.TotalFee = varFeeDetail.TotalFee;
                                studentFee.TotalFeeCM = varFeeDetail.TotalFeeCM;
                                studentFee.Scholership = varFeeDetail.Scholership;
                                studentFee.TotalFeeAfterDiscount = varFeeDetail.TotalFeeAfterDiscount;
                                studentFee.CMoneyPaidOrNot = "No";
                                studentFee.DisBy = varFeeDetail.DisBy;
                                studentFee.DisResion = varFeeDetail.DisResion;
                                studentFee.CreatedBy = "Admin";
                                studentFee.CreatedDateTime = DateTime.Now;
                                studentFee.UpdateDateTime = DateTime.Now;
                                studentFee.UpdateBy = "";
                                _studentFeeRepo.Add(studentFee);

                            }


                            //----------Update Registration Table IsMove = true-------
                            var statusUpdate = _repoStudentRegi.IsMoved(true, Convert.ToInt32(RegistudentId));  

                        }
                    }
                }
            }


           


            return Json(new
            {
                success = true,
                data = ""
            });
        }


    }
}
