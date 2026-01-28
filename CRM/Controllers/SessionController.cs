using CRM.Data;
using CRM.Models;
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
    public class SessionController : Controller
    {
        private readonly ILogger<SessionController> _logger;
        private readonly MstSessionRepository _sessionRepo;
        private readonly string _baseUrl;
        public SessionController(ILogger<SessionController> logger,
           IOptions<AppSettings> config,
           MstSessionRepository sessionRepo)
        {
            _sessionRepo = sessionRepo;
            _logger = logger;
            _baseUrl = config.Value.BaseUrl;
        }


        public IActionResult Index()
        {
            ViewBag.BaseUrl = _baseUrl;
            var data = _sessionRepo.GetAll();

            return View(data);
        }

        [HttpPost]
        public IActionResult Create(string className)
        {
            if (string.IsNullOrWhiteSpace(className))
                return Json(new { success = false, message = "Class name is required" });

            var model = new MstSession
            {
                Name = className,
                CreatedBy = "Admin",
                CreateDateTime = DateTime.Now
            };

            _sessionRepo.Add(model);

            return Json(new { success = true });
        }

        public IActionResult Get(int id)
        {
            var data = _sessionRepo.GetById(id);
            return Json(data);
        }

        [HttpPost]
        public IActionResult Update(int id, string className)
        {
            var data = _sessionRepo.GetById(id);

            if (data == null)
                return Json(new { success = false, message = "Year not found" });

            data.Name = className;
            data.CreatedBy = "Admin";
            data.CreateDateTime = DateTime.Now;

            _sessionRepo.Update(data);

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _sessionRepo.Delete(id);
            return Json(new { success = true });
        }

    }
}
