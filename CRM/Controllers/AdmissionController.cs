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
        private readonly StudentHistoryRepository _historyStudentRepo;

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
                StudentHistoryRepository historyStudentRepo,
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
            _historyStudentRepo = historyStudentRepo;
        }
        public IActionResult Index()
        {
            ViewBag.BaseUrl = _baseUrl;
            var model = new StudentListView();

            //----Session----
            model.SessionList = _sessionRepo.GetAll()
                       .Select(x => new SelectListItem
                       {
                           Value = x.Name.ToString(),
                           Text = x.Name
                       })
                       .ToList();

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




        public IActionResult SearchList(string name, string classes, string year, string course, string session)
        {


            //--------Get List
            var data = _repoStudentRegi.FilterList(name, classes, year, course, session);



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
            //----------StudentRegistration No-------
           var newRegistrationNo =  _repoStudentRegi.GetLatestId();

            newRegistrationNo = newRegistrationNo + 1;

            model.RegNo = newRegistrationNo;

            ViewBag.BaseUrl = _baseUrl;
            model.DisBy = "None";
            model.DisResion = "None";
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

            if(!string.IsNullOrEmpty(student.DisBy))
            obj.DisBy = student.DisBy.ToUpper();

            if (!string.IsNullOrEmpty(student.DisResion))
                obj.DisResion = student.DisResion;

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
                studentFee.CMoneyPaidOrNot = "NO";
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


        public IActionResult ViewAdmission(int Id)
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

            //-------------------

           


            //--------Get Student detail---------
            var getValue = _repoStudentRegi.GetById(Id);
            model.Id = getValue.Id;
            model.Name = getValue.Name;
            model.FatherName = getValue.FatherName;
            model.MotherName = getValue.MotherName;
            model.MobileNo = getValue.MobileNo;
            model.DOB = getValue.DOB;
            model.Gender = getValue.Gender;
            model.Caste = getValue.Caste;

            model.Session = getValue.Session;
            model.Year = getValue.Year;
            model.Class = getValue.Subject;
            model.Course = getValue.Course;
            model.RegNo = getValue.RegNo;
            model.FormNo = getValue.FormNo;
            model.SchoNo = getValue.SchoNo;
            model.RegPvt = getValue.RegPvt;
            model.Status = getValue.Status;

            var getFeeDetail = _studentRegiFee.GetById(Id);
            model.RegFeeId = getFeeDetail.Id;
            
            model.NewStudentFee = getFeeDetail.NewStudentFee;
            model.CMoney = getFeeDetail.CMoney;
            model.TutionFee = getFeeDetail.TutionFee;
            model.OtherFee = getFeeDetail.OtherFee;
            model.TotalFee = getFeeDetail.TotalFee;
            model.Scholership = getFeeDetail.Scholership;
            model.DisBy = getFeeDetail.DisBy;
            model.DisResion = getFeeDetail.DisResion;
            model.TotalFeeAfterDiscount = getFeeDetail.TotalFeeAfterDiscount;
            
            return View(model);
        }


        [HttpPost]
        public IActionResult NewAdmissionUpdatePost(StudentRegistrationForView student)
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
            obj.DisBy = student.DisBy;
            obj.DisResion = student.DisResion;
            obj.CreateBy = "Admin";
            obj.CreateDate = DateTime.Now;
            obj.Id = student.Id;
            try
            {
                 _repoStudentRegi.Update(obj);

                //-------------Student Fee---------------
                StudentRegitrationFee studentFee = new StudentRegitrationFee();
                studentFee.Id = student.RegFeeId;
                studentFee.StudentId = student.Id;
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
                studentFee.DisBy = student.DisBy;
                studentFee.DisResion = student.DisResion;
                studentFee.CreatedBy = "Admin";
                studentFee.CreatedDateTime = DateTime.Now;
                studentFee.UpdateDateTime = DateTime.Now;
                studentFee.UpdateBy = "";
                _studentRegiFee.Update(studentFee);

            }
            catch (Exception ex)
            {
            }




            TempData["msg"] = "Student Added Successfully!";
            return RedirectToAction("Index");
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
            if (!string.IsNullOrEmpty(studentList))
            {
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

                            stuObj.Photo = student.FormNo.ToString();

                            if (student.Session != null)
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
                            stuObj.RegEx = student.RegPvt;

                            if(student.SchoNo.HasValue)
                            stuObj.SchoolarNo = student.SchoNo.Value.ToString();

                            stuObj.NewOld = "NEW";
                            stuObj.CreateBy = "Admin";
                            stuObj.CreateDatetime = DateTime.Now;

                            var SavedStudentTable = _repoStudent.SaveAndGetAllDetail(stuObj);

                            //---------Get Fee detail from FeeReigtration table----------
                            var varFeeDetail = _studentRegiFee.GetById(Convert.ToInt32(RegistudentId));
                            if (RegistudentId != null)
                            {
                                StudentFee studentFee = new StudentFee();
                                studentFee.StudentId = SavedStudentTable.Id;
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

                            //--------------Student History Table--------------
                            
                            StudentHistory historyObj = new StudentHistory();
                            historyObj.StudentId = SavedStudentTable.Id;
                            historyObj.AdmissionForm = SavedStudentTable.AdmissionFormNo.Value;
                            historyObj.AdmissionDate = DateTime.Now;
                            historyObj.Session = SavedStudentTable.Session.ToUpper();
                            historyObj.Classs = SavedStudentTable.Class.ToUpper();
                            historyObj.Course = SavedStudentTable.Course.ToUpper();
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

                            if(SavedStudentTable.DOB.HasValue)
                            historyObj.DOB = SavedStudentTable.DOB.Value;

                            historyObj.Address = SavedStudentTable.Address; //----
                            historyObj.MobileNo = SavedStudentTable.MobileNoOne;
                           // historyObj.TCIssue = SavedStudentTable.TCissue;//---
                            historyObj.SamagraId = SavedStudentTable.SamagraID; //---
                            historyObj.AdharNo = SavedStudentTable.AadhaarNo; //----
                            historyObj.ExamFormSubmited = SavedStudentTable.ExamFormSubmited;

                            historyObj.Photo = SavedStudentTable.Photo;

                            historyObj.CreateBy = "Admin";
                            historyObj.CreateDate = DateTime.Now;
                            historyObj.UpdateBy = "Admin";
                            historyObj.UpdateDate = DateTime.Now;
                            _historyStudentRepo.Add(historyObj);


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


        [HttpPost]
        public IActionResult DownloadExcel(string hdnExcelSession, string hdnExcelClass, string hdnExcelCourse, string hdnExcelYear, string hdnExcelName)
        {
            try
            {

                var students = _repoStudentRegi.DownLoadExcelFilterList(hdnExcelName, hdnExcelClass, hdnExcelYear, hdnExcelCourse, hdnExcelSession);

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Student List");

                    // Header
                    worksheet.Cell(1, 1).Value = "Id";
                    worksheet.Cell(1, 2).Value = "RegNo";
                    worksheet.Cell(1, 3).Value = "FormNo";
                    worksheet.Cell(1, 4).Value = "SchoNo";
                    worksheet.Cell(1, 5).Value = "Session";
                    worksheet.Cell(1, 6).Value = "Year";
                    worksheet.Cell(1, 7).Value = "Class";
                    worksheet.Cell(1, 8).Value = "Course";
                    worksheet.Cell(1, 9).Value = "Sem";
                    worksheet.Cell(1, 10).Value = "RegPvt";
                    worksheet.Cell(1, 11).Value = "Status";
                    worksheet.Cell(1, 12).Value = "Name";
                    worksheet.Cell(1, 13).Value = "FatherName";
                    worksheet.Cell(1, 14).Value = "MotherName";
                    worksheet.Cell(1, 15).Value = "DOB";
                    worksheet.Cell(1, 16).Value = "Caste";
                    worksheet.Cell(1, 17).Value = "Gender";
                    worksheet.Cell(1, 18).Value = "MobileNo";
                    worksheet.Cell(1, 19).Value = "CreateBy";
                    worksheet.Cell(1, 20).Value = "IsMove";
                    worksheet.Cell(1, 21).Value = "NewOld";
                    worksheet.Cell(1, 22).Value = "NewStudentFee";
                    worksheet.Cell(1, 23).Value = "CMoney";
                    worksheet.Cell(1, 24).Value = "TutionFee";
                    worksheet.Cell(1, 25).Value = "OtherFee";
                    worksheet.Cell(1, 26).Value = "TotalFee";
                    worksheet.Cell(1, 27).Value = "TotalFeeCM";
                    worksheet.Cell(1, 28).Value = "TotalFeeAfterDiscount";
                    worksheet.Cell(1, 29).Value = "PaidAmount";
                    worksheet.Cell(1, 30).Value = "ScholershipExcel";
                    worksheet.Cell(1, 31).Value = "DisByExcel";
                    worksheet.Cell(1, 32).Value = "DisResionExcel";
                    worksheet.Cell(1, 33).Value = "CMoneyPaidOrNot";
                    worksheet.Row(1).Style.Font.Bold = true;

                    int row = 2;
                    foreach (var item in students)
                    {
                        worksheet.Cell(row, 1).Value = item.Id;
                        worksheet.Cell(row, 2).Value = item.RegNo;
                        worksheet.Cell(row, 3).Value = item.FormNo;
                        worksheet.Cell(row, 4).Value = item.SchoNo;
                        worksheet.Cell(row, 5).Value = item.Session;
                        worksheet.Cell(row, 6).Value = item.Year;
                        worksheet.Cell(row, 7).Value = item.Subject;
                        worksheet.Cell(row, 8).Value = item.Course;
                        worksheet.Cell(row, 9).Value = item.Sem;
                        worksheet.Cell(row, 10).Value = item.RegPvt;
                        worksheet.Cell(row, 11).Value = item.Status;
                        worksheet.Cell(row, 12).Value = item.Name;
                        worksheet.Cell(row, 13).Value = item.FatherName;
                        worksheet.Cell(row, 14).Value = item.MotherName;
                        worksheet.Cell(row, 15).Value = item.DOB;
                        worksheet.Cell(row, 16).Value = item.Caste;
                        worksheet.Cell(row, 17).Value = item.Gender;
                        worksheet.Cell(row, 18).Value = item.MobileNo;
                        worksheet.Cell(row, 19).Value = item.CreateBy;
                        worksheet.Cell(row, 20).Value = item.CreateDate;
                        worksheet.Cell(row, 21).Value = item.UpdateBy;
                        worksheet.Cell(row, 22).Value = item.UpdateDate;
                        worksheet.Cell(row, 23).Value = item.IsMove.HasValue.ToString();
                        worksheet.Cell(row, 24).Value = item.NewOld;
                        worksheet.Cell(row, 25).Value = item.NewStudentFee;
                        worksheet.Cell(row, 26).Value = item.CMoney;
                        worksheet.Cell(row, 27).Value = item.TutionFee;
                        worksheet.Cell(row, 28).Value = item.OtherFee;
                        worksheet.Cell(row, 29).Value = item.TotalFee;
                        worksheet.Cell(row, 30).Value = item.TotalFeeCM;
                        worksheet.Cell(row, 31).Value = item.TotalFeeAfterDiscount;
                        worksheet.Cell(row, 32).Value = item.PaidAmount;
                        worksheet.Cell(row, 33).Value = item.ScholershipExcel;
                        worksheet.Cell(row, 34).Value = item.DisByExcel;
                        worksheet.Cell(row, 35).Value = item.DisResionExcel;
                        worksheet.Cell(row, 36).Value = item.CMoneyPaidOrNot;
                        
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
