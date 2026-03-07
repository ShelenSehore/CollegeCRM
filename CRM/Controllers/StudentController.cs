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
             StudentRegistrationRepository repoStudentRegi)
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
            stuObj.Session = student.Session;
            stuObj.Year = student.Year;
            stuObj.EnRollNo = student.EnRollNo;
            stuObj.AdmissionDate = student.AdmissionDate;
            stuObj.Course = student.Course;
            stuObj.Class = student.Class;
            stuObj.RollNo = student.RollNo;
            stuObj.RegEx = student.RegEx;
            stuObj.SchoolarNo = student.SchoolarNo;
            stuObj.NewOld = student.NewOld;
            stuObj.SubCode = student.SubCode;
            stuObj.Medium = student.Medium;
            stuObj.AadhaarNo = student.AadhaarNo;
            stuObj.SamagraID = student.SamagraID;
            
            stuObj.StudentName = student.StudentName;
            stuObj.FatherName = student.FatherName;
            stuObj.MotherName = student.MotherName;
            stuObj.DOB = student.DOB;
            stuObj.Caste = student.Caste;
            stuObj.Gender = student.Gender;
            stuObj.MobileNoOne = student.MobileNoOne;
            stuObj.Address = student.Address;
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
            stuObj.Session = newSession;
            stuObj.Year = newYear;
            stuObj.Course = newCourse;
            stuObj.Class = newClass;
            stuObj.MobileNoOne = newMbileNo;

           var teee =  _repoStudent.PromotStudent(stuObj);

            //-------------Acedemic Detail save-----------
            Academy objAcademy = new Academy();
            objAcademy.RegStudentId = 0;
            objAcademy.StudentId = id;
            objAcademy.SchoolName = passedCollege;
            objAcademy.PassingYear = passedYear;
            objAcademy.Board = passedBoard;
            objAcademy.MaxMark = passedMaxMark;
            objAcademy.ObtMark = passedObtainMark;
            objAcademy.Result = passedResult;
            objAcademy.Parcent = passedParcentage;
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
                    model.RegEx = varStudentDetail.RegEx;
                    model.Course = varStudentDetail.Course;

                    if (varStudentDetail.SchoolarNo != null)
                        model.SchoolarNo = varStudentDetail.SchoolarNo.ToString();

                    model.NewOld = varStudentDetail.NewOld;
                    model.SubCode = varStudentDetail.SubCode;
                    model.Medium = varStudentDetail.Medium;
                    model.Gender = varStudentDetail.Gender;
                    model.Caste = varStudentDetail.Caste;
                    model.StudentName = varStudentDetail.StudentName;
                    model.DOB = varStudentDetail.DOB;
                    model.AadhaarNo = varStudentDetail.AadhaarNo;
                    model.AbcNo = varStudentDetail.AbcNo;
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

            var StudentPhoto = PhotoBaseUrl + "\\Photo\\" + id + ".jpg"; 
            if (System.IO.File.Exists(StudentPhoto))
            {
                model.Photo = "/StudentData/Photo/" + id + ".jpg"; 
            }

            var TCPhoto = PhotoBaseUrl + "\\TC\\" + id + ".jpg";
            if (System.IO.File.Exists(TCPhoto))
            {
                model.TCPhoto = "/StudentData/TC/" + id + ".jpg";
            }


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

        //--------------------- Save Personal Detail--------------
        public IActionResult SavePromossionDetail11(int id, string varStudentName, string varFatherName, string varMotherName, string varMobileNo,
            string varDOB,string varFatherMobileNo, string varGender, string varMinority, string varCaset, string varAbcNo,
            string varSamagraID, string varAddress, string varTC, string varPH, string varAdhaarNo)
        {
            Student stuObj = new Student();
            stuObj.Id = id;
            stuObj.StudentName = varStudentName;
            stuObj.FatherName = varFatherName;
            stuObj.MotherName = varMotherName;
            stuObj.MobileNoOne = varMobileNo;
            if (!string.IsNullOrEmpty(varDOB))
                stuObj.DOB = Convert.ToDateTime(varDOB);

            stuObj.FatherMobileNo = varFatherMobileNo;
            stuObj.Gender = varGender;
            stuObj.Minority = varMinority;
            stuObj.Caste = varCaset;
            stuObj.AbcNo = varAbcNo;
            stuObj.AadhaarNo = varAdhaarNo;
            stuObj.SamagraID = varSamagraID;
            stuObj.Address = varAddress;
            stuObj.TC = varTC;
            stuObj.PH = varPH;
            stuObj.UpdateDatetime = DateTime.Now;
            stuObj.UpdatedBy = "UPdate Admin";

            var teee = _repoStudent.UpdatePersonalDetail(stuObj);

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
            stuObj.EnRollNo = varEnRollNo;
            stuObj.RollNo = varRollNo;
            stuObj.SchoolarNo = varSchoolarNo;
            stuObj.SubCode = varSubCode;
            stuObj.RegEx = varRegEx;
            stuObj.UpdateDatetime = DateTime.Now;
            stuObj.UpdatedBy = "UPdate Admin";

            var teee = _repoStudent.UpdateCollegeDetail(stuObj);

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
            objAcademy.SchoolName = varSchoolName;
            objAcademy.PassingYear = varAcademicYear;
            objAcademy.Board = varBoard;
            objAcademy.MaxMark = varMaxMark;
            objAcademy.ObtMark = varObtMark;
            objAcademy.Result = varResult;
            objAcademy.Parcent = varParcent;
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
            string varPromotCourse, string varPromoYear, string varCurrentYear, string varCurrentSession)

        {
            //-------Fee Master-------------
            var FeeMasterDetail = _feeRepo.GetFeeByClasssCouseSessionYearNewOld(varPromotClass, varPromotCourse, varPromotSession, varPromoYear, "New");
            if (FeeMasterDetail == null)
            {
                return Json(new { success = true, data = true });
            }
            //-------Old Fee Detail--------------
            var OldFeeDetail = _studentFeeRepo.GetFeeByClasssCouseSessionYearNewOld(id, varPromotClass, varPromotCourse, varCurrentSession, varCurrentYear, "New");


            //-------------Student Fee---------------
            StudentFee studentFee = new StudentFee();
            studentFee.StudentId = id;
            studentFee.Year = FeeMasterDetail.Year;
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
            studentFee.CMoneyPaidOrNot = "No";
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
            stuObj.UpdateDatetime = DateTime.Now;
            stuObj.UpdatedBy = "UPdate Admin";

            var teee = _repoStudent.PromoteStudentDetail(stuObj);


            return Json(new { success = true, data = true });
        }


        //----------Fill Exam Form--------------

        public IActionResult SaveExamFormFilled(string ids)
        {
           

            var teee = _repoStudent.ExamFormFilled(ids);

            return Json(new { success = true, data = true });
        }



    }
}
