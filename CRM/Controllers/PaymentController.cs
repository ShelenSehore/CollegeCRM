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

        public PaymentController(ILogger<PaymentController> logger,
           IOptions<AppSettings> config,
           MstClassRepository classRepo,
          MstCourseRepository courseRepo,
           MstSubjectRepository subjecctRepo,
           StudentRepository repoStudent,
           MstSessionRepository sessionRepo,
           MstYearRepository yearRepo,
           MstFeeRepository feeRepo,
            StudentFeeRepository studentFeeRepo,
            StudentTransactionRepository transactionRepo,
            StudentRegistrationRepository repoStudentRegi)
        {
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
            var data = _repoStudent.FilterList(name, classes, course, year, session);
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
                
            }
            


            //-------Transaction Detail---------------
            if (FeeDetail != null) 
            {
                returnObj.studentTransaction = _transactionRepo.GetAllByStudentIDandFeeID(id, returnObj.studentFeeDetail.Id);
                
            }
            




            return View(returnObj);
        }

        //---------------Save Payment Detail--------------
        public IActionResult SavePaymentDetail(int studentId, int studentFeeId, string recBookNo, string recNumber, string paymentdate,
            string paymentMode, string transactionNo, string varHead1, string varHead2, string varHead3, string varHead4,
            int varAmount1=0, int varAmount2 = 0, int varAmount3 = 0, int varAmount4 = 0, int totalPay = 0)
        {
            ViewBag.BaseUrl = _baseUrl;

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
                        RecBookNo = recBookNo,
                        RecNumber = recNumber,
                        PaymentMode = paymentMode,
                        TransactionNo = transactionNo,
                        CreatedBy = "Admin",
                        Head = varHead1,
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
                        RecBookNo = recBookNo,
                        RecNumber = recNumber,
                        PaymentMode = paymentMode,
                        TransactionNo = transactionNo,
                        CreatedBy = "Admin",
                        Head = varHead2,
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
                        RecNumber = recNumber,
                        PaymentMode = paymentMode,
                        TransactionNo = transactionNo,
                        CreatedBy = "Admin",
                        Head = varHead3,
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
                        RecBookNo = recBookNo,
                        RecNumber = recNumber,
                        PaymentMode = paymentMode,
                        TransactionNo = transactionNo,
                        CreatedBy = "Admin",
                        Head = varHead4,
                        Amount = varAmount4,
                        CreateDateTime = string.IsNullOrEmpty(paymentdate)
                            ? DateTime.Now
                            : Convert.ToDateTime(paymentdate)
                    });
                }
                _transactionRepo.BulkSave(transactions);


                //-------------Update into Student Fee Table----
                StudentFee studentFee = new StudentFee();
                studentFee.Id = studentFeeId;
                studentFee.PaidAmount = totalPay;
                if(varHead1 == "Caution money" || varHead2 == "Caution money" || varHead3 == "Caution money" || varHead4 == "Caution money")
                 studentFee.CMoneyPaidOrNot = "Yes";


                _studentFeeRepo.UpdateOnlyFeeAmount(studentFee);

            }

            return Json(new { success = true, data = "" }) ;
        }



    }
}
