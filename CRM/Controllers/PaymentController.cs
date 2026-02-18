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

            var data = _repoStudent.GetById(id);
            
            returnObj.studentDetail = data;
            //-----------Fee Detail--------
            var FeeDetail = _studentFeeRepo.GetFeeByClasssCouseSessionYearNewOld(id, data.Class, data.Course, data.Session, data.Year, data.NewOld);
            returnObj.studentFeeDetail = FeeDetail;

            return View(returnObj);
        }

        //---------------Save Payment Detail--------------
        public IActionResult SavePaymentDetail(int studentId, int studentFeeId, string recBookNo, string recNumber,
            string paymentMode, string transactionNo, string varHead1, string varHead2, string varHead3, string varHead4,
            int varAmount1, int varAmount2, int varAmount3, int varAmount4, int totalPay)
        {
            ViewBag.BaseUrl = _baseUrl;

            StudentTransaction saveTransaction = new StudentTransaction();
            saveTransaction.StudentFeeId = studentFeeId;
            saveTransaction.StudentId = studentId;
            saveTransaction.RecBookNo = recBookNo;
            saveTransaction.RecNumber = recNumber;
            saveTransaction.PaymentMode = paymentMode;
            saveTransaction.TransactionNo = transactionNo;
            saveTransaction.CreateDateTime = DateTime.Now;
            saveTransaction.CreatedBy = "Admin";
            saveTransaction.Head = varHead1;
            saveTransaction.Amount = varAmount1;

            var teee =_transactionRepo.SaveAndGetId(saveTransaction);


            return View();
        }



    }
}
