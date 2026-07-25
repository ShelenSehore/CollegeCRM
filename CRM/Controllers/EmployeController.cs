using CRM.Data;
using CRM.Interface;
using CRM.Models;
using CRM.ModelsForView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Controllers
{
    public class EmployeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeController> _logger;
        private readonly AppSettings _mySettings;
        private readonly string _baseUrl;
        public EmployeController(
            IOptions<AppSettings> settings,
            ILogger<EmployeController> logger,
            IEmployeeRepository employeeRepository,
            IOptions<AppSettings> config)
        {
            _mySettings = settings.Value;
            _logger = logger;
            _employeeRepository = employeeRepository;
            _baseUrl = config.Value.BaseUrl;
        }

        public IActionResult Index()
        {
            ViewBag.BaseUrl = _baseUrl;
            var tee = _employeeRepository.GetAll();

            return View();
        }

        public IActionResult Add()
        {
            EmployeeViewForModel model = new EmployeeViewForModel();
            var tee = _employeeRepository.GetAll();

            return View(model);
        }

        [HttpPost]
        public IActionResult AddPost(EmployeeViewForModel vm)
        {
            try {
            
            Employee employee = new Employee
            {
                Name = vm.Name,
                FatherName = vm.FatherName,
                MotherName = vm.MotherName,
                Designation = vm.Designation,
                Subject = vm.Subject,
                MobileNo = vm.MobileNo,
                WhatsupNo = vm.WhatsupNo,
                //Cast = vm.Cast,
               
                //EmailAddress = vm.EmailAddress,
                //PanNo = vm.PanNo,
                //BankName = vm.BankName,
                //AccountNo = vm.AccountNo,
                //IFSC = vm.IFSC,
                //Address = vm.Address,
                //PinCode = vm.PinCode,
                //DOB = vm.DOB,
                //Department = vm.Department,
                //UP = vm.UP,
                //PG = vm.PG,
                //BED = vm.BED,
                //MED = vm.MED,
                //Other1 = vm.Other1,
                //Other2 = vm.Other2,
                //Specialization = vm.Specialization,
                //TeachingExperience = vm.TeachingExperience,
                //Code28Designation = vm.Code28Designation,
                //NotificationNo = vm.NotificationNo,
                //Date = vm.Date,
                //AppointmentorderNo = vm.AppointmentorderNo,
                //AppointDate = vm.AppointDate,
                //JointingDate = vm.JointingDate,
                //PayScale = vm.PayScale,
                //Photo = vm.Photo,
                //ActiveUnactive = vm.ActiveUnactive,
                //CollegeName = vm.CollegeName
            };


            var tee = _employeeRepository.Add(employee);

            
            return Redirect("Index");

            }
            catch (Exception ex)
            { 
            }
            return null;
        }
    }
}
