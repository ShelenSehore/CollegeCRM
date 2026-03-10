using CRM.Data;
using CRM.ModelsForView;
using CRM.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Controllers
{
    public class DashboardController : Controller
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
        public DashboardController(ILogger<StudentController> logger,

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
        public IActionResult Index()
        {
            DashboardModelView modelView = new DashboardModelView();
            modelView = _repoStudent.DashboardDetail();

            return View(modelView);
        }
    }
}
