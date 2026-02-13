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
    public class FeeController : Controller
    {
        private readonly ILogger<FeeController> _logger;

        private readonly MstClassRepository _classRepo;
        private readonly MstCourseRepository _courseRepo;
        private readonly MstFeeRepository _feeRepo;
        private readonly MstSubjectRepository _subjecctRepo;
        private readonly string _baseUrl;
        private readonly MstSessionRepository _sessionRepo;
        private readonly MstYearRepository _yearRepo;

        public FeeController(ILogger<FeeController> logger,
           IOptions<AppSettings> config,

           MstSubjectRepository subjecctRepo,
           MstFeeRepository fee,
           MstCourseRepository courseRepo,
           MstYearRepository yearRepo,
            MstSessionRepository sessionRepo,
           MstClassRepository classRepo)
        {
            _subjecctRepo = subjecctRepo;
            _courseRepo = courseRepo;

            _classRepo = classRepo;
            _feeRepo = fee;
            _logger = logger;
            _baseUrl = config.Value.BaseUrl;
            _yearRepo = yearRepo;
            _sessionRepo = sessionRepo;
        }

        public IActionResult Index()
        {
            ViewBag.BaseUrl = _baseUrl;
            var model = new FeeViewModel();



            var varSubject = _subjecctRepo.GetAll();
            var varFee = _feeRepo.GetAll();

            if (varFee != null)
            {
                List<FeeListModel> feeClasssList = new List<FeeListModel>();
                foreach (var row in varFee)
                {
                    FeeListModel obj = new FeeListModel();

                    //var temName = varSubject
                    //                .Where(x => x.Id == Convert.ToInt32(row.Course))
                    //                .Select(x => x.Name)
                    //                .FirstOrDefault();
                    //var temCourse = varSubject
                    //                .Where(x => x.Id == Convert.ToInt32(row.Course))
                    //                .Select(x => x.Course)
                    //                .FirstOrDefault();
                    //var temClass = varSubject
                    //                .Where(x => x.Id == Convert.ToInt32(row.Course))
                    //                .Select(x => x.Class)
                    //                .FirstOrDefault();

                    //obj.SubjectName = temName+" / "+ temCourse + " / " + temClass + " / " ;

                    obj.Id = row.Id;
                    obj.NewOld = row.NewOld;
                    obj.Session = row.Session;
                    obj.Year = row.Year;
                    obj.NewStudentFee = row.NewStudentFee;
                    obj.CMoney = row.CMoney;
                    obj.TutionFee = row.TutionFee;
                    obj.OtherFee = row.OtherFee;
                    obj.TotalFee = row.TotalFee;
                    obj.TotalFeeCM = row.TotalFeeCM;

                    feeClasssList.Add(obj);
                }
                model.FeeClassList = feeClasssList.OrderBy(x => x.Course).ToList();

            }



            return View(model);
        }


        public IActionResult AddFee()
        {
            ViewBag.BaseUrl = _baseUrl;
            var model = new FeeViewModel();

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



        [HttpPost]
        public IActionResult FeePost(FeeViewModel feeViewModel)
        {
            //if (string.IsNullOrWhiteSpace(className))
            //    return Json(new { success = false, message = "Course name is required" });

            var model = new MstFee
            {

                NewOld = feeViewModel.NewOld,
                Session = feeViewModel.Session,
                Year = feeViewModel.Year,
                Subject = feeViewModel.Subject,
                Course = feeViewModel.Course,
                NewStudentFee = feeViewModel.NewStudentFee,
                CMoney = feeViewModel.CMoney,
                TutionFee = feeViewModel.TutionFee,
                OtherFee = feeViewModel.OtherFee,
                TotalFee = feeViewModel.TotalFee,
                TotalFeeCM = feeViewModel.TotalFeeCM


            };

            _feeRepo.Add(model);

            return RedirectToAction("index");
        }


        [HttpPost]
        public IActionResult DeleteFee(int id)
        {
            _feeRepo.Delete(id);
            return Json(new { success = true });
        }

        public IActionResult GetFee(int id)
        {
            // var data = _FeeClassRepo.GetById(id);

            return Json(new { success = true });
        }

        //---------------------Submit Fee---------------

        public IActionResult SearchForFee()
        {
            return View();
        }


    }
}