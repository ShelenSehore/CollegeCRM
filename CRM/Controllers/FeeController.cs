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
    public class FeeController : Controller
    {
        private readonly ILogger<FeeController> _logger;
        private readonly MstFeeClassRepository _classFeeClass;
        private readonly MstClassRepository _classRepo;
        private readonly MstFeeRepository _fee;
        private readonly string _baseUrl;

        public FeeController(ILogger<FeeController> logger,
           IOptions<AppSettings> config,
           MstFeeClassRepository classFeeClass,
           MstFeeRepository fee,
           MstClassRepository classRepo)
        {
            _fee = fee;
            _classRepo = classRepo;
            _logger = logger;
            _baseUrl = config.Value.BaseUrl;
        }

        public IActionResult Index()
        {
            ViewBag.BaseUrl = _baseUrl;
            var model = new FeeViewModel();

            model.ClassList = _classRepo.GetAll()
                       .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
                       .ToList();

            model.FeeList = _fee.GetAll()
                      .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
                      .ToList();

            var feeList = _classFeeClass.GetAll();




            return View(model);
        }
    }
}
