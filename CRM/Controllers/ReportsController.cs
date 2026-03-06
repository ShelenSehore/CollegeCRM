using CRM.Data;
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
    public class ReportsController : Controller
    {
        private readonly AppSettings _mySettings;

        private readonly StudentRepository _repoStudent;
        private readonly StudentTransactionRepository _repoStudTransaction;
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


        public ReportsController(ILogger<StudentController> logger,

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
             StudentTransactionRepository repoStudTransaction)
        {

            _mySettings = settings.Value;
            _repoStudent = repoStudent;
            _repoStudTransaction = repoStudTransaction;
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


            ////--------Get List
            //var data = _repoStudent.GetAll();

            //model.StudentList = data;

            return View(model);
        }


       
        public IActionResult GetStudentList(string had, string paymentMode, string reciptNo, string fromDate, string toDate, string name,
            string session, string year, string classes, string course)
        {

             var data = _repoStudTransaction.FilterList(had, paymentMode, reciptNo, fromDate, toDate, name, session, year, classes, course);


            return Json(new { success = true, data = data });
        }



        public IActionResult Dfc()
        {
            return View();
        }
    }
}
