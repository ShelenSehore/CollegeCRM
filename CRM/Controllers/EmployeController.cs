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
           List<EmployeeViewForModel>  employeeList = new List<EmployeeViewForModel>();
            var getData = _employeeRepository.GetAll();
            if (getData != null) 
            {
                EmployeeViewForModel obj = new EmployeeViewForModel();
                foreach (var row in getData) 
                {
                    obj.Id = row.Id;
                    obj.Name = row.Name;
                    obj.MobileNo = row.MobileNo;
                    obj.Department = row.Department;
                    obj.Designation = row.Designation;
                    obj.Subject = row.Subject;
                    obj.Photo = row.Photo;

                    employeeList.Add(obj);
                }
                
            }

            return View(employeeList);
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
                
                Name = string.IsNullOrEmpty(vm.Name) ? null : vm.Name.ToUpper() ,
                FatherName = string.IsNullOrEmpty(vm.FatherName) ? null : vm.FatherName.ToUpper(),
                MotherName = string.IsNullOrEmpty(vm.MotherName) ? null : vm.MotherName.ToUpper(),
                Designation = string.IsNullOrEmpty(vm.Designation) ? null : vm.Designation.ToUpper(),
                Subject = string.IsNullOrEmpty(vm.Subject) ? null : vm.Subject.ToUpper(),
                MobileNo = string.IsNullOrEmpty(vm.MobileNo) ? null : vm.MobileNo.ToUpper(),
                WhatsupNo = string.IsNullOrEmpty(vm.WhatsupNo) ? null : vm.WhatsupNo.ToUpper(),
                Cast = string.IsNullOrEmpty(vm.Cast) ? null : vm.Cast.ToUpper(),
                EmailAddress = string.IsNullOrEmpty(vm.EmailAddress) ? null : vm.EmailAddress.ToUpper(),
                PanNo = string.IsNullOrEmpty(vm.PanNo) ? null : vm.PanNo.ToUpper(),
                BankName = string.IsNullOrEmpty(vm.BankName) ? null : vm.BankName.ToUpper(),
                AccountNo = string.IsNullOrEmpty(vm.AccountNo) ? null : vm.AccountNo.ToUpper(),
                IFSC = string.IsNullOrEmpty(vm.IFSC) ? null : vm.IFSC.ToUpper(),
                Address = string.IsNullOrEmpty(vm.Address) ? null : vm.Address.ToUpper(),
                PinCode = string.IsNullOrEmpty(vm.PinCode) ? null : vm.PinCode.ToUpper(),
                DOB = vm.DOB,
                Department = string.IsNullOrEmpty(vm.Department) ? null : vm.Department.ToUpper(),
                UP = string.IsNullOrEmpty(vm.UP) ? null : vm.UP.ToUpper(),
                PG = string.IsNullOrEmpty(vm.PG) ? null : vm.PG.ToUpper(),
                BED = string.IsNullOrEmpty(vm.BED) ? null : vm.BED.ToUpper(),
                MED = string.IsNullOrEmpty(vm.MED) ? null : vm.MED.ToUpper(),
                Other1 = string.IsNullOrEmpty(vm.Other1) ? null : vm.Other1.ToUpper(),
                Other2 = string.IsNullOrEmpty(vm.Other2) ? null : vm.Other2.ToUpper(),
                Specialization = string.IsNullOrEmpty(vm.Specialization) ? null : vm.Specialization.ToUpper(),
                TeachingExperience = string.IsNullOrEmpty(vm.TeachingExperience) ? null : vm.TeachingExperience.ToUpper(),
                Code28Designation = string.IsNullOrEmpty(vm.Code28Designation) ? null : vm.Code28Designation.ToUpper(),
                NotificationNo = string.IsNullOrEmpty(vm.NotificationNo) ? null : vm.NotificationNo.ToUpper(),
                Date = vm.Date,
                AppointmentorderNo = string.IsNullOrEmpty(vm.AppointmentorderNo) ? null : vm.AppointmentorderNo.ToUpper(),
                AppointDate = vm.AppointDate,
                JointingDate = vm.JointingDate,
                PayScale = string.IsNullOrEmpty(vm.PayScale) ? null : vm.PayScale.ToUpper(),
                Photo = string.IsNullOrEmpty(vm.Photo) ? null : vm.Photo.ToUpper(),
                ActiveUnactive = string.IsNullOrEmpty(vm.ActiveUnactive) ? null : vm.ActiveUnactive.ToUpper(),
                CollegeName = string.IsNullOrEmpty(vm.CollegeName) ? null : vm.CollegeName.ToUpper(),
            };


            var tee = _employeeRepository.Add(employee);

            
            return Redirect("Index");

            }
            catch (Exception ex)
            { 
            }
            return null;
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _employeeRepository.Delete(id);
            return Json(new { success = true, data = "Success" });
        }

    }
}
