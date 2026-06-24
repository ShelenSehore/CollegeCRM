using CRM.Models;
using CRM.ModelsForView;
using CRM.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MstClassRepository _repo;
        private readonly MstSessionRepository _sessionRepo;
        public HomeController(ILogger<HomeController> logger,
            MstSessionRepository sessionRepo,
            MstClassRepository repo)
        {
            _repo = repo;
            _logger = logger;
            _sessionRepo = sessionRepo;
        }

        public IActionResult Index()
        {
            //----Session----
            ViewBag.Session = _sessionRepo.GetAll()
                       .Select(x => new SelectListItem
                       {
                           Value = x.Name.ToString(),
                           Text = x.Name
                       })
                       .ToList();
            LoginViewModel model = new LoginViewModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult IndexPost(LoginViewModel model)
        {
            try { 
            //----Session----
            ViewBag.Session = _sessionRepo.GetAll()
                       .Select(x => new SelectListItem
                       {
                           Value = x.Name.ToString(),
                           Text = x.Name
                       })
                       .ToList();

            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            HttpContext.Session.Clear();
            HttpContext.Session.SetString("Session", model.Session);
            //HttpContext.Session.SetString("UserName", model.Email);
            //HttpContext.Session.SetString("UserId", user.Id.ToString());
            //HttpContext.Session.SetString("Role", user.Role);

            return RedirectToAction("index","Dashboard");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
