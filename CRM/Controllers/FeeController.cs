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
        private readonly MstFeeClassRepository _FeeClassRepo;
        private readonly MstClassRepository _classRepo;
        private readonly MstCourseRepository _courseRepo;
        private readonly MstFeeRepository _feeRepo;
        private readonly string _baseUrl;

        public FeeController(ILogger<FeeController> logger,
           IOptions<AppSettings> config,
           MstFeeClassRepository FeeClassRepo,
           MstFeeRepository fee,
           MstCourseRepository courseRepo,
           MstClassRepository classRepo)
        {
            _courseRepo = courseRepo;
            _FeeClassRepo = FeeClassRepo;
            _classRepo = classRepo;
            _feeRepo = fee;
            _logger = logger;
            _baseUrl = config.Value.BaseUrl;
        }

        public IActionResult Index()
        {
            ViewBag.BaseUrl = _baseUrl;
            var model = new FeeViewModel();

            model.CourseList = _courseRepo.GetAll()
                     .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.CourseName })
                     .ToList();

            model.ClassList = _classRepo.GetAll()
                       .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
                       .ToList();

            model.FeeList = _feeRepo.GetAll()
                      .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Type })
                      .ToList();

            var feeList = _FeeClassRepo.GetAll();

            if (feeList != null) 
            {
                List<FeeListModel> feeClasssList = new List<FeeListModel>();
                foreach (var row in feeList) 
                {
                    FeeListModel obj = new FeeListModel();
                    obj.Id = row.Id;
                    obj.FeeId = row.FeeId;
                    obj.FeeName = _feeRepo.GetAll()
                                    .Where(x => x.Id == row.FeeId)
                                    .Select(x => x.Type)
                                    .FirstOrDefault();
                    obj.Amount = _feeRepo.GetAll()
                                    .Where(x => x.Id == row.FeeId)
                                    .Select(x => x.Amount)
                                    .FirstOrDefault();

                    obj.Course = row.Course;
                    obj.Class = row.Class;

                    feeClasssList.Add(obj);
                }
                model.FeeClassList = feeClasssList.OrderBy(x=>x.Class).ToList();

            }



            return View(model);
        }



        [HttpPost]
        public IActionResult FeePost(string feeId , string className, string couseName)
        {
            if (string.IsNullOrWhiteSpace(className))
                return Json(new { success = false, message = "Course name is required" });

            var model = new MstFeeClass
            {
                FeeId = Convert.ToInt32(feeId),
                Class = className,
                Course = couseName

            };

            _FeeClassRepo.Add(model);

            return Json(new { success = true });
        }


        [HttpPost]
        public IActionResult DeleteFee(int id)
        {
            _FeeClassRepo.Delete(id);
            return Json(new { success = true });
        }

        public IActionResult GetFee(int id)
        {
            var data = _FeeClassRepo.GetById(id);

            return Json(data);
        }


    }
}
