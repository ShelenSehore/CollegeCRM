using CRM.Models;
using CRM.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentRepository _repoStudent;
        private readonly ILogger<StudentController> _logger;
        public StudentController(ILogger<StudentController> logger,
            StudentRepository repoStudent)
        {
            _repoStudent = repoStudent;
            _logger = logger;
        }
        public IActionResult Index()
        {
            //var data = _repoStudent.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }

            student.AdmissionDate = DateTime.Now;

            _repoStudent.Add(student);

            TempData["msg"] = "Student Added Successfully!";
            return RedirectToAction("Index");
        }


    }
}
