using ClosedXML.Excel;
using CRM.Data;
using CRM.Models;
using CRM.ModelsForView;
using CRM.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
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
        private readonly AppSettings _mySettings;
       
        private readonly StudentRepository _repoStudent;
        private readonly StudentRegistrationRepository _repoStudentRegi;
        private readonly ILogger<StudentController> _logger;
        private readonly MstCourseRepository _courseRepo;
        private readonly MstClassRepository _classRepo;
        private readonly MstYearRepository _yearRepo;
        private readonly string _baseUrl;
        private readonly MstSessionRepository _sessionRepo;
        private readonly MstSubjectRepository _subjecctRepo;
        private readonly AcademicRepository _repoAcademic;
        private readonly StudentFeeRepository _studentFeeRepo;
        private readonly MstFeeRepository _feeRepo;
        private readonly StudentHistoryRepository _historyStudentRepo;
        public StudentController(ILogger<StudentController> logger,
            
            IOptions<AppSettings> settings,
            IOptions<AppSettings> config,
            MstClassRepository classRepo,
           MstCourseRepository courseRepo,
            MstSubjectRepository subjecctRepo,
            StudentRepository repoStudent,
            MstYearRepository yearRepo,
            MstSessionRepository sessionRepo,
            AcademicRepository repoAcademic,
             StudentFeeRepository studentFeeRepo,
             MstFeeRepository feeRepo,
             StudentRegistrationRepository repoStudentRegi,
             StudentHistoryRepository historyStudentRepo
             )
        {
            
            _mySettings = settings.Value;
            _repoStudent = repoStudent;
            _repoStudentRegi = repoStudentRegi;
            _courseRepo = courseRepo;
            _classRepo = classRepo;
            _logger = logger;
            _baseUrl = config.Value.BaseUrl;
            _subjecctRepo = subjecctRepo;
            _yearRepo = yearRepo;
            _sessionRepo = sessionRepo;
            _repoAcademic = repoAcademic;
            _studentFeeRepo = studentFeeRepo;
            _feeRepo = feeRepo;
            _historyStudentRepo = historyStudentRepo;
        }
        public IActionResult Index(int? id)
        {
            var model = new StudentViewModel();
           
            model.ClassList = _classRepo.GetAll()
                        .Select(x => new SelectListItem {
                            Value = x.Name.ToString(), 
                            Text = x.Name 
                        })
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


            //------Course  -- Subject---
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



            if ((id != 0) && (id != null))
            {
                var varStudentDetail = _repoStudent.GetById(id.Value);

                if (varStudentDetail != null)
                {
                    model.Id = model.Id;
                    model.AdmissionFormNo = varStudentDetail.AdmissionFormNo;
                    model.Year = varStudentDetail.Year;
                    model.EnRollNo = varStudentDetail.EnRollNo;
                    model.AdmissionDate = varStudentDetail.AdmissionDate;
                    model.Class = varStudentDetail.Class;
                    model.RollNo = varStudentDetail.RollNo;
                    model.RegEx = varStudentDetail.RegEx;
                    model.Course = varStudentDetail.Course;

                    if (varStudentDetail.SchoolarNo != null)
                        model.SchoolarNo = varStudentDetail.SchoolarNo.ToString();

                    model.NewOld = varStudentDetail.NewOld;
                    model.SubCode = varStudentDetail.SubCode;
                    model.Medium = varStudentDetail.Medium;
                    model.Gender = varStudentDetail.Gender;
                    model.Caste = varStudentDetail.Caste;
                    model.AadhaarNo = varStudentDetail.AadhaarNo;
                    model.StudentName = varStudentDetail.StudentName;
                    model.DOB = varStudentDetail.DOB;
                    model.SamagraID = varStudentDetail.SamagraID;
                    model.FatherName = varStudentDetail.FatherName;
                    model.MotherName = varStudentDetail.MotherName;
                    model.MobileNoOne = varStudentDetail.MobileNoOne;
                    model.FatherMobileNo = varStudentDetail.FatherMobileNo;
                    model.TC = varStudentDetail.TC;
                    model.PH = varStudentDetail.PH;
                    model.Address = varStudentDetail.Address;
                    model.Minority = varStudentDetail.Minority;

                    //---Dropdown--
                    model.SelectedClass = varStudentDetail.Class; 
                    model.SelectedSubject = model.Subject;
                    model.SelectedCourse = varStudentDetail.Course;
                    model.SelectedYear = varStudentDetail.Year;
                    model.Session = varStudentDetail.Session;



                    ////-------------Fee Detail-------------
                    //var varFeeDetail = _studentFeeRepo.GetStudentFeeDetailB(id.Value, model.Class, model.Course, model.Year, model.Session, model.NewOld);
                    //if (varFeeDetail != null) { 
                    //    model.NewStudentFee = varFeeDetail.NewStudentFee;
                    //    model.CMoney = varFeeDetail.CMoney;
                    //    model.TutionFee = varFeeDetail.TutionFee;
                    //    model.OtherFee = varFeeDetail.OtherFee;
                    //    model.TotalFee = varFeeDetail.TotalFee;
                    //    model.TotalFeeCM = varFeeDetail.TotalFeeCM;
                    //    model.Scholership = varFeeDetail.Scholership;
                    //    model.DisBy = varFeeDetail.DisBy;
                    //    model.DisResion = varFeeDetail.DisResion;
                    //    }



                }
            }



            ViewBag.BaseUrl = _baseUrl;
            //var data = _repoStudent.GetAll();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(StudentViewModel student)
        {
            if (!ModelState.IsValid)
            {
              
                return View(student);
            }

            Student stuObj = new Student();
            stuObj.AdmissionFormNo = student.AdmissionFormNo;
            stuObj.Session = student.Session.ToUpper();
            stuObj.Year = student.Year.ToUpper();
            stuObj.EnRollNo = student.EnRollNo.ToUpper();
            stuObj.AdmissionDate = student.AdmissionDate;
            stuObj.Course = student.Course;
            stuObj.Class = student.Class;
            stuObj.RollNo = student.RollNo.ToUpper();
            stuObj.RegEx = student.RegEx.ToUpper();
            stuObj.SchoolarNo = student.SchoolarNo.ToUpper();
            stuObj.NewOld = student.NewOld.ToUpper();
            stuObj.SubCode = student.SubCode.ToUpper();
            stuObj.Medium = student.Medium.ToUpper();
            stuObj.AadhaarNo = student.AadhaarNo.ToUpper();
            stuObj.SamagraID = student.SamagraID.ToUpper();
            
            stuObj.StudentName = student.StudentName.ToUpper();
            stuObj.FatherName = student.FatherName.ToUpper();
            stuObj.MotherName = student.MotherName.ToUpper();
            stuObj.DOB = student.DOB;
            stuObj.Caste = student.Caste;
            stuObj.Gender = student.Gender;
            stuObj.MobileNoOne = student.MobileNoOne;
            stuObj.Address = student.Address.ToUpper();
            stuObj.Id = student.Id;
            stuObj.Minority = student.Minority;
            stuObj.PH = student.PH;
            stuObj.TC = student.TC;
            stuObj.AbcNo = student.AbcNo;
            stuObj.CreateBy = "Admin";
            stuObj.CreateDatetime = DateTime.Now;

            if (student.Id == 0)
            {
                stuObj.CreateBy = "Admin";
              //  stuObj.CreateDatetime = DateTime.Now;
                var StudentID = _repoStudent.SaveAndGetId(stuObj);

                //-------------Acedemic Detail save-----------

                Academy objAcademy = new Academy();
                objAcademy.RegStudentId = 0;
                objAcademy.StudentId = StudentID;
                objAcademy.SchoolName = student.SchoolName.ToUpper();
                objAcademy.PassingYear = student.PassingYear.ToUpper();
                objAcademy.Board = student.Board.ToUpper();
                objAcademy.MaxMark = student.MaxMark;
                objAcademy.ObtMark = student.ObtMark;
                objAcademy.Result = student.Result.ToUpper();
                objAcademy.Parcent = student.Parcent.ToUpper();
                objAcademy.CreatedBy = "Admin";
                objAcademy.CreatedDate = DateTime.Now;
                objAcademy.UpdatedDate = DateTime.Now;
                objAcademy.UpdatedBy = "Admin";
                var AcedemicId = _repoAcademic.SaveAndGetId(objAcademy);
            }
            else
            {
                //stuObj.DOB = null;
                //stuObj.AdmissionDate = null;
                stuObj.UpdatedBy = "Admin";
                stuObj.UpdateDatetime = DateTime.Now;
                _repoStudent.Update(stuObj);
                
                //-------------Acedemic Detail save-----------
                if (student.IsSaveAcedmicDetail != null)
                {
                    Academy objAcademy = new Academy();
                    objAcademy.RegStudentId = 0;
                    objAcademy.StudentId = stuObj.Id;
                    objAcademy.SchoolName = student.SchoolName.ToUpper();
                    objAcademy.PassingYear = student.PassingYear.ToUpper();
                    objAcademy.Board = student.Board.ToUpper();
                    objAcademy.MaxMark = student.MaxMark;
                    objAcademy.ObtMark = student.ObtMark;
                    objAcademy.Result = student.Result.ToUpper();
                    objAcademy.Parcent = student.Parcent.ToUpper();
                    objAcademy.CreatedBy = "Admin";
                    objAcademy.CreatedDate = DateTime.Now;
                    objAcademy.UpdatedDate = DateTime.Now;
                    objAcademy.UpdatedBy = "Admin";
                    var AcedemicId = _repoAcademic.SaveAndGetId(objAcademy);
                }

            }
           



           




            TempData["msg"] = "Student Added Successfully!";
            return RedirectToAction("List");
        }

        public IActionResult List()
        {
            ViewBag.BaseUrl = _baseUrl;
            var model = new StudentListView();

            model.ClassList = _classRepo.GetAll()
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


            //----Year----
            model.YearList = _yearRepo.GetAll()
                       .Select(x => new SelectListItem
                       {
                           Value = x.Name.ToString(),
                           Text = x.Name
                       })
                       .ToList();

           


            //------Course  -- Subject---
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


            ////--------Get List
            //var data = _repoStudent.GetAll();

            //model.StudentList = data;

            return View(model);
        }

        public IActionResult SearchList(string name, string classes, string year, string course, string session)
        {
           

            //--------Get List
            var data = _repoStudent.FilterList(name, classes, year, course, session);



            return Json(new { success = true, data = data });
        }

        public IActionResult SearchListForRegistration(string session, string classes, string course, string year, string name)
        {


            //--------Get List
            var data = _repoStudent.GetByStudentRegistrationPage(session, classes,
                   course, year, name);


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
        public IActionResult DownloadExcel(string hdnExcelSession, string hdnExcelClass, string hdnExcelCourse, string hdnExcelYear, string hdnExcelName)
        {
            try
            {

               // var students = _repoStudent.GetByStudentRegistrationPage(hdnExcelSession, hdnExcelClass, hdnExcelCourse, hdnExcelYear, hdnExcelName);
                 var students = _repoStudent.GetStudentAllDetailById(hdnExcelSession, hdnExcelClass, hdnExcelCourse, hdnExcelYear, hdnExcelName);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Student List");

                    int row = 1;

                    // ================= HEADER =================
                    worksheet.Cell(row, 1).Value = "Id";
                    worksheet.Cell(row, 2).Value = "AdmissionFormNo";
                    worksheet.Cell(row, 3).Value = "Year";
                    worksheet.Cell(row, 4).Value = "Session";
                    worksheet.Cell(row, 5).Value = "EnRollNo";
                    worksheet.Cell(row, 6).Value = "AdmissionDate";
                    worksheet.Cell(row, 7).Value = "Class";
                    worksheet.Cell(row, 8).Value = "RollNo";
                    worksheet.Cell(row, 9).Value = "RegEx";
                    worksheet.Cell(row, 10).Value = "Course";
                    worksheet.Cell(row, 11).Value = "SchoolarNo";
                    worksheet.Cell(row, 12).Value = "NewOld";
                    worksheet.Cell(row, 13).Value = "SubCode";
                    worksheet.Cell(row, 14).Value = "Medium";
                    worksheet.Cell(row, 15).Value = "Gender";
                    worksheet.Cell(row, 16).Value = "Caste";
                    worksheet.Cell(row, 17).Value = "AadhaarNo";
                    worksheet.Cell(row, 18).Value = "StudentName";
                    worksheet.Cell(row, 19).Value = "DOB";
                    worksheet.Cell(row, 20).Value = "SamagraID";
                    worksheet.Cell(row, 21).Value = "FatherName";
                    worksheet.Cell(row, 22).Value = "MobileNoOne";
                    worksheet.Cell(row, 23).Value = "MobileNoTwo";
                    worksheet.Cell(row, 24).Value = "MotherName";
                    worksheet.Cell(row, 25).Value = "TC";
                    worksheet.Cell(row, 26).Value = "PH";
                    worksheet.Cell(row, 27).Value = "FatherMobileNo";
                    worksheet.Cell(row, 28).Value = "Address";
                    worksheet.Cell(row, 29).Value = "Minority";
                    worksheet.Cell(row, 30).Value = "CreateBy";
                    worksheet.Cell(row, 31).Value = "CreateDatetime";
                    worksheet.Cell(row, 32).Value = "ExamFormSubmited";
                    worksheet.Cell(row, 33).Value = "AbcNo";

                    worksheet.Cell(row, 34).Value = "CMoney";
                    worksheet.Cell(row, 35).Value = "TutionFee";
                    worksheet.Cell(row, 36).Value = "OtherFee";
                    worksheet.Cell(row, 37).Value = "TotalFee";
                    worksheet.Cell(row, 38).Value = "TotalFeeCM";

                    // ================= DATA =================
                    worksheet.Row(1).Style.Font.Bold = true;
                    row++;
                   

               
                foreach (var item in students)
                {
                        worksheet.Cell(row, 1).Value = item.Id;
                        worksheet.Cell(row, 2).Value = item.AdmissionFormNo;
                        worksheet.Cell(row, 3).Value = item.Year;
                        worksheet.Cell(row, 4).Value = item.Session;
                        worksheet.Cell(row, 5).Value = item.EnRollNo;
                        worksheet.Cell(row, 6).Value = item.AdmissionDate;
                        worksheet.Cell(row, 7).Value = item.Class;
                        worksheet.Cell(row, 8).Value = item.RollNo;
                        worksheet.Cell(row, 9).Value = item.RegEx;
                        worksheet.Cell(row, 10).Value = item.Course;
                        worksheet.Cell(row, 11).Value = item.SchoolarNo;
                        worksheet.Cell(row, 12).Value = item.NewOld;
                        worksheet.Cell(row, 13).Value = item.SubCode;
                        worksheet.Cell(row, 14).Value = item.Medium;
                        worksheet.Cell(row, 15).Value = item.Gender;
                        worksheet.Cell(row, 16).Value = item.Caste;
                        worksheet.Cell(row, 17).Value = item.AadhaarNo;
                        worksheet.Cell(row, 18).Value = item.StudentName;
                        worksheet.Cell(row, 19).Value = item.DOB;
                        worksheet.Cell(row, 20).Value = item.SamagraID;
                        worksheet.Cell(row, 21).Value = item.FatherName;
                        worksheet.Cell(row, 22).Value = item.MobileNoOne;
                        worksheet.Cell(row, 23).Value = item.MobileNoTwo;
                        worksheet.Cell(row, 24).Value = item.MotherName;
                        worksheet.Cell(row, 25).Value = item.TC;
                        worksheet.Cell(row, 26).Value = item.PH;
                        worksheet.Cell(row, 27).Value = item.FatherMobileNo;
                        worksheet.Cell(row, 28).Value = item.Address;
                        worksheet.Cell(row, 29).Value = item.Minority;
                        worksheet.Cell(row, 30).Value = item.CreateBy;
                        worksheet.Cell(row, 31).Value = item.CreateDatetime;
                        worksheet.Cell(row, 32).Value = item.ExamFormSubmited;
                        worksheet.Cell(row, 33).Value = item.AbcNo;

                        worksheet.Cell(row, 34).Value = item.CMoney;
                        worksheet.Cell(row, 35).Value = item.TutionFee;
                        worksheet.Cell(row, 36).Value = item.OtherFee;
                        worksheet.Cell(row, 37).Value = item.TotalFee;
                        worksheet.Cell(row, 38).Value = item.TotalFeeCM;

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

        [HttpPost]
        public IActionResult DownloadOldStudentExcel(string hdnExcelSession, string hdnExcelClass, string hdnExcelCourse, string hdnExcelYear, string hdnExcelName)
        {
            try
            {

                var students = _repoStudent.GetByStudentHistoryPage(hdnExcelSession, hdnExcelClass, hdnExcelCourse, hdnExcelYear, hdnExcelName);
                // var students = _repoStudentRegi.FilterList(name, classes, subject, course, regPvt);

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Student List");

                    // Header
                    worksheet.Cell(1, 1).Value = "Id";
                    worksheet.Cell(1, 2).Value = "AdmissionFormNo";
                    worksheet.Cell(1, 3).Value = "Year";
                    worksheet.Cell(1, 4).Value = "EnRollNo";
                    worksheet.Cell(1, 5).Value = "AdmissionDate";
                    worksheet.Cell(1, 6).Value = "Class";
                    worksheet.Cell(1, 7).Value = "RollNo";
                    worksheet.Cell(1, 8).Value = "RegEx";
                    worksheet.Cell(1, 9).Value = "Course";
                    worksheet.Cell(1, 10).Value = "SchoolarNo";
                    worksheet.Cell(1, 11).Value = "NewOld";
                    worksheet.Cell(1, 12).Value = "SubCode";
                    worksheet.Cell(1, 13).Value = "Medium";
                    worksheet.Cell(1, 14).Value = "Gender";
                    worksheet.Cell(1, 15).Value = "Caste";
                    worksheet.Cell(1, 16).Value = "AadhaarNo";
                    worksheet.Cell(1, 17).Value = "StudentName";
                    worksheet.Cell(1, 18).Value = "DOB";
                    worksheet.Cell(1, 19).Value = "SamagraID";
                    worksheet.Cell(1, 20).Value = "FatherName";
                    worksheet.Cell(1, 21).Value = "MobileNoOne";
                    worksheet.Cell(1, 22).Value = "MobileNoTwo";
                    worksheet.Cell(1, 23).Value = "MotherName";
                    worksheet.Cell(1, 24).Value = "TC";
                    worksheet.Cell(1, 25).Value = "PH";
                    worksheet.Cell(1, 26).Value = "FatherMobileNo";
                    worksheet.Cell(1, 27).Value = "Address";
                    worksheet.Cell(1, 28).Value = "Minority";
                    worksheet.Cell(1, 29).Value = "Session";
                    worksheet.Cell(1, 30).Value = "CreateBy";
                    worksheet.Cell(1, 31).Value = "CreateDatetime";
                    worksheet.Cell(1, 32).Value = "AbcNo";
                    worksheet.Cell(1, 33).Value = "ExamFormSubmited";
                    worksheet.Row(1).Style.Font.Bold = true;

                    int row = 2;
                    foreach (var item in students)
                    {
                        worksheet.Cell(row, 1).Value = item.StudentId;
                        worksheet.Cell(row, 2).Value = item.AdmissionForm;
                        worksheet.Cell(row, 3).Value = item.Year;
                        worksheet.Cell(row, 4).Value = item.EnrolNo;
                        worksheet.Cell(row, 5).Value = item.AdmissionDate;
                        worksheet.Cell(row, 6).Value = item.Classs;
                        worksheet.Cell(row, 7).Value = item.RollNo;
                        worksheet.Cell(row, 8).Value = item.RegPvt;
                        worksheet.Cell(row, 9).Value = item.Course;
                        worksheet.Cell(row, 10).Value = item.ScholerNo;
                        worksheet.Cell(row, 11).Value = item.NewOld;
                        worksheet.Cell(row, 12).Value = "SubCode"; //-------
                        worksheet.Cell(row, 13).Value = item.Medium;
                        worksheet.Cell(row, 14).Value = item.Gender;
                        worksheet.Cell(row, 15).Value = item.Cast;
                        worksheet.Cell(row, 16).Value = item.AdharNo;
                        worksheet.Cell(row, 17).Value = item.StudentName;
                        worksheet.Cell(row, 18).Value = item.DOB;
                        worksheet.Cell(row, 19).Value = item.SamagraId;
                        worksheet.Cell(row, 20).Value = item.FatherName;
                        worksheet.Cell(row, 21).Value = item.MobileNo;
                        worksheet.Cell(row, 22).Value = item.MobileNo;
                        worksheet.Cell(row, 23).Value = item.MotherName;
                        worksheet.Cell(row, 24).Value =  "TC"; //---
                        worksheet.Cell(row, 25).Value = item.PH;
                        worksheet.Cell(row, 26).Value = "FatherMobile";
                        worksheet.Cell(row, 27).Value = item.Address;
                        worksheet.Cell(row, 28).Value = "Minority"; //------
                        worksheet.Cell(row, 29).Value = item.Session;
                        worksheet.Cell(row, 30).Value = item.CreateBy;
                        worksheet.Cell(row, 31).Value = item.CreateDate.ToString("dd/MM/yyyy");
                        worksheet.Cell(row, 32).Value = item.AbcId;
                        worksheet.Cell(row, 33).Value = item.ExamFormSubmited;

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


        //-------------------Promossions------
        public IActionResult StudentDetail(int id)
        {
            var data = _repoStudent.GetById(id);
            if(data.AdmissionDate != null)
            data.FatherName = data.AdmissionDate.Value.ToString("dd/MM/yyyy");

            return Json(new { success = true, data = data });
        }
        //----------------Save---Promossions------
        public IActionResult SavePromossionDetail(int id, string newAdmissionDate, int newAdmisionFormNo, string newClass, string newYear, string newCourse, string newSession, string newMbileNo,
           string passedAdmissionDate, string passedAdmisionFormNo, string passedClass, string passedYear, string passedCourse, string passedSession,
           string passedCollege, string passedBoard, string passedMaxMark, string passedObtainMark, string passedResult, string passedParcentage )
        {
            Student stuObj = new Student();
            stuObj.Id = id;
            if(!string.IsNullOrEmpty(newAdmissionDate))
            stuObj.AdmissionDate = Convert.ToDateTime(DateTime.Now);

            stuObj.AdmissionFormNo = newAdmisionFormNo;
            stuObj.Session = newSession.ToUpper();
            stuObj.Year = newYear;
            stuObj.Course = newCourse;
            stuObj.Class = newClass;
            stuObj.MobileNoOne = newMbileNo;

           var teee =  _repoStudent.PromotStudent(stuObj);

            //-------------Acedemic Detail save-----------
            Academy objAcademy = new Academy();
            objAcademy.RegStudentId = 0;
            objAcademy.StudentId = id;
            objAcademy.SchoolName = passedCollege.ToUpper();
            objAcademy.PassingYear = passedYear.ToUpper();
            objAcademy.Board = passedBoard.ToUpper();
            objAcademy.MaxMark = passedMaxMark;
            objAcademy.ObtMark = passedObtainMark;
            objAcademy.Result = passedResult.ToUpper();
            objAcademy.Parcent = passedParcentage.ToUpper();
            objAcademy.Class = passedClass;
            objAcademy.Course = passedCourse;
            objAcademy.Session = passedSession;
            objAcademy.AdmissionForm = passedAdmisionFormNo;
            objAcademy.AdmissionDate = passedAdmissionDate;
            objAcademy.CreatedBy = "Admin";
            objAcademy.CreatedDate = DateTime.Now;
            objAcademy.UpdatedDate = DateTime.Now;
            objAcademy.UpdatedBy = "Admin";
            var AcedemicId = _repoAcademic.SaveAndGetId(objAcademy);

            
            return Json(new { success = true, data = true });
        }


       
        //------------------------
        public IActionResult PromodeStudent(int? id)
        {
            var model = new StudentViewModel();

            model.ClassList = _classRepo.GetAll()
                        .Select(x => new SelectListItem
                        {
                            Value = x.Name.ToString(),
                            Text = x.Name
                        })
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


            //------Course  -- Subject---
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



            if ((id != 0) && (id != null))
            {
                var varStudentDetail = _repoStudent.GetById(id.Value);

                if (varStudentDetail != null)
                {
                    model.Id = varStudentDetail.Id;
                    model.AdmissionFormNo = varStudentDetail.AdmissionFormNo;
                    model.Year = varStudentDetail.Year;
                    model.EnRollNo = varStudentDetail.EnRollNo;
                    model.AdmissionDate = varStudentDetail.AdmissionDate;
                    model.Class = varStudentDetail.Class;
                    model.RollNo = varStudentDetail.RollNo;

                    if(!string.IsNullOrEmpty(varStudentDetail.RegEx))
                    model.RegEx = varStudentDetail.RegEx.ToUpper();

                    model.Course = varStudentDetail.Course;

                    if (varStudentDetail.SchoolarNo != null)
                        model.SchoolarNo = varStudentDetail.SchoolarNo.ToString();

                    if (!string.IsNullOrEmpty(varStudentDetail.NewOld))
                        model.NewOld = varStudentDetail.NewOld.ToUpper();

                    if (!string.IsNullOrEmpty(varStudentDetail.SubCode))
                        model.SubCode = varStudentDetail.SubCode.ToUpper();

                    model.Medium = varStudentDetail.Medium;
                    model.Gender = varStudentDetail.Gender;
                    model.Caste = varStudentDetail.Caste;

                    if (!string.IsNullOrEmpty(varStudentDetail.StudentName))
                        model.StudentName = varStudentDetail.StudentName.ToUpper();

                    model.DOB = varStudentDetail.DOB;

                    if (!string.IsNullOrEmpty(varStudentDetail.AadhaarNo))
                        model.AadhaarNo = varStudentDetail.AadhaarNo.ToUpper();

                    if (!string.IsNullOrEmpty(varStudentDetail.AbcNo))
                        model.AbcNo = varStudentDetail.AbcNo.ToUpper();

                    if (!string.IsNullOrEmpty(varStudentDetail.SamagraID))
                        model.SamagraID = varStudentDetail.SamagraID.ToUpper();

                    if (!string.IsNullOrEmpty(varStudentDetail.FatherName))
                        model.FatherName = varStudentDetail.FatherName;

                    if (!string.IsNullOrEmpty(varStudentDetail.MotherName))
                        model.MotherName = varStudentDetail.MotherName.ToUpper();

                    if (!string.IsNullOrEmpty(varStudentDetail.MobileNoOne))
                        model.MobileNoOne = varStudentDetail.MobileNoOne.ToUpper();

                    if (!string.IsNullOrEmpty(varStudentDetail.FatherMobileNo))
                        model.FatherMobileNo = varStudentDetail.FatherMobileNo.ToUpper();

                    model.TC = varStudentDetail.TC;
                    model.PH = varStudentDetail.PH;

                    if (!string.IsNullOrEmpty(varStudentDetail.Address))
                        model.Address = varStudentDetail.Address.ToUpper();

                    model.Minority = varStudentDetail.Minority;
                    model.ExamFormSubmited = varStudentDetail.ExamFormSubmited;

                    //---Dropdown--
                    model.SelectedClass = varStudentDetail.Class;
                    model.SelectedSubject = model.Subject;
                    model.SelectedCourse = varStudentDetail.Course;
                    model.SelectedYear = varStudentDetail.Year;
                    model.Session = varStudentDetail.Session;


                }
            }
            //-----------Acadmic Detail-----------------
            var varAcadmicList = _repoAcademic.GetListByStudentId(id.Value);
            if (varAcadmicList != null) 
            {
                model.AcadmicList = varAcadmicList;
            }


            //-------------Document List-------------------
            var PhotoBaseUrl = _mySettings.DocumentUrl;

            var StudentPhoto = PhotoBaseUrl + "\\Photo\\" + model.Photo + ".jpg"; 
            if (System.IO.File.Exists(StudentPhoto))
            {
                model.Photo = "/StudentData/Photo/" + model.AdmissionFormNo + ".jpg"; 
            }

            var TCPhoto = PhotoBaseUrl + "\\TC\\" + model.Photo + ".jpg";
            if (System.IO.File.Exists(TCPhoto))
            {
                model.TCPhoto = "/StudentData/TC/" + model.AdmissionFormNo + ".jpg";
            }


            //-----------------Student History-----------------

            model.StudentHistoryList = _historyStudentRepo.GetListByStuId(model.Id);


            //string[] files = Directory.GetFiles(StudentPhoto);


            //Dictionary<string, string> temList = new Dictionary<string, string>();
            //foreach (string file in files)
            //{
            //    var varfilename = Path.GetFileName(file);

            //    temList.Add(varfilename, file);

            //}

            //model.DocumentList = temList;

            ViewBag.BaseUrl = _baseUrl;
            //var data = _repoStudent.GetAll();
            return View(model);
        }


        public bool UpdateStudentHistory(int StudentId)
        {
            try 
            {

                var SavedStudentTable = _repoStudent.GetById(StudentId);
                //------------Student History----
                StudentHistory historyObj = new StudentHistory();

                historyObj.StudentId = StudentId;
                historyObj.AdmissionForm = SavedStudentTable.AdmissionFormNo.Value;
                if (SavedStudentTable.AdmissionDate != null)
                    historyObj.AdmissionDate = Convert.ToDateTime(SavedStudentTable.AdmissionDate);

                historyObj.Session = SavedStudentTable.Session;
                historyObj.Classs = SavedStudentTable.Class;
                historyObj.Course = SavedStudentTable.Course;
                historyObj.Year = SavedStudentTable.Year;

                historyObj.ScholerNo = SavedStudentTable.SchoolarNo;
                historyObj.RegPvt = SavedStudentTable.RegEx;
                historyObj.NewOld = SavedStudentTable.NewOld;
                historyObj.Medium = SavedStudentTable.Medium;
                historyObj.EnrolNo = SavedStudentTable.EnRollNo; //--------
                historyObj.RollNo = SavedStudentTable.RollNo; //---
                historyObj.Gender = SavedStudentTable.Gender;
                // historyObj.Status = SavedStudentTable.Status; //-----
                historyObj.StudentName = SavedStudentTable.StudentName;
                historyObj.FatherName = SavedStudentTable.FatherName;
                historyObj.MotherName = SavedStudentTable.MotherName;
                historyObj.PH = SavedStudentTable.PH; //--
                historyObj.Cast = SavedStudentTable.Caste;

                if (SavedStudentTable.DOB.HasValue)
                    historyObj.DOB = SavedStudentTable.DOB.Value;

                historyObj.Medium = SavedStudentTable.Medium;
                historyObj.EnrolNo = SavedStudentTable.EnRollNo;
                historyObj.RollNo = SavedStudentTable.RollNo;
                historyObj.Minority = SavedStudentTable.Minority;
                historyObj.Address = SavedStudentTable.Address; //----
                historyObj.MobileNo = SavedStudentTable.MobileNoOne;
                historyObj.TCIssue = "No";//---
                historyObj.SamagraId = SavedStudentTable.SamagraID; //---
                historyObj.AdharNo = SavedStudentTable.AadhaarNo; //----
                historyObj.AbcId = SavedStudentTable.AbcNo; //----
                historyObj.ExamFormSubmited = SavedStudentTable.ExamFormSubmited;

                historyObj.CreateBy = "Admin";
                historyObj.CreateDate = DateTime.Now;
                historyObj.UpdateBy = "Admin";
                historyObj.UpdateDate = DateTime.Now;
                _historyStudentRepo.UpdateHistoryDetail(historyObj);
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }


        //--------------------- Save Personal Detail--------------
        public IActionResult SavePromossionDetail11(int id, string varStudentName, string varFatherName, string varMotherName, string varMobileNo,
            string varDOB,string varFatherMobileNo, string varGender, string varMinority, string varCaset, string varAbcNo,
            string varSamagraID, string varAddress, string varTC, string varPH, string varAdhaarNo)
        {
            Student stuObj = new Student();
            stuObj.Id = id;

            if(!string.IsNullOrEmpty(varStudentName))
            stuObj.StudentName = varStudentName.ToUpper();

            if (!string.IsNullOrEmpty(varFatherName))
                stuObj.FatherName = varFatherName.ToUpper();

            if (!string.IsNullOrEmpty(varMotherName))
                stuObj.MotherName = varMotherName.ToUpper();

            if (!string.IsNullOrEmpty(varMobileNo))
                stuObj.MobileNoOne = varMobileNo.ToUpper();

            if (!string.IsNullOrEmpty(varDOB))
                stuObj.DOB = Convert.ToDateTime(varDOB);

            stuObj.FatherMobileNo = varFatherMobileNo;
            stuObj.Gender = varGender;
            stuObj.Minority = varMinority;
            stuObj.Caste = varCaset;

            if (!string.IsNullOrEmpty(varAbcNo))
                stuObj.AbcNo = varAbcNo.ToUpper();

            if (!string.IsNullOrEmpty(varAdhaarNo))
                stuObj.AadhaarNo = varAdhaarNo.ToUpper();

            if (!string.IsNullOrEmpty(varSamagraID))
                stuObj.SamagraID = varSamagraID.ToUpper();

            if (!string.IsNullOrEmpty(varAddress))
                stuObj.Address = varAddress.ToUpper();

            stuObj.TC = varTC;
            stuObj.PH = varPH;
            stuObj.UpdateDatetime = DateTime.Now;
            stuObj.UpdatedBy = "Update Admin";

            var teee = _repoStudent.UpdatePersonalDetail(stuObj);

            //-------------Update student History-----
            
            var check = UpdateStudentHistory(stuObj.Id);


            return Json(new { success = true, data = true });
        }


        //--------------------- Save Update Detail--------------
        public IActionResult UpdateCollegeDetail(int id, string varSession, string varNewOld, string varMedium,
            string varClass, string varCourse, string varYear, string varAdmissionFormNo, string varEnRollNo,
             string varAdmissionDate, string varRollNo, string varSchoolarNo, string varSubCode, string varRegEx)

        {
            Student stuObj = new Student();
            stuObj.Id = id;

            if (!string.IsNullOrEmpty(varAdmissionFormNo))
                stuObj.AdmissionFormNo = Convert.ToInt32(varAdmissionFormNo);


            if (!string.IsNullOrEmpty(varAdmissionDate))
                stuObj.AdmissionDate = Convert.ToDateTime(varAdmissionDate);

            
            stuObj.NewOld = varNewOld;
            stuObj.Medium = varMedium;
            //stuObj.Session = varSession;   //-------------varSession
            //stuObj.Class = varClass;  //-------------Class
            //stuObj.Course = varCourse;  //-------Course
            //stuObj.Year = varYear;  //----------Year
            if (!string.IsNullOrEmpty(varEnRollNo))
                stuObj.EnRollNo = varEnRollNo.ToUpper();

            if (!string.IsNullOrEmpty(varRollNo))
                stuObj.RollNo = varRollNo.ToUpper();
            

            if (!string.IsNullOrEmpty(varSchoolarNo))
                stuObj.SchoolarNo = varSchoolarNo.ToUpper();

            if (!string.IsNullOrEmpty(varSubCode))
                stuObj.SubCode = varSubCode.ToUpper();

            stuObj.RegEx = varRegEx;
            stuObj.UpdateDatetime = DateTime.Now;
            stuObj.UpdatedBy = "Update Admin";

            var teee = _repoStudent.UpdateCollegeDetail(stuObj);

            //-------------Update student History-----
            var check = UpdateStudentHistory(stuObj.Id);


            return Json(new { success = true, data = true });
        }


        //--------------------- Save Acadmic Detail--------------
        public IActionResult UpdateAcadmicDetail(int id, string varAcademicYear, string varAcadmicSession,
            string varAcadmicClass, string varAcadmicCourse, string varSchoolName, string varBoard, string varMaxMark,
            string varObtMark, string varResult, string varParcent, string varAdmissionFormNo)

        {
         

            Academy objAcademy = new Academy();
            objAcademy.RegStudentId = 0;
            objAcademy.StudentId = id;

            if (!string.IsNullOrEmpty(varSchoolName))
                objAcademy.SchoolName = varSchoolName.ToUpper();

            if (!string.IsNullOrEmpty(varAcademicYear))
                objAcademy.PassingYear = varAcademicYear.ToUpper();


            objAcademy.Session = varAcadmicSession;

            if (!string.IsNullOrEmpty(varAcadmicClass))
                objAcademy.Class = varAcadmicClass.ToUpper();

            if (!string.IsNullOrEmpty(varAcadmicClass))
                objAcademy.Course = varAcadmicCourse.ToUpper();

            if (!string.IsNullOrEmpty(varBoard))
                objAcademy.Board = varBoard.ToUpper();

           
            objAcademy.MaxMark = varMaxMark;
            objAcademy.ObtMark = varObtMark;

            if (!string.IsNullOrEmpty(varResult))
                objAcademy.Result = varResult.ToUpper();

            if (!string.IsNullOrEmpty(varParcent))
                objAcademy.Parcent = varParcent.ToUpper();

            objAcademy.AdmissionForm = varAdmissionFormNo;
            objAcademy.CreatedBy = "Admin";
            objAcademy.CreatedDate = DateTime.Now;
            objAcademy.UpdatedDate = DateTime.Now;
            objAcademy.UpdatedBy = "Admin";
            var AcedemicId = _repoAcademic.SaveAndGetId(objAcademy);



            return Json(new { success = true, data = true });
        }


        //--------------------- Promoted Detail--------------
        public IActionResult PromotStudent(int id, string varPromotSession, string varPromotClass,
            string varPromotCourse, string varPromoYear, string varCurrentYear, string varCurrentSession,
            int varPromotFormNo, string varPromotDate)

        {
            //-------Fee Master-------------
            var FeeMasterDetail = _feeRepo.GetFeeByClasssCouseSessionYearNewOld(varPromotClass, varPromotCourse, varPromotSession, varPromoYear, "New");
            if (FeeMasterDetail == null)
            {
                return Json(new { success = true, data = true });
            }
            //-------Old Fee Detail--------------
            var OldFeeDetail = _studentFeeRepo.GetFeeByClasssCouseSessionYearNewOld(id, varPromotClass, varPromotCourse, varCurrentSession, varCurrentYear, "New");

            var SavedStudentTable = _repoStudent.GetById(id);
            //------------ Add Student History----
            StudentHistory historyObj = new StudentHistory();

            historyObj.StudentId = SavedStudentTable.Id;
            historyObj.AdmissionForm = varPromotFormNo;
            if (!string.IsNullOrEmpty(varPromotDate))
                historyObj.AdmissionDate = Convert.ToDateTime(varPromotDate);

            historyObj.Session = varPromotSession;
            historyObj.Classs = varPromotClass;
            historyObj.Course = varPromotCourse;
            historyObj.Year = varPromoYear.ToUpper();

            historyObj.ScholerNo = SavedStudentTable.SchoolarNo;
            historyObj.RegPvt = SavedStudentTable.RegEx;
            historyObj.NewOld = SavedStudentTable.NewOld;
            historyObj.Medium = SavedStudentTable.Medium;
            historyObj.EnrolNo = SavedStudentTable.EnRollNo; //--------
            historyObj.RollNo = SavedStudentTable.RollNo; //---
            historyObj.Gender = SavedStudentTable.Gender;
            // historyObj.Status = SavedStudentTable.Status; //-----
            historyObj.StudentName = SavedStudentTable.StudentName;
            historyObj.FatherName = SavedStudentTable.FatherName;
            historyObj.MotherName = SavedStudentTable.MotherName;
            historyObj.PH = SavedStudentTable.PH; //--
            historyObj.Cast = SavedStudentTable.Caste;
            historyObj.Minority = SavedStudentTable.Minority;

            if (SavedStudentTable.DOB.HasValue)
                historyObj.DOB = SavedStudentTable.DOB.Value;

            historyObj.Address = SavedStudentTable.Address; //----
            historyObj.MobileNo = SavedStudentTable.MobileNoOne;
            // historyObj.TCIssue = SavedStudentTable.TCissue;//---
            historyObj.SamagraId = SavedStudentTable.SamagraID; //---
            historyObj.AdharNo = SavedStudentTable.AadhaarNo; //----
            historyObj.AbcId = SavedStudentTable.AbcNo; //----
            historyObj.ExamFormSubmited = SavedStudentTable.ExamFormSubmited;

            historyObj.CreateBy = "Admin";
            historyObj.CreateDate = DateTime.Now;
            historyObj.UpdateBy = "Admin";
            historyObj.UpdateDate = DateTime.Now;
            _historyStudentRepo.Add(historyObj);



            //-------------Student Fee---------------
            StudentFee studentFee = new StudentFee();
            studentFee.StudentId = id;
            studentFee.Year = FeeMasterDetail.Year.ToUpper();
            studentFee.Course = varPromotCourse;
            studentFee.Class = varPromotClass;
            studentFee.Session = varPromotSession;
            studentFee.NewOld = "Promote";
            studentFee.NewStudentFee = 0;
            studentFee.CMoney = FeeMasterDetail.CMoney;
            studentFee.TutionFee = FeeMasterDetail.TutionFee;
            studentFee.OtherFee = FeeMasterDetail.OtherFee;
            studentFee.TotalFee =  0+ FeeMasterDetail.TutionFee + FeeMasterDetail.OtherFee;
            //studentFee.TotalFeeCM = FeeDetail.TotalFeeCM; //---- Not using
            studentFee.Scholership = OldFeeDetail.Scholership;
            studentFee.TotalFeeAfterDiscount = studentFee.TotalFee - studentFee.Scholership;
            studentFee.CMoneyPaidOrNot = "NO";
            studentFee.DisBy = OldFeeDetail.DisBy;
            studentFee.DisResion = OldFeeDetail.DisResion;
            studentFee.CreatedBy = "Admin";
            studentFee.CreatedDateTime = DateTime.Now;

            studentFee.UpdateDateTime = DateTime.Now;
            studentFee.UpdateBy = "";
            _studentFeeRepo.Add(studentFee);


            //---------Update Student Detail----------------
            studentFee.Year = FeeMasterDetail.Year;
            studentFee.Course = varPromotCourse;
            studentFee.Class = varPromotClass;
            studentFee.Session = varPromotSession;

            Student stuObj = new Student();
            stuObj.Id = id;
            stuObj.Year = FeeMasterDetail.Year;
            stuObj.Course = varPromotCourse;
            stuObj.Class = varPromotClass;
            stuObj.Session = varPromotSession;
            stuObj.ExamFormSubmited = "No";
            stuObj.UpdateDatetime = DateTime.Now;
            stuObj.UpdatedBy = "UPdate Admin";

            var teee = _repoStudent.PromoteStudentDetail(stuObj);


            return Json(new { success = true, data = true });
        }


        //----------Fill Exam Form--------------

        public IActionResult SaveExamFormFilled(string ids)
        {
           

            var teee = _repoStudent.ExamFormFilled(ids);
            //----------Update into History table--------
            var idList = ids.Split(',')
                  .Select(int.Parse)
                  .ToList();
            if (idList.Count() > 0) 
            {
                foreach (int studentId in idList) 
                {
                    UpdateStudentHistory(studentId);
                }
                
            }

            return Json(new { success = true, data = true });
        }


        //--------------Student History-------------
        public IActionResult OldStudentList()
        {
            ViewBag.BaseUrl = _baseUrl;
            var model = new StudentListView();

            model.ClassList = _classRepo.GetAll()
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


            //----Year----
            model.YearList = _yearRepo.GetAll()
                       .Select(x => new SelectListItem
                       {
                           Value = x.Name.ToString(),
                           Text = x.Name
                       })
                       .ToList();




            //------Course  -- Subject---
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


            ////--------Get List
            //var data = _repoStudent.GetAll();

            //model.StudentList = data;

            return View(model);
        }

        public IActionResult SearchOldStudentList(string name, string classes, string year, string course, string session)
        {
            //--------Get List
            var data = _repoStudent.GetByStudentHistoryPage(session, classes, course, year, name);

            return Json(new { success = true, data = data });
        }

        //--------------Delete Acadmic Detail-------------------
        
        public IActionResult Delete(int id)
        {
            _repoAcademic.Delete(id);
            return Json(new { success = true, data = "Success" });
        }

    }
}
