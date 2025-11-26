using CRM.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Controllers
{
    public class SujectController : Controller
    {
        private readonly MstSubjectRepository _subjecctRepo;
        private readonly ILogger<SujectController> _logger;

        public SujectController(ILogger<SujectController> logger,
           MstSubjectRepository subjecctRepo)
        {
            _subjecctRepo = subjecctRepo;
            _logger = logger;
        }
        public IActionResult Index()
        {
            var data = _subjecctRepo.GetAll();
            return View(data);

        }
    }
}
