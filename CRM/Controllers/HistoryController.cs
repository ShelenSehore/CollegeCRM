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
    public class HistoryController : Controller
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
        public HistoryController(ILogger<StudentController> logger,

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


        public IActionResult Index()
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


            return View(model);
        }


        public IActionResult SearchOldStudentList(string name, string classes, string year, string course, string session)
        {
            //--------Get List
            var data = _repoStudent.GetByStudentHistoryPage(session, classes, course, year, name);

            return Json(new { success = true, data = data });
        }

        public IActionResult View(int? id) 
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


            int studentId = 0;
            if ((id != 0) && (id != null))
            {
                var varStudentDetail = _repoStudent.StudentHistoryGetById(id.Value);

                if (varStudentDetail != null)
                {
                    studentId = varStudentDetail.StudentId;
                    model.Id = varStudentDetail.StudentHistoryId;
                    model.AdmissionFormNo = varStudentDetail.AdmissionForm;
                    model.Year = varStudentDetail.Year;
                    model.EnRollNo = varStudentDetail.EnrolNo;
                    model.AdmissionDate = varStudentDetail.AdmissionDate;
                    model.Class = varStudentDetail.Classs;
                    model.RollNo = varStudentDetail.RollNo;

                    if (!string.IsNullOrEmpty(varStudentDetail.RegPvt))
                        model.RegEx = varStudentDetail.RegPvt.ToUpper();

                    model.Course = varStudentDetail.Course;

                    if (varStudentDetail.ScholerNo != null)
                        model.SchoolarNo = varStudentDetail.ScholerNo.ToString();

                    if (!string.IsNullOrEmpty(varStudentDetail.NewOld))
                        model.NewOld = varStudentDetail.NewOld.ToUpper();

                    //if (!string.IsNullOrEmpty(varStudentDetail.co))
                    //    model.SubCode = varStudentDetail.SubCode.ToUpper();

                    model.Medium = varStudentDetail.Medium;
                    model.Gender = varStudentDetail.Gender;
                    model.Caste = varStudentDetail.Cast;

                    if (!string.IsNullOrEmpty(varStudentDetail.StudentName))
                        model.StudentName = varStudentDetail.StudentName.ToUpper();

                    model.DOB = varStudentDetail.DOB;

                    if (!string.IsNullOrEmpty(varStudentDetail.AdharNo))
                        model.AadhaarNo = varStudentDetail.AdharNo.ToUpper();

                    if (!string.IsNullOrEmpty(varStudentDetail.AbcId))
                        model.AbcNo = varStudentDetail.AbcId.ToUpper();

                    if (!string.IsNullOrEmpty(varStudentDetail.SamagraId))
                        model.SamagraID = varStudentDetail.SamagraId.ToUpper();

                    if (!string.IsNullOrEmpty(varStudentDetail.FatherName))
                        model.FatherName = varStudentDetail.FatherName;

                    if (!string.IsNullOrEmpty(varStudentDetail.MotherName))
                        model.MotherName = varStudentDetail.MotherName.ToUpper();

                    if (!string.IsNullOrEmpty(varStudentDetail.MobileNo))
                        model.MobileNoOne = varStudentDetail.MobileNo.ToUpper();

                    //if (!string.IsNullOrEmpty(varStudentDetail.FatherMobileNo))
                    //    model.FatherMobileNo = varStudentDetail.FatherMobileNo.ToUpper();

                    model.TC = varStudentDetail.TCIssue;
                    model.PH = varStudentDetail.PH;

                    if (!string.IsNullOrEmpty(varStudentDetail.Address))
                        model.Address = varStudentDetail.Address.ToUpper();

                    model.Minority = varStudentDetail.Minority;
                    model.ExamFormSubmited = varStudentDetail.ExamFormSubmited;

                    //---Dropdown--
                    model.SelectedClass = varStudentDetail.Classs;
                    model.SelectedSubject = model.Subject;
                    model.SelectedCourse = varStudentDetail.Course;
                    model.SelectedYear = varStudentDetail.Year;
                    model.Session = varStudentDetail.Session;


                }
            }
            //-----------Acadmic Detail-----------------
            var varAcadmicList = _repoAcademic.GetListByStudentId(studentId);
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

            ViewBag.BaseUrl = _baseUrl;
            
            return View(model);
        }

        //--------------------- Save Personal Detail--------------
        public IActionResult SaveStudentHistory(int id, string varStudentName, string varFatherName, string varMotherName, string varMobileNo,
            string varDOB, string varFatherMobileNo, string varGender, string varMinority, string varCaset, string varAbcNo,
            string varSamagraID, string varAddress, string varTC, string varPH, string varAdhaarNo)
        {
            StudentHistory stuObj = new StudentHistory();
            stuObj.StudentHistoryId = id;

            if (!string.IsNullOrEmpty(varStudentName))
                stuObj.StudentName = varStudentName.ToUpper();

            if (!string.IsNullOrEmpty(varFatherName))
                stuObj.FatherName = varFatherName.ToUpper();

            if (!string.IsNullOrEmpty(varMotherName))
                stuObj.MotherName = varMotherName.ToUpper();

            if (!string.IsNullOrEmpty(varMobileNo))
                stuObj.MobileNo = varMobileNo.ToUpper();

            if (!string.IsNullOrEmpty(varDOB))
                stuObj.DOB = Convert.ToDateTime(varDOB);

           // stuObj.FatherMobileNo = varFatherMobileNo;
            stuObj.Gender = varGender;
            stuObj.Minority = varMinority;
            stuObj.Cast = varCaset;

            if (!string.IsNullOrEmpty(varAbcNo))
                stuObj.AbcId = varAbcNo.ToUpper();

            if (!string.IsNullOrEmpty(varAdhaarNo))
                stuObj.AdharNo = varAdhaarNo.ToUpper();

            if (!string.IsNullOrEmpty(varSamagraID))
                stuObj.SamagraId = varSamagraID.ToUpper();

            if (!string.IsNullOrEmpty(varAddress))
                stuObj.Address = varAddress.ToUpper();

            stuObj.TCIssue = varTC;
            stuObj.PH = varPH;
            stuObj.UpdateDate = DateTime.Now;
            stuObj.CreateBy = "Update Admin";

            _historyStudentRepo.UpdateHistoryDetailForHistoryPage(stuObj);

            
            return Json(new { success = true, data = true });
        }


        //--------------------- Save Update Detail--------------
        public IActionResult UpdateCollegeDetail(int id, string varSession, string varNewOld, string varMedium,
            string varClass, string varCourse, string varYear, string varAdmissionFormNo, string varEnRollNo,
             string varAdmissionDate, string varRollNo, string varSchoolarNo, string varSubCode, string varRegEx)

        {
            StudentHistory stuObj = new StudentHistory();
            stuObj.StudentHistoryId = id;

            if (!string.IsNullOrEmpty(varAdmissionFormNo))
                stuObj.AdmissionForm = Convert.ToInt32(varAdmissionFormNo);


            if (!string.IsNullOrEmpty(varAdmissionDate))
                stuObj.AdmissionDate = Convert.ToDateTime(varAdmissionDate);


            stuObj.NewOld = varNewOld;
            stuObj.Medium = varMedium;
            //stuObj.Session = varSession;   //-------------varSession
            //stuObj.Class = varClass;  //-------------Class
            //stuObj.Course = varCourse;  //-------Course
            //stuObj.Year = varYear;  //----------Year
            if (!string.IsNullOrEmpty(varEnRollNo))
                stuObj.EnrolNo = varEnRollNo.ToUpper();

            if (!string.IsNullOrEmpty(varRollNo))
                stuObj.RollNo = varRollNo.ToUpper();


            if (!string.IsNullOrEmpty(varSchoolarNo))
                stuObj.ScholerNo = varSchoolarNo.ToUpper();

            //if (!string.IsNullOrEmpty(varSubCode))
            //    stuObj.SubCode = varSubCode.ToUpper();

            stuObj.RegPvt = varRegEx;
            stuObj.UpdateDate = DateTime.Now;
            stuObj.UpdateBy = "Update Admin";

            var teee = _historyStudentRepo.UpdateHisotryCollegeDetailForHistoryPage(stuObj);

            return Json(new { success = true, data = true });
        }



    }
}
