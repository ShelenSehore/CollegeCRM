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
    public class PaymentController : Controller
    {
        private readonly AppSettings _mySettings;
        private readonly StudentRepository _repoStudent;
        private readonly StudentRegistrationRepository _repoStudentRegi;
        private readonly ILogger<PaymentController> _logger;
        private readonly MstCourseRepository _courseRepo;
        private readonly MstClassRepository _classRepo;
        private readonly string _baseUrl;
        private readonly MstSubjectRepository _subjecctRepo;
        private readonly MstSessionRepository _sessionRepo;
        private readonly MstYearRepository _yearRepo;
        private readonly MstFeeRepository _feeRepo;
        private readonly StudentFeeRepository _studentFeeRepo;
        private readonly StudentTransactionRepository _transactionRepo;
        private readonly StudentHistoryRepository _historyStudentRepo;
        public PaymentController(ILogger<PaymentController> logger,
           IOptions<AppSettings> config,
           IOptions<AppSettings> settings,
           MstClassRepository classRepo,
          MstCourseRepository courseRepo,
           MstSubjectRepository subjecctRepo,
           StudentRepository repoStudent,
           MstSessionRepository sessionRepo,
           MstYearRepository yearRepo,
           MstFeeRepository feeRepo,
            StudentFeeRepository studentFeeRepo,
            StudentTransactionRepository transactionRepo,
            StudentRegistrationRepository repoStudentRegi, StudentHistoryRepository historyStudentRepo
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
            _sessionRepo = sessionRepo;
            _yearRepo = yearRepo;
            _feeRepo = feeRepo;
            _studentFeeRepo = studentFeeRepo;
            _transactionRepo = transactionRepo;
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


        public IActionResult SearchList(string name, string classes, string course, string year, string session)
        {
            var data = _repoStudent.GetByStudentRegistrationPage(session, classes, course, year, name);
            return Json(new { success = true, data = data });
        }

        //-------------Get Payment Detail-----------
        public IActionResult StudentPaymentDetail(int id)
        {
            StudentPaymentDetailView returnObj = new StudentPaymentDetailView();

            var data = _repoStudent.GetById(id);
            if (data.AdmissionDate != null)
                data.FatherName = data.AdmissionDate.Value.ToString("dd/MM/yyyy");
            returnObj.studentDetail = data;
            //-----------Fee Detail--------
            var FeeDetail = _studentFeeRepo.GetFeeByClasssCouseSessionYearNewOld(id, data.Class, data.Course, data.Session, data.Year, data.NewOld);
            returnObj.studentFeeDetail = FeeDetail;

            return Json(new { success = true, data = returnObj });
        }


        //-------------------PaymentDetail----------------------------
        public IActionResult PaymentDetail(int id)
        {
            ViewBag.BaseUrl = _baseUrl;
            StudentPaymentDetailView returnObj = new StudentPaymentDetailView();
            //-------Student Detail---------
            var data = _repoStudent.GetById(id);
            returnObj.studentDetail = data;

            returnObj.ClassList = _classRepo.GetAll()
                       .Select(x => new SelectListItem
                       {
                           Value = x.Name.ToString(),
                           Text = x.Name
                       })
                       .ToList();

            returnObj.ClassList.FirstOrDefault(item => item.Value.ToUpper() == returnObj.studentDetail.Class.ToUpper()).Selected = true;
            //----Year----
            returnObj.YearList = _yearRepo.GetAll()
                       .Select(x => new SelectListItem
                       {
                           Value = x.Name.ToString(),
                           Text = x.Name
                       })
                       .ToList();
            returnObj.YearList.FirstOrDefault(item => item.Value.ToUpper() == returnObj.studentDetail.Year.ToUpper()).Selected = true;
            //----Session----
            returnObj.SessionList = _sessionRepo.GetAll()
                       .Select(x => new SelectListItem
                       {
                           Value = x.Name.ToString(),
                           Text = x.Name
                       })
                       .ToList();

            returnObj.SessionList.FirstOrDefault(item => item.Value.ToUpper() == returnObj.studentDetail.Session.ToUpper()).Selected = true;
            //------Course  -- Subject---
            var varAllSubject = _subjecctRepo.GetAll();

            returnObj.SubjectList = varAllSubject
                .GroupBy(x => x.Name)
                .Select(g => g.First())
                .Select(x => new SelectListItem
                {
                    Value = x.Name,
                    Text = x.Name
                })
                .ToList();

            returnObj.SubjectList.FirstOrDefault(item => item.Value.ToUpper() == returnObj.studentDetail.Course.ToUpper()).Selected = true;





            var PhotoBaseUrl = _mySettings.DocumentUrl;

            var StudentPhoto = PhotoBaseUrl + "\\Photo\\" + data.AdmissionFormNo + ".jpg";
            if (System.IO.File.Exists(StudentPhoto))
            {
                returnObj.studentDetail.Photo = "/StudentData/Photo/" + data.AdmissionFormNo + ".jpg";
            }

            //-----------Fee Detail--------
            //var FeeDetail = _studentFeeRepo.GetFeeByClasssCouseSessionYearNewOld(id, data.Class, data.Course, data.Session, data.Year, data.NewOld);
            //returnObj.studentFeeDetail = FeeDetail;
            var FeeDetail = _studentFeeRepo.GetFeeByStudentID(id);
            if (FeeDetail != null) 
            {
                returnObj.studentFeeDetail = FeeDetail.OrderByDescending(x => x.Id).FirstOrDefault();

                if (FeeDetail.Count() > 1) 
                {
                    int skipFirst = 1;
                    foreach (var feeRow in FeeDetail.OrderByDescending(x => x.Id)) 
                    {
                        if ((skipFirst != 1) &&(feeRow.PaidAmount < feeRow.TotalFeeAfterDiscount) )
                        {
                            returnObj.OldDuesAmount = returnObj.OldDuesAmount + (feeRow.TotalFeeAfterDiscount - feeRow.PaidAmount);

                            
                        }
                        skipFirst = skipFirst + 1;

                    }
                    
                }
                returnObj.FeeDueList = FeeDetail.OrderByDescending(x => x.Id).ToList();

            }
            


            //-------Transaction Detail---------------
            if (FeeDetail != null) 
            {
                var varPaymentTable = _transactionRepo.GetAllByStudentID(id);
                var varPaymentHistory = from tr in varPaymentTable
                                        join fee in returnObj.FeeDueList
                           on tr.StudentFeeId equals fee.Id
                                        select new
                                        {
                                            tr.Id,
                                            tr.StudentId,
                                            tr.StudentFeeId,
                                            tr.Head,
                                            tr.RecBookNo,
                                            tr.RecNumber,
                                            tr.Amount,
                                            tr.PaymentMode,
                                            tr.TransactionNo,
                                            tr.CreatedBy,
                                            tr.CreateDateTime,
                                            fee.Class,
                                            fee.Year,
                                            fee.Session,
                                            fee.Course,
                                            fee.TotalFeeAfterDiscount,
                                            fee.PaidAmount
                                        };
                List<PaymentHistoryModelForView> paymentHisotryList = new List<PaymentHistoryModelForView>();

                if (varPaymentHistory != null)
                {
                    foreach (var rowtr in varPaymentHistory)
                    {
                        PaymentHistoryModelForView payObj = new PaymentHistoryModelForView();
                        payObj.Id = rowtr.Id;
                        payObj.Head = rowtr.Head;
                        payObj.Amount = rowtr.Amount;
                        payObj.RecNumber = rowtr.RecNumber;
                        payObj.PaymentMode = rowtr.PaymentMode;
                        payObj.CreateDateTime = rowtr.CreateDateTime;
                        payObj.CreatedBy = rowtr.CreatedBy;
                        payObj.TransactionNo = rowtr.TransactionNo;
                        payObj.Class = rowtr.Class;
                        payObj.Session = rowtr.Session;
                        payObj.Year = rowtr.Year;
                        payObj.Course = rowtr.Course;

                        paymentHisotryList.Add(payObj);
                    }
                    returnObj.studentTransaction = paymentHisotryList;
                }


            }
            




            return View(returnObj);
        }

        //---------------Save Payment Detail--------------
        public IActionResult SavePaymentDetail(int studentId, int studentFeeId, string recBookNo, string recNumber, string paymentdate,
            string paymentMode, string transactionNo, string varHead1, string varHead2, string varHead3, string varHead4,
            int varAmount1=0, int varAmount2 = 0, int varAmount3 = 0, int varAmount4 = 0, int totalPay = 0,
            string payClass =null, string payCourse = null, string payYear = null, string paySession = null)
        {
            ViewBag.BaseUrl = _baseUrl;

            //--------------Get Student Fee ID-----------
            var getFeeID = _studentFeeRepo.GetFeeIdByClassSessionYear(studentId, payClass, payCourse, payYear, paySession);
            studentFeeId = getFeeID.Id;

            //---------Validate Head and total should be same-----
            if (varAmount1 + varAmount2 + varAmount3 + varAmount4 == totalPay)
            {
                var transactions = new List<StudentTransaction>();

                if ((varAmount1 != 0) && (varHead1 != "Select"))
                {
                    transactions.Add(new StudentTransaction
                    {
                        StudentFeeId = studentFeeId,
                        StudentId = studentId,
                        RecBookNo = recBookNo.ToUpper(),
                        RecNumber = recNumber.ToUpper(),
                        PaymentMode = paymentMode.ToUpper(),
                        TransactionNo = transactionNo.ToUpper(),
                        CreatedBy = "Admin",
                        Head = varHead1.ToUpper(),
                        Amount = varAmount1,
                        CreateDateTime = string.IsNullOrEmpty(paymentdate)
                            ? DateTime.Now
                            : Convert.ToDateTime(paymentdate)
                    });
                }
                if ((varAmount2 != 0) && (varHead2 != "Select"))
                {
                    transactions.Add(new StudentTransaction
                    {
                        StudentFeeId = studentFeeId,
                        StudentId = studentId,
                        RecBookNo = recBookNo.ToUpper(),
                        RecNumber = recNumber.ToUpper(),
                        PaymentMode = paymentMode.ToUpper(),
                        TransactionNo = transactionNo.ToUpper(),
                        CreatedBy = "Admin",
                        Head = varHead2.ToUpper(),
                        Amount = varAmount2,
                        CreateDateTime = string.IsNullOrEmpty(paymentdate)
                            ? DateTime.Now
                            : Convert.ToDateTime(paymentdate)
                    });
                }
                if ((varAmount3 != 0) && (varHead3 != "Select"))
                {
                    transactions.Add(new StudentTransaction
                    {
                        StudentFeeId = studentFeeId,
                        StudentId = studentId,
                        RecBookNo = recBookNo,
                        RecNumber = recNumber.ToUpper(),
                        PaymentMode = paymentMode.ToUpper(),
                        TransactionNo = transactionNo.ToUpper(),
                        CreatedBy = "Admin",
                        Head = varHead3.ToUpper(),
                        Amount = varAmount3,
                        CreateDateTime = string.IsNullOrEmpty(paymentdate)
                            ? DateTime.Now
                            : Convert.ToDateTime(paymentdate)
                    });
                }
                if ((varAmount4 != 0) && (varHead4 != "Select"))
                {
                    transactions.Add(new StudentTransaction
                    {
                        StudentFeeId = studentFeeId,
                        StudentId = studentId,
                        RecBookNo = recBookNo.ToUpper(),
                        RecNumber = recNumber.ToUpper(),
                        PaymentMode = paymentMode.ToUpper(),
                        TransactionNo = transactionNo,
                        CreatedBy = "Admin",
                        Head = varHead4.ToUpper(),
                        Amount = varAmount4,
                        CreateDateTime = string.IsNullOrEmpty(paymentdate)
                            ? DateTime.Now
                            : Convert.ToDateTime(paymentdate)
                    });
                }
                _transactionRepo.BulkSave(transactions);

                //-------------Update into Student Fee Table----
                StudentFee studentFee = new StudentFee();
                studentFee.Id = getFeeID.Id;


                //-------------If Coussion alreaddy apaid we skip--------
                if (getFeeID.CMoneyPaidOrNot.ToUpper() != "YES")
                {
                    if (varHead1 == "Caution money" || varHead2 == "Caution money" || varHead3 == "Caution money" || varHead4 == "Caution money")
                        studentFee.CMoneyPaidOrNot = "YES";
                    else
                        studentFee.CMoneyPaidOrNot = "NO";
                }
                else
                {
                    studentFee.CMoneyPaidOrNot = getFeeID.CMoneyPaidOrNot;
                }

                studentFee.PaidAmount = 0;

                if (varHead1 == "Admission fees" || varHead1 == "Tuition fees")
                    studentFee.PaidAmount = studentFee.PaidAmount +varAmount1;

                if (varHead2 == "Admission fees" || varHead2 == "Tuition fees")
                    studentFee.PaidAmount = studentFee.PaidAmount + varAmount2;
                if (varHead3 == "Admission fees" || varHead3 == "Tuition fees")
                    studentFee.PaidAmount = studentFee.PaidAmount + varAmount3;
                if (varHead4 == "Admission fees" || varHead4 == "Tuition fees")
                    studentFee.PaidAmount = studentFee.PaidAmount + varAmount4;
              
                _studentFeeRepo.UpdateOnlyFeeAmount(studentFee);

                //-------TC Issue Update into Student and History Table
                if (varHead1 == "Transfer certificate fees" || varHead2 == "Transfer certificate fees" || varHead3 == "Transfer certificate fees" || varHead4 == "Transfer certificate fees") 
                {
                    _repoStudent.TCIssue(studentId);
                    UpdateStudentHistory(studentId);
                }

            }

            return Json(new { success = true, data = "" }) ;
        }


        //------------Update Student History table-----------
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
                historyObj.TCIssue = SavedStudentTable.TC;//---
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



    }
}
