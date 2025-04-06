using AutoMapper;
using Business_Logic_Layer.Interfaces;
using Business_Logic_Layer.Repositories;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation_Layer.Dtos;
using Presentation_Layer.Helpers;

namespace Presentation_Layer.Controllers
{
    [Authorize]

    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
             _mapper = mapper;
        }


        public async Task<IActionResult> Index(string? Search)
        {
            IEnumerable<Employee> employees;
            if (Search is not null)
            {
                employees = await _unitOfWork.employeeRepository.GetByNameAsync(Search);
                return View(employees);
            }
            else
            {
                employees = await _unitOfWork.employeeRepository.GetAllAsync();
                return View(employees);
            }
           
            //ViewData["Message"]= "Welcome To Employee Page";

            //ViewBag.message = "Welcome To Employee Page";
        }

        public async Task<IActionResult> Search(string? Search)
        {
            IEnumerable<Employee> employees;
            if (Search is not null)
            {
                employees = await _unitOfWork.employeeRepository.GetByNameAsync(Search);
                return PartialView("PartialViews/EmployeeTablePartialView", employees);
            }
            else
            {
                employees = await _unitOfWork.employeeRepository.GetAllAsync();
                return PartialView("PartialViews/EmployeeTablePartialView", employees);
            }

            //ViewData["Message"]= "Welcome To Employee Page";

            //ViewBag.message = "Welcome To Employee Page";
        }
        [HttpGet]

        public async Task<IActionResult> Create()
        {
            var departments = await _unitOfWork.departmentRepository.GetAllAsync();
            ViewData["departments"] = departments;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto employeeDto)
        {
            if (ModelState.IsValid)
            {
                if(employeeDto.Image is not null)
                {
                    employeeDto.ImageName = DocumentSetting.UploadFile(employeeDto.Image, "images");
                }
                
                var employee = _mapper.Map<Employee>(employeeDto);
                await _unitOfWork.employeeRepository.AddAsync(employee);
                var Count =await _unitOfWork.CompleteAsync();
                if (Count > 0)
                {
                    TempData["Message"] = "Employee Added Successfully";
                    return RedirectToAction("Index");
                }
            }
            return View(employeeDto);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");

            var employee = await _unitOfWork.employeeRepository.GetAsync(id.Value);

            if (employee is null) return NotFound(new { StatusCode = 404, Message = $"Employee With Id {id} Is Not Found" });

            return View(ViewName, employee);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var departments =await _unitOfWork.departmentRepository.GetAllAsync();
            ViewData["departments"] = departments;
            if (id is null) return BadRequest("Invalid Id");

            var employee =await _unitOfWork.employeeRepository.GetAsync(id.Value);

            if (employee is null) return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} Is Not Found" });

            //var employeeDto = new CreateEmployeeDto()
            //{
            //    Name = employee.Name,
            //    Age = employee.Age,
            //    Address = employee.Address,
            //    Email = employee.Email,
            //    Phone = employee.Phone,
            //    Salary = employee.Salary,
            //    IsActive = employee.IsActive,
            //    IsDeleted = employee.IsDeleted,
            //    HiringDate = employee.HiringDate,
            //    CreateAt = employee.CreateAt,
            //    DepartmentId = employee.DepartmentId
            //};
            var employeeDto = _mapper.Map<CreateEmployeeDto>(employee);
            return View(employeeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateEmployeeDto employeeDto)
        {
            var employee=await _unitOfWork.employeeRepository.GetAsync(id);
            if (ModelState.IsValid)
            {
                if (employeeDto.ImageName is not null && employeeDto.Image is not null)
                {
                    DocumentSetting.DeleteFile("images", employeeDto.ImageName);
                }
                if (employeeDto.Image is not null)
                {
                    employeeDto.ImageName = DocumentSetting.UploadFile(employeeDto.Image, "images");
                }
                else
                {
                    employeeDto.ImageName = employee.ImageName; // Preserve existing image name
                }
                _mapper.Map(employeeDto, employee);
                employee.Id = id;
                _unitOfWork.employeeRepository.Update(employee);
                var Count =await _unitOfWork.CompleteAsync();
                if (Count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(employeeDto);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit([FromRoute] int id, CreateDepartmentDto department)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var departmentModel = new Department()
        //        {
        //            Id = id,
        //            Code = department.Code,
        //            Name = department.Name,
        //            CreateAt = department.CreateAt
        //        };
        //        var Count = _departmentRepository.Update(departmentModel);
        //        if (Count > 0)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    return View(department);
        //}

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            //if (id is null) return BadRequest("Invalid Id");

            //var department = _departmentRepository.Get(id.Value);

            //if (department is null) return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} Is Not Found" });

            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete([FromRoute] int id, CreateEmployeeDto employeeDto)
        {
            if (ModelState.IsValid)
            {
                
                var employee = _mapper.Map<Employee>(employeeDto);
                employee.Id = id;
                _unitOfWork.employeeRepository.Delete(employee);
                var Count =await _unitOfWork.CompleteAsync();
                if (Count > 0)
                {
                    if (employeeDto.ImageName is not null)
                    {
                        DocumentSetting.DeleteFile("images", employeeDto.ImageName);
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(employeeDto);
        }
    }
}
